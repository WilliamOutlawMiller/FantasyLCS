using HtmlAgilityPack;

namespace Constants
{
    public class SeasonInfo
    {
        // These constants are used for url construction. The lol.fandom.com url follows this format:
        // https://lol.fandom.com/wiki/LCS/2023_Season/Spring_Season/

        public const string DOMAIN = "https://gol.gg/tournament/tournament-matchlist/";
        public const string SEASON = "LCS%20Spring%20";
        public const string YEAR = "2023/";
    }

    public class GolGGXPaths
    {
        public const string FULLSTATS = "//table[contains(@class, 'completestats')]";
    }
}