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
                match.FullStats = controller.GetMatchFullStats(matchID);

                foreach (var fullStat in match.FullStats)
                {
                    Player matchPlayer = existingPlayers.Where(player => player.Name.ToLower().Equals(fullStat.Name.ToLower())).Single();
                    fullStat.PlayerID = matchPlayer.ID;
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

                List<int> playerIDs = controller.GetPlayerIDs(teamID);

                foreach (int playerID in playerIDs)
                {
                    controller.URL = SeasonInfo.PlayerStatsURL.DOMAIN + playerID + SeasonInfo.PlayerStatsURL.FILTER;
                    updatedPlayerData.Add(controller.GetPlayer(playerID));
                }
            }

            WriteData<Player>(updatedPlayerData);
        } 
    }

    public static void CreateTeam(string name)
    {
        List<Team> teams = ReadData<Team>();

        int uniqueID = CreateUniqueIdFromString(name);
        Team team = teams.Where(team => team.ID == uniqueID).SingleOrDefault();

        if (team != null)
        {
            throw new Exception("A roster with that name already exists, you dummy!");
        }

        teams.Add(new Team
        {
            ID = uniqueID,
            Name = name,
            Players = new List<Player>(),
            Subs = new List<Player>(),
        });

        WriteData<Team>(teams);
    }

    public static void AddPlayerToRoster(int rosterID, int playerID)
    {
        List<Team> rosters = ReadData<Team>();
        Player player = Get<Player>(playerID);

        Team roster = rosters.Where(roster => roster.ID == rosterID).Single();

        roster.Players.Add(player);
        WriteData<Team>(rosters);
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