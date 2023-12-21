using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

using static StorageManager;
using static StaticMethods;

public class DataManager
{
    public static void UpdateMatchData()
    {
        List<Match> existingMatchData = new List<Match>();
        List<Match> updatedMatchData = new List<Match>();
        List<Player> existingPlayers = ReadData<Player>();

        if (ShouldRefreshData<Match>())
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
                match.Players = new List<Player>();
                match.FullStats = controller.GetMatchFullStats(matchID);

                foreach (var fullStat in match.FullStats)
                {
                    Player matchPlayer = existingPlayers.Where(player => player.Name.ToLower().Equals(fullStat.Name.ToLower())).Single();
                    fullStat.PlayerID = matchPlayer.ID;
                    match.Players.Add(matchPlayer);
                }

                updatedMatchData.Add(match);
            }

            WriteData<Match>(updatedMatchData);
        }
    }

    public static void UpdatePlayerList()
    { 
        List<Player> existingPlayerData = new List<Player>();
        List<Player> updatedPlayerData = new List<Player>();
        
        if (ShouldRefreshData<Player>())
        {
            GolGGScraper controller = new GolGGScraper();

            controller.URL = SeasonInfo.TeamListURL.DOMAIN;
            List<int> teamIDs = controller.GetTeamIDs();

            foreach (int teamID in teamIDs)
            {
                controller.URL = SeasonInfo.TeamStatsURL.DOMAIN + teamID + SeasonInfo.TeamStatsURL.FILTER;

                Team team = new Team();
                team = controller.GetTeam(teamID);

                foreach (int playerID in team.Roster.PlayerIDs)
                {
                    controller.URL = SeasonInfo.PlayerStatsURL.DOMAIN + playerID + SeasonInfo.PlayerStatsURL.FILTER;
                    updatedPlayerData.Add(controller.GetPlayer(playerID));
                }
            }

            WriteData<Player>(updatedPlayerData);
        } 
    }

    public static void CreateRoster(string name)
    {
        List<Roster> rosters = ReadData<Roster>();

        rosters.Add(new Roster
        {
            ID = CreateUniqueIdFromString(name),
            Name = name,
            Players = new List<Player>(),
            Subs = new List<Player>(),
        });

        WriteData<Roster>(rosters);
    }

    public static void AddPlayerToRoster(int rosterID, int playerID)
    {
        List<Roster> rosters = ReadData<Roster>();
        Player player = Get<Player>(playerID);

        Roster roster = rosters.Where(roster => roster.ID == rosterID).Single();

        roster.Players.Add(player);
        WriteData<Roster>(rosters);
    }
    
    public static T Get<T>(int id) where T : class
    {
        List<T> data = ReadData<T>();

        return data.FirstOrDefault(item => 
        {
            var idProperty = item.GetType().GetProperty("ID");
            if (idProperty != null)
            {
                var value = idProperty.GetValue(item);
                return value != null && (int)value == id;
            }
            return false;
        });
    }
}