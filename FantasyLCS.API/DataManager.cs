using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

using FantasyLCS.DataObjects;
using static StorageManager;
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

                    Match match = new Match();
                    match.ID = matchID;
                    match.FullStats = controller.GetMatchFullStats(matchID);

                    foreach (var fullStat in match.FullStats)
                    {
                        Player matchPlayer = context.Players.Find(fullStat.Name);
                        fullStat.PlayerID = matchPlayer.ID;
                    }

                    context.Matches.Add(match);
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
                        controller.URL = SeasonInfo.PlayerStatsURL.DOMAIN + playerID + SeasonInfo.PlayerStatsURL.FILTER;
                        context.Players.Add(controller.GetPlayer(playerID));
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

}