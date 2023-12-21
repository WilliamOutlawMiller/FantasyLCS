using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;
using static StorageManager;
using Microsoft.AspNetCore.Mvc;

public class UpdateData
{
    public static void UpdateMatchData()
    {
        List<Match> existingMatchData = new List<Match>();
        List<Match> updatedMatchData = new List<Match>();
        List<Player> existingPlayers = ReadData<Player>();

        if (ShouldRefreshData<Match>())
        {
            GolGGController controller = new GolGGController();

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

            WriteData(updatedMatchData);
        }
    }

    public static void UpdatePlayerList()
    { 
        List<Player> existingPlayerData = new List<Player>();
        List<Player> updatedPlayerData = new List<Player>();
        
        if (ShouldRefreshData<Player>())
        {
            GolGGController controller = new GolGGController();

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

            WriteData(updatedPlayerData);
        } 
    }

    public static Player GetPlayer(int id)
    {
        List<Player> players = ReadData<Player>();
        return players.Where(player => player.ID == id).Single();
    }
}