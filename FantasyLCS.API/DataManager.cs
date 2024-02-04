using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyLCS.DataObjects;
using FantasyLCS.API;
using static StaticMethods;
using System.Xml.Linq;
using System.Numerics;
using System.Linq;

public class DataManager
{
    public static void UpdateMatchData(AppDbContext context)
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

    public static void UpdatePlayerList(AppDbContext context)
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

    public static void UpdateScores(AppDbContext context)
    {
        List<Player> players = context.Players.ToList();
        List<FullStat> fullStats = context.FullStats.ToList();
        List<Score> scores = context.Scores.ToList();

        if (players.Count <= 0)
        {
            lock (SharedLockObjects.ExternalDataRefreshLock)
            {
                UpdatePlayerList(context);
                players = context.Players.ToList();
            }
        }

        if (fullStats.Count <= 0)
        {
            lock (SharedLockObjects.ExternalDataRefreshLock)
            {
                UpdateMatchData(context);
                fullStats = context.FullStats.ToList();
            }
        }

        foreach (Player player in players)
        {
            List<FullStat> playerFullStats = fullStats.Where(fs => fs.PlayerID == player.ID).ToList(); 

            foreach (FullStat fullStat in playerFullStats)
            {
                Score score = scores.SingleOrDefault(score => score.MatchID == fullStat.MatchID && score.PlayerID == player.ID);

                if (score == null)
                {
                    lock (SharedLockObjects.ScoresLock)
                    {
                        score = CreateScore(player, fullStat, context);
                    }
                }
            }
        }
    }

    public static Score CreateScore(Player player, FullStat fullStat, AppDbContext context)
    {
        Score score = new Score();

        score.MatchID = fullStat.MatchID;
        score.MatchDate = fullStat.MatchDate;
        score.PlayerID = player.ID;

        if (fullStat.KDA.Equals("Perfect KDA"))
            score.KDAScore = 15;
        else
            score.KDAScore = Convert.ToDouble(fullStat.KDA) * 2;

        score.KPScore = Convert.ToDouble(fullStat.KillPercent.TrimEnd('%')) / 10;

        score.DPMScore = Convert.ToDouble(fullStat.DPM) * 0.01;
        score.VSPMScore = Convert.ToDouble(fullStat.VisionScorePerMinute) * 0.8;

        score.CSD15Score = Convert.ToDouble(fullStat.CSD15) * 0.15;
        score.GD15Score = Convert.ToDouble(fullStat.GD15) * 0.005;

        score.DamageTakenScore = Convert.ToDouble(fullStat.TotalDamageTaken) * 0.00005;
        score.TurretDamageScore = Convert.ToDouble(fullStat.DmgToTurrets) * 0.00025;
        score.TeamHealingScore = Convert.ToDouble(fullStat.TotalHealOnTeammates) * 0.00035;
        score.TeamShieldingScore = Convert.ToDouble(fullStat.TotalDamageShieldedOnTeammates) * 0.0003;

        score.SoloKillScore   = Double.TryParse(fullStat.SoloKills, out var numSoloKills)     ? numSoloKills   * 2 : 0;
        score.DoubleKillScore = Double.TryParse(fullStat.DoubleKills, out var numDoubleKills) ? numDoubleKills * 2 : 0;
        score.TripleKillScore = Double.TryParse(fullStat.TripleKills, out var numTripleKills) ? numTripleKills * 4 : 0;
        score.QuadraKillScore = Double.TryParse(fullStat.QuadraKills, out var numQuadraKills) ? numQuadraKills * 6 : 0;
        score.PentaKillScore  = Double.TryParse(fullStat.PentaKills, out var numPentaKills)   ? numPentaKills  * 8 : 0;

        score.ObjectiveStealScore = Convert.ToDouble(fullStat.ObjectivesStolen) * 5;

        score.CCInstancesScore = Convert.ToDouble(fullStat.TimeCCingOthers) * 0.02;
        score.CCTimeScore = (Convert.ToDouble(fullStat.TotalTimeCCDealt) * 0.000374) + 0.127;

        score.BountyCollectedScore = Convert.ToDouble(fullStat.ShutdownBountyCollected) * 0.004;
        score.BountyLostScore = -Convert.ToDouble(fullStat.ShutdownBountyLost) * 0.004;

        score.FinalScore = score.KDAScore + score.KPScore + score.DPMScore + score.VSPMScore + score.CSD15Score + score.GD15Score 
            + score.DamageTakenScore + score.TurretDamageScore + score.TeamHealingScore + score.TeamShieldingScore + score.SoloKillScore 
            + score.DoubleKillScore + score.TripleKillScore + score.QuadraKillScore + score.PentaKillScore + score.ObjectiveStealScore 
            + score.CCInstancesScore + score.CCTimeScore + score.BountyCollectedScore + score.BountyLostScore;

        context.Scores.Add(score);

        context.SaveChanges();
        return score;
    }

