using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;

public class GetProData
{
    public static List<PlayerStats> GetFullStatsForMatch()
    {
        // string url = SeasonInfo.DOMAIN + SeasonInfo.SEASON + SeasonInfo.YEAR; 
        string url = "https://gol.gg/game/stats/53263/page-fullstats/";

        GolGGController controller = new GolGGController();
        List<PlayerStats> playerStats = controller.GetMatchFullStats(url);
        return playerStats;
    }
}