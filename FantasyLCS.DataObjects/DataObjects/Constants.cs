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

    public class DraftPlayerConstants
    {
        public static readonly List<DraftPlayer> DraftPlayers = new List<DraftPlayer>
        {
            // Top Players
            new DraftPlayer { ID = 1, Name = "Sniper", Position = Position.Top, Drafted = false },
            new DraftPlayer { ID = 2, Name = "Fudge", Position = Position.Top, Drafted = false },
            new DraftPlayer { ID = 3, Name = "Rich", Position = Position.Top, Drafted = false },
            new DraftPlayer { ID = 4, Name = "Bwipo", Position = Position.Top, Drafted = false },
            new DraftPlayer { ID = 5, Name = "Castle", Position = Position.Top, Drafted = false },
            new DraftPlayer { ID = 6, Name = "Dhokla", Position = Position.Top, Drafted = false },
            new DraftPlayer { ID = 7, Name = "FakeGod", Position = Position.Top, Drafted = false },
            new DraftPlayer { ID = 8, Name = "Impact", Position = Position.Top, Drafted = false },

            // Jungle Players
            new DraftPlayer { ID = 9, Name = "River", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { ID = 10, Name = "Blaber", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { ID = 11, Name = "eXyu", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { ID = 12, Name = "Inspired", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { ID = 13, Name = "Armao", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { ID = 14, Name = "Contractz", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { ID = 15, Name = "Bugi", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { ID = 16, Name = "UmTi", Position = Position.Jungle, Drafted = false },
            
            // Mid Players
            new DraftPlayer { ID = 17, Name = "Quid", Position = Position.Mid, Drafted = false },
            new DraftPlayer { ID = 18, Name = "jojopyun", Position = Position.Mid, Drafted = false },
            new DraftPlayer { ID = 19, Name = "Dove", Position = Position.Mid, Drafted = false },
            new DraftPlayer { ID = 20, Name = "Jensen", Position = Position.Mid, Drafted = false },
            new DraftPlayer { ID = 21, Name = "Mask", Position = Position.Mid, Drafted = false },
            new DraftPlayer { ID = 22, Name = "Palafox", Position = Position.Mid, Drafted = false },
            new DraftPlayer { ID = 23, Name = "Insanity", Position = Position.Mid, Drafted = false },
            new DraftPlayer { ID = 24, Name = "APA", Position = Position.Mid, Drafted = false },
        
            // Bot Players
            new DraftPlayer { ID = 25, Name = "Meech", Position = Position.Bot, Drafted = false },
            new DraftPlayer { ID = 26, Name = "Berserker", Position = Position.Bot, Drafted = false },
            new DraftPlayer { ID = 27, Name = "Tomo", Position = Position.Bot, Drafted = false },
            new DraftPlayer { ID = 28, Name = "Massu", Position = Position.Bot, Drafted = false },
            new DraftPlayer { ID = 29, Name = "Tactical", Position = Position.Bot, Drafted = false },
            new DraftPlayer { ID = 30, Name = "FBI", Position = Position.Bot, Drafted = false },
            new DraftPlayer { ID = 31, Name = "Bvoy", Position = Position.Bot, Drafted = false },
            new DraftPlayer { ID = 32, Name = "Yeon", Position = Position.Bot, Drafted = false },
        
            // Support Players
            new DraftPlayer { ID = 33, Name = "Eyla", Position = Position.Support, Drafted = false },
            new DraftPlayer { ID = 34, Name = "VULCAN", Position = Position.Support, Drafted = false },
            new DraftPlayer { ID = 35, Name = "Isles", Position = Position.Support, Drafted = false },
            new DraftPlayer { ID = 36, Name = "Busio", Position = Position.Support, Drafted = false },
            new DraftPlayer { ID = 37, Name = "Olleh", Position = Position.Support, Drafted = false },
            new DraftPlayer { ID = 38, Name = "huhi", Position = Position.Support, Drafted = false },
            new DraftPlayer { ID = 39, Name = "Zeyzal", Position = Position.Support, Drafted = false },
            new DraftPlayer { ID = 40, Name = "CoreJJ", Position = Position.Support, Drafted = false },
        };
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