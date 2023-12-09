using HtmlAgilityPack;
using Constants;

public class GetProDataFromWiki
{
    public static string GetMatchPicksAndBans()
    {
        string url = SeasonInfo.DOMAIN + SeasonInfo.SEASON_YEAR + SeasonInfo.SEASON + MatchInfo.PICKS_AND_BANS; 



        return url;
    }
}