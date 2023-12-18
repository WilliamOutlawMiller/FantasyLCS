using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;

public class GetProDataFromWiki
{
    public static JsonArray GetMatchPicksAndBans()
    {
        // string url = SeasonInfo.DOMAIN + SeasonInfo.SEASON + SeasonInfo.YEAR; 
        string url = "https://gol.gg/game/stats/53263/page-fullstats/";

        MatchController controller = new MatchController();
        
        return controller.GetMatchPicksAndBans(url);
    }
}