    public static Team CreateTeam(string name, string logoUrl, string username, AppDbContext context)
    {
        User user = context.Users.SingleOrDefault(user => user.Username.ToLower().Equals(username.ToLower()));

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        if (context.Teams.Any(team => team.Name == name))
        {
            throw new Exception("A team with that name already exists.");
        }
            
        if ((user.TeamID != null && user.TeamID > 0) || context.Teams.Any(team => team.OwnerName.ToLower().Equals(username.ToLower())))
        {
            throw new Exception("User already has a team.");
        }

        var newTeam = new Team
        {
            Name = name,
            OwnerName = username,
            LogoUrl = logoUrl,
            Wins = 0,
            Losses = 0,
            DraftPlayerIDs = new List<int>()
        };

        context.Teams.Add(newTeam);
        context.SaveChanges();

        user.TeamID = newTeam.ID;
        context.SaveChanges();

        return newTeam;
    }

    public static void DeleteTeam(string teamName, string ownerName, AppDbContext context)
    {
        var team = context.Teams.SingleOrDefault(team => team.Name == teamName && team.OwnerName == ownerName);

        if (team == null)
        {
            throw new Exception("No team found with that team name and owner name.");
        }

        context.Teams.Remove(team);
        context.SaveChanges();
    }

    public static League CreateLeague(string leagueName, string leagueOwner, AppDbContext context)
    {
        User user = context.Users.SingleOrDefault(user => user.Username.ToLower().Equals(leagueOwner.ToLower()));
        List<League> leagues = context.Leagues.ToList();

        if (leagues.Any(league => league.Name.Equals(leagueName)))
        {
            throw new Exception("A team with that name already exists.");
        }

        if (leagues.Any(league => league.UserIDs.Contains(user.ID)))
        {
            throw new Exception("This user is already in a league.");
        }

        string joinCode = GenerateUniqueCode(context.Leagues);

        var newLeague = new League
        {
            Name = leagueName,
            Owner = leagueOwner,
            JoinCode = joinCode,
            UserIDs = new List<int> { user.ID },
            LeagueStatus = LeagueStatus.NotStarted
        };

        context.Leagues.Add(newLeague);
        context.SaveChanges();

        user.LeagueID = newLeague.ID;
        context.SaveChanges();

        return newLeague;
    }

    private static string GenerateUniqueCode(DbSet<League> leagues)
    {
        var random = new Random();
        var uniqueCode = "";
        do
        {
            uniqueCode = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        while (leagues.Any(league => league.JoinCode == uniqueCode));

        return uniqueCode;
    }

    public static League JoinLeague(string username, string joinCode, AppDbContext context)
    {
        User user = context.Users.SingleOrDefault(user => user.Username.ToLower().Equals(username.ToLower()));
        List<League> leagues = context.Leagues.ToList();
        League league = leagues.SingleOrDefault(league => league.JoinCode.ToLower().Equals(joinCode.ToLower()));

        if (league == null)
        {
            throw new Exception("Invalid join code.");
        }

        if (leagues.Any(league => league.UserIDs.Contains(user.ID)))
        {
            throw new Exception("This user is already in a league.");
        }

        if (league.UserIDs.Count >= 8)
        {
            throw new Exception("This league already has eight players.");
        }

        league.UserIDs.Add(user.ID);
        context.SaveChanges();

        user.LeagueID = league.ID;
        context.SaveChanges();

        return league;
    }

    public static void RemoveUserFromLeague(string username, AppDbContext context)
    {
        User user = context.Users.SingleOrDefault(user => user.Username.ToLower().Equals(username.ToLower()));
        League league = context.Leagues.SingleOrDefault(league => league.ID == user.LeagueID);

        if (user == null)
        {
            throw new Exception("User not found.");
        }
        if (league == null)
        {
            throw new Exception("League not found.");
        }

        user.LeagueID = null;
        league.UserIDs.Remove(user.ID);

        if (league.UserIDs.Count == 0)
            context.Leagues.Remove(league);

        else if (league.Owner.ToLower().Equals(user.Username.ToLower()))
        {
            int newOwnerID = league.UserIDs.First();
            User newOwner = context.Users.SingleOrDefault(user => user.ID == newOwnerID);
            league.Owner = newOwner.Username;
        }

        context.SaveChanges();
    }

    public static List<Score> GetLeagueMatchScores(int id, AppDbContext context)
    {
        List<Score> leagueMatchScores = new List<Score>();

        // Get all scores and fullstats objects for use later
        List<Score> scores = context.Scores.ToList();
        List<FullStat> fullStats = context.FullStats.ToList();

        LeagueMatch leagueMatch = context.LeagueMatches
            .Include(lm => lm.TeamOne)
            .Include(lm => lm.TeamTwo)
            .FirstOrDefault(leagueMatch => leagueMatch.ID == id);

        if (leagueMatch == null)
            throw new Exception("League match not found.");

        var draftPlayerIDs = new List<int>();
        draftPlayerIDs.AddRange(leagueMatch.TeamOne.DraftPlayerIDs);
        draftPlayerIDs.AddRange(leagueMatch.TeamTwo.DraftPlayerIDs);

        // Get all draft players that are in the LeagueMatch (business data grouping of players associated with your fantasy league teams)
        List<DraftPlayer> draftPlayers = context.DraftPlayers.Where(dp => draftPlayerIDs.Contains(dp.ID)).ToList();

        // Pair business object with real world data object

        List<Tuple<DraftPlayer, Player>> playerObjectAssociations = new List<Tuple<DraftPlayer, Player>>();
        foreach (var draftPlayer in draftPlayers)
        {
            // Get the real world player object that have the same name. It should be a 1 to 1 ratio, as each league should only contain one instance of any DraftPlayer
            Player player = context.Players.SingleOrDefault(player => player.Name.ToLower().Equals(draftPlayer.Name.ToLower()));
            playerObjectAssociations.Add(new Tuple<DraftPlayer, Player>(draftPlayer, player));
        }

        // objectAssociation stores Item1 = DraftPlayer, Item2 = Player
        foreach (var objectAssociation in playerObjectAssociations)
        {
            DraftPlayer draftPlayer = objectAssociation.Item1;
            Player player = objectAssociation.Item2;

            FullStat fullStat = fullStats.FirstOrDefault(fullStat => fullStat.PlayerID == player.ID && fullStat.MatchDate == leagueMatch.MatchDate);
            if (fullStat == null)
            {
                int substitutePlayerID = Substitutes.SubstitutePlayerIDs[player.ID];
                if (substitutePlayerID != null)
                {
                    fullStat = fullStats.FirstOrDefault(fullStat => fullStat.PlayerID == substitutePlayerID && fullStat.MatchDate == leagueMatch.MatchDate);
                }
                else
                {
                    lock (SharedLockObjects.ExternalDataRefreshLock)
                    {
                        DataManager.UpdatePlayerList(context);
                        DataManager.UpdateMatchData(context);
                        fullStats = context.FullStats.ToList();
                    }

                    fullStat = fullStats.FirstOrDefault(fullStat => fullStat.PlayerID == player.ID);
                }
            }

            Score score = scores.FirstOrDefault(score => score.PlayerID == player.ID && score.MatchDate == fullStat.MatchDate);
            if (score == null)
            {
                int substitutePlayerID = Substitutes.SubstitutePlayerIDs[player.ID];
                if (substitutePlayerID != null)
                {
                    score = scores.FirstOrDefault(score => score.PlayerID == substitutePlayerID && score.MatchDate == fullStat.MatchDate);
                }
                else
                {
                    lock (SharedLockObjects.ScoresLock)
                    {
                        // The score will be the same for each player, regardless of their fantasy league-specific team
                        DataManager.UpdateScores(context);
                        scores = context.Scores.ToList();
                    }

                    score = scores.FirstOrDefault(score => score.PlayerID == player.ID && score.MatchDate == fullStat.MatchDate);
                }
            }

            leagueMatchScores.Add(score);
        }

        return leagueMatchScores;
    }

    public static void AddPlayerToTeam(int teamID, int playerID, AppDbContext context)
    {
        var team = context.Teams.Find(teamID);
        var player = context.Players.Find(playerID);

        if (team != null && player != null)
        {
            team.DraftPlayerIDs.Add(playerID);
            player.TeamID = teamID;

            context.SaveChanges();
        }
    }

    public static void RemovePlayerFromTeam(int teamID, int playerID, AppDbContext context)
    {
        var team = context.Teams.Find(teamID);
        var player = context.Players.Find(playerID);

        if (team != null && player != null)
        {
            team.DraftPlayerIDs.Remove(playerID);
            player.TeamID = 0;

            context.SaveChanges();
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