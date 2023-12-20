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

    
}

public class GolGGConstants
{
    public const string FULLSTATS = "//table[contains(@class, 'completestats')]";

    public static Dictionary<string,string> TeamStats = new Dictionary<string, string>
    {
        {"TeamSummary"       , "//table[@class='table_list' and caption[contains(., 'stats')]]"                          },
        {"BannedBy"          , "//table[@class='table_list' and caption[contains(.,'Most banned champions by')]]"        },
        {"BannedAgainst"     , "//table[@class='table_list' and caption[contains(., 'Most banned champions against')]]"  },
        {"Economy"           , "//table[@class='table_list' and caption[contains(., 'gold and farm stats')]]"            },
        {"Aggression"        , "//table[@class='table_list' and caption[contains(., 'damage and kills stats')]]"         },
        {"Objectives"        , "//table[@class='table_list' and caption[contains(., 'objectives control stats')]]"       },
        {"Vision"            , "//table[@class='table_list' and caption[contains(., 'vision stats')]]"                   },
        {"Roster"            , "//table[contains(@class, 'table_list') and caption[contains(., 'player')]]"              }
    };

    public static Dictionary<string,string> PlayerStats = new Dictionary<string, string>
    {
        {"GeneralStats"      , "//table[@class='table_list' and caption[contains(., 'stats')]]"                          },
        {"ChampionStats"     , "//table[contains(@class, 'table_list') and caption[contains(., 'champion pool')]]"       },
        {"AggressionStats"   , "//table[@class='table_list' and caption[contains(., 'damage and kill stats')]]"          },
        {"EarlyGameStats"    , "//table[@class='table_list' and caption[contains(., 'laning stats')]]"                   },
        {"VisionStats"       , "//table[@class='table_list' and caption[contains(., 'vision stats')]]"                   }
    };
}