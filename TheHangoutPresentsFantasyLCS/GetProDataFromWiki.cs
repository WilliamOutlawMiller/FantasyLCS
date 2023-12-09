using HtmlAgilityPack;
using Constants;
using Microsoft.AspNetCore.Mvc;

public class GetProDataFromWiki
{
    public static string GetMatchPicksAndBans()
    {
        string url = SeasonInfo.DOMAIN + SeasonInfo.SEASON_YEAR + SeasonInfo.SEASON + MatchInfo.PICKS_AND_BANS; 

        MatchController controller = new MatchController();
        ViewResult webpage = controller.View(url);
        return url;
    }
}