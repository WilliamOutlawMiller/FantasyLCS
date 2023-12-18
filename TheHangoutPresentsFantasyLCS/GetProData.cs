using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;

public class GetProData
{
    public static Match GetMatch()
    {
        Match match = new Match();
        // string url = SeasonInfo.DOMAIN + SeasonInfo.SEASON + SeasonInfo.YEAR; 
        string url = "https://gol.gg/game/stats/53263/page-fullstats/";

        GolGGController controller = new GolGGController();
        match.PlayerStats = controller.GetMatchFullStats(url);
        return match;
    }
}