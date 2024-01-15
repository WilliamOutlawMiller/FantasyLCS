using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyLCS.DataObjects;
using static StaticMethods;
using System.Xml.Linq;
using System.Numerics;

public class DataManager
{
    public static void UpdateMatchData()
    {
        using (var context = new AppDbContext())
        {
            if (ShouldRefreshData<Match>(context))
            {
                GolGGScraper controller = new GolGGScraper();

                controller.URL = SeasonInfo.MatchListURL.DOMAIN;
                List<int> matchIDs = controller.GetMatchIDs();

                // todo: add functionality to handle for playoffs/championship BO5
                foreach (int matchID in matchIDs)
                {
                    controller.URL = SeasonInfo.GameURL.DOMAIN + matchID + SeasonInfo.GameURL.FILTER;

                    Match existingMatch = context.Matches.FirstOrDefault(m => m.ID == matchID);
                    Match match = new Match();
                    match.ID = matchID;
                    match.FullStats = controller.GetMatchFullStats(matchID);
                    
                    foreach (var fullStat in match.FullStats)
                    {
                        fullStat.PlayerID = context.Players.FirstOrDefault(p => p.Name.Equals(fullStat.Name)).ID;
                    }

                    // Map existing FullStats to Players
                    if (existingMatch != null)
                    {
                        foreach (var fullStat in match.FullStats)
                        {
                            // Check if a FullStats entry with the same MatchID and PlayerID already exists
                            var existingFullStat = context.FullStats.FirstOrDefault(e => e.MatchID == fullStat.MatchID && e.PlayerID == fullStat.PlayerID);

                            if (existingFullStat != null)
                            {
                                // Update the existing entry
                                context.Entry(existingFullStat).CurrentValues.SetValues(fullStat);
                            }
                            else
                            {
                                // Add a new entry if it doesn't exist
                                context.FullStats.Add(fullStat);
                            }
                        }

                        context.Entry(existingMatch).CurrentValues.SetValues(match);
                    }
                    else
                    {
                        context.Matches.Add(match);
                    }
                }

                context.SaveChanges();
            }
        }
    }

    public static void UpdatePlayerList()
    {
        using (var context = new AppDbContext())
        {
            if (ShouldRefreshData<Player>(context))
            {
                GolGGScraper controller = new GolGGScraper();

                controller.URL = SeasonInfo.TeamListURL.DOMAIN;
                List<int> teamIDs = controller.GetTeamIDs();

                foreach (int teamID in teamIDs)
                {
                    controller.URL = SeasonInfo.TeamStatsURL.DOMAIN + teamID + SeasonInfo.TeamStatsURL.FILTER;

                    List<int> playerIDs = controller.GetPlayerIDs(teamID);

                    foreach (int playerID in playerIDs)
                    {
                        Player existingPlayer = context.Players
                            .Include(p => p.GeneralStats)
                            .Include(p => p.AggressionStats)
                            .Include(p => p.EarlyGameStats)
                            .Include(p => p.ChampionStats)
                            .Include(p => p.VisionStats)
                            .FirstOrDefault(player => player.ID == playerID);

                        controller.URL = SeasonInfo.PlayerStatsURL.DOMAIN + playerID + SeasonInfo.PlayerStatsURL.FILTER;
                        Player currentPlayer = controller.GetPlayer(playerID);

                        if (existingPlayer != null)
                        {
                            context.Entry(existingPlayer).CurrentValues.SetValues(currentPlayer);
                            context.Entry(existingPlayer.GeneralStats).CurrentValues.SetValues(currentPlayer.GeneralStats);
                            context.Entry(existingPlayer.AggressionStats).CurrentValues.SetValues(currentPlayer.AggressionStats);
                            context.Entry(existingPlayer.EarlyGameStats).CurrentValues.SetValues(currentPlayer.EarlyGameStats);
                            context.Entry(existingPlayer.VisionStats).CurrentValues.SetValues(currentPlayer.VisionStats);

                            foreach (var currentChampionStat in currentPlayer.ChampionStats)
                            {
                                var existingChampionStat = existingPlayer.ChampionStats.FirstOrDefault(cs =>
                                    cs.ChampionID == currentChampionStat.ChampionID);

                                if (existingChampionStat != null)
                                {
                                    context.Entry(existingChampionStat).CurrentValues.SetValues(currentChampionStat);
                                }
                                else
                                {
                                    existingPlayer.ChampionStats.Add(currentChampionStat);
                                }
                            }


                        }
                        else
                            context.Add(currentPlayer);
                    }
                }

                context.SaveChanges();
            }
        }
    }

    public static void CreateTeam(string name, string logoUrl, string username)
    {
        using (var context = new AppDbContext())
        {
            if (context.Teams.Any(team => team.Name == name))
            {
                throw new Exception("A team with that name already exists.");
            }

            var newTeam = new Team
            {
                Name = name,
                OwnerName = username,
                LogoUrl = logoUrl,
                Wins = 0,
                Losses = 0,
                PlayerIDs = new List<int>()
            };

            context.Teams.Add(newTeam);
            context.SaveChanges();
        }
    }

    public static void DeleteTeam(string teamName, string ownerName)
    {
        using (var context = new AppDbContext())
        {
            var team = context.Teams.SingleOrDefault(team => team.Name == teamName && team.OwnerName == ownerName);

            if (team == null)
            {
                throw new Exception("No team found with that team name and owner name.");
            }

            context.Teams.Remove(team);
            context.SaveChanges();
        }
    }

    public static void CreateLeague(string leagueName, string leagueOwner)
    {
        using (var context = new AppDbContext())
        {
            if (context.Leagues.Any(league => league.Name.Equals(leagueName)))
            {
                throw new Exception("A team with that name already exists.");
            }

            if (context.Leagues.Any(league => league.Owner.Equals(leagueOwner)))
            {
                throw new Exception("This user already owns a league.");
            }

            var newLeague = new League
            {
                Name = leagueName,
                Owner = leagueOwner,
                Users = new List<User>()
            };

            context.Leagues.Add(newLeague);
            context.SaveChanges();
        }
    }

    public static void AddPlayerToTeam(int teamID, int playerID)
    {
        using (var context = new AppDbContext())
        {
            var team = context.Teams.Find(teamID);
            var player = context.Players.Find(playerID);

            if (team != null && player != null)
            {
                team.PlayerIDs.Add(playerID);
                player.TeamID = teamID;

                context.SaveChanges();
            }
        }
    }

    public static void RemovePlayerFromTeam(int teamID, int playerID)
    {
        using (var context = new AppDbContext())
        {
            var team = context.Teams.Find(teamID);
            var player = context.Players.Find(playerID);

            if (team != null && player != null)
            {
                team.PlayerIDs.Remove(playerID);
                player.TeamID = 0;

                context.SaveChanges();
            }
        }
    }

    public static bool ShouldRefreshData<T>(AppDbContext context)
    {
        string dataType = typeof(T).Name;
        var lastUpdate = context.DataUpdateLogs
                                 .Where(log => log.DataType == dataType)
                                 .OrderByDescending(log => log.LastUpdated)
                                 .Select(log => log.LastUpdated)
                                 .FirstOrDefault();

        return DateTime.Now - lastUpdate >= TimeSpan.FromDays(7);
    }
}