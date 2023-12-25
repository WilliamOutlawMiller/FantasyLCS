using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

using FantasyLCS.DataObjects;
using static StorageManager;
using static StaticMethods;
using System.Xml.Linq;

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

            UpdateData(updatedMatchData);
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

            UpdateData(updatedPlayerData);
        } 
    }

    public static void CreateTeam(string name, string logoUrl, string username)
    {
        List<Team> teams = ReadData<Team>();

        int uniqueID = CreateUniqueIdFromString(name);
        Team team = teams.Where(team => team.ID == uniqueID).SingleOrDefault();

        if (team != null)
        {
            throw new Exception("A team with that name already exists, you dummy!");
        }

        teams.Add(new Team
        {
            ID = uniqueID,
            Name = name,
            OwnerName = username,
            LogoUrl = logoUrl,
            Wins = 0,
            Losses = 0,
            PlayerIDs = new List<int>(),
            SubIDs = new List<int>(),
        });

        WriteData(teams);
    }

    public static void DeleteTeam(string teamName, string ownerName)
    {
        List<Team> teams = ReadData<Team>();

        Team team = teams.Where(team => team.Name == teamName && team.OwnerName == ownerName).SingleOrDefault();

        if (team == null)
        {
            throw new Exception("No team found with that team name and owner name.");
        }

        teams.Remove(team);

        WriteData(teams);
    }

    public static Team GetTeamByUsername(string username)
    {
        List<Team> teams = ReadData<Team>();

        return teams.Where(team => team.OwnerName == username).SingleOrDefault();
    }

    public static void AddPlayerToTeam(int teamID, int playerID)
    {
        Team team = Get<Team>(teamID);
        Player player = Get<Player>(playerID);

        team.PlayerIDs.Add(playerID);
        player.TeamID = teamID;

        UpdateData(team);
        UpdateData(player);
    }

    public static void RemovePlayerFromTeam(int teamID, int playerID)
    {
        Team team = Get<Team>(teamID);
        Player player = Get<Player>(playerID);

        team.PlayerIDs.Remove(playerID);
        player.TeamID = 0;

        UpdateData(team);
        UpdateData(player);
    }

    public static List<Player> GetAvailablePlayers()
    {
        List<Player> players = ReadData<Player>();
        return players.Where(player => player.TeamID == 0).ToList();
    }

    public static int GetTeamID(string name)
    {
        List<Team> teams = ReadData<Team>();
        return teams.FirstOrDefault(team => team.Name.Equals(name)).ID;
    }
}