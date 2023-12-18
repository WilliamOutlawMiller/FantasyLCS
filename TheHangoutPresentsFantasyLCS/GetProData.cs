using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;

public class GetProData
{
    public static Match GetMatch()
    {
        // todo: right now this is reading the fullstats page of one game. This will need to be refactored for BO5 that reads from multiple matches
        
        Match match = new Match();
        // string url = SeasonInfo.DOMAIN + SeasonInfo.SEASON + SeasonInfo.YEAR; 
        string url = "https://gol.gg/game/stats/53263/page-fullstats/";

        GolGGController controller = new GolGGController();
        match.PlayerStats = controller.GetMatchFullStats(url);
        return match;
    }
}