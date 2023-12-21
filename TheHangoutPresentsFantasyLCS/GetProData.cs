using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;

public class GetProData
{
    
    public static List<int> GetMatchIDs()
    {
        List<int> matchIDs = new List<int>();

        string url = "https://gol.gg/tournament/tournament-matchlist/LCS%20Summer%202023/";

        GolGGController controller = new GolGGController(url);

        return controller.GetMatchIDs();
    }

    public static List<FullStats> GetMatchFullStats()
    {
        List<FullStats> matchFullStats = new List<FullStats>();
        
        string url = "https://gol.gg/game/stats/53263/page-fullstats/";

        GolGGController controller = new GolGGController(url);
        
        return controller.GetMatchFullStats();
    }

    public static List<Player> GetPlayerList()
    { 
        List<Player> players = new List<Player>();
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
                players.Add(controller.GetPlayer(playerID));
            }
        }

        return players;
    }
}