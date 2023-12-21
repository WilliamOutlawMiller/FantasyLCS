using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;
using static StorageManager;

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
            return updatedPlayerData;
        }
        else 
        {
            existingPlayerData = ReadData<Player>();
            return existingPlayerData;
        }       
    }
}