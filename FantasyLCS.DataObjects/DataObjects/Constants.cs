namespace Constants
{
    public class SeasonInfo
    {
        // todo: update these to Spring 2024 when data becomes available
        public const string SEASON = "season-S13";
        public const string SPLIT = "split-Summer";
        public const string TOURNAMENT = "LCS%20Summer%202023";

        public class TeamListURL
        {
            public const string DOMAIN = $"https://gol.gg/teams/list/{SEASON}/{SPLIT}/tournament-{TOURNAMENT}/";
        }

        public class TeamStatsURL
        {
            public const string DOMAIN = "https://gol.gg/teams/team-stats/";
            public const string FILTER = $"/{SPLIT}/tournament-{TOURNAMENT}/";
        }

        public class PlayerStatsURL
        {
            public const string DOMAIN = "https://gol.gg/players/player-stats/";
            public const string FILTER = $"/{SEASON}/{SPLIT}/tournament-{TOURNAMENT}/champion-ALL/";
        }

        public class MatchListURL
        {
            public const string DOMAIN = $"https://gol.gg/tournament/tournament-matchlist/{TOURNAMENT}/";
        }

        public class GameURL
        {
            public const string DOMAIN = $"https://gol.gg/game/stats/";
            public const string FILTER = $"/page-fullstats/";
        }
    }

    
}

public class GolGGConstants
{
    public const string FULLSTATS = "//table[contains(@class, 'completestats')]";
    public const string MATCHLIST = "//table[contains(@class, 'table_list') and caption[contains(., 'results')]]";
    public const string TEAMLIST = "//div[contains(@class, 'col-12') and contains(@class, 'p-4')]/descendant::table";
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
        {"PlayerName"        , "//div[contains(@class, 'col-12') and contains(@class, 'mt-4')]/descendant::h1"           },
        {"GeneralStats"      , "//table[@class='table_list' and caption[contains(., 'stats')]]"                          },
        {"ChampionStats"     , "//table[contains(@class, 'table_list') and caption[contains(., 'champion pool')]]"       },
        {"AggressionStats"   , "//table[@class='table_list' and caption[contains(., 'damage and kill stats')]]"          },
        {"EarlyGameStats"    , "//table[@class='table_list' and caption[contains(., 'laning stats')]]"                   },
        {"VisionStats"       , "//table[@class='table_list' and caption[contains(., 'vision stats')]]"                   }
    };
}