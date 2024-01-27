namespace Constants
{
    public class SeasonInfo
    {
        // todo: update these to Spring 2024 when data becomes available
        public const string SEASON = "season-S14";
        public const string SPLIT = "split-Spring";
        public const string TOURNAMENT = "LCS%20Spring%202024";

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
            new DraftPlayer { Name = "Sniper", Position = Position.Top, Drafted = false },
            new DraftPlayer { Name = "Fudge", Position = Position.Top, Drafted = false },
            new DraftPlayer { Name = "Rich", Position = Position.Top, Drafted = false },
            new DraftPlayer { Name = "Bwipo", Position = Position.Top, Drafted = false },
            new DraftPlayer { Name = "Castle", Position = Position.Top, Drafted = false },
            new DraftPlayer { Name = "Dhokla", Position = Position.Top, Drafted = false },
            new DraftPlayer { Name = "FakeGod", Position = Position.Top, Drafted = false },
            new DraftPlayer { Name = "Impact", Position = Position.Top, Drafted = false },

            // Jungle Players
            new DraftPlayer { Name = "River", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { Name = "Blaber", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { Name = "eXyu", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { Name = "Inspired", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { Name = "Armao", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { Name = "Contractz", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { Name = "Bugi", Position = Position.Jungle, Drafted = false },
            new DraftPlayer { Name = "UmTi", Position = Position.Jungle, Drafted = false },
            
            // Mid Players
            new DraftPlayer { Name = "Quid", Position = Position.Mid, Drafted = false },
            new DraftPlayer { Name = "jojopyun", Position = Position.Mid, Drafted = false },
            new DraftPlayer { Name = "Dove", Position = Position.Mid, Drafted = false },
            new DraftPlayer { Name = "Jensen", Position = Position.Mid, Drafted = false },
            new DraftPlayer { Name = "Mask", Position = Position.Mid, Drafted = false },
            new DraftPlayer { Name = "Palafox", Position = Position.Mid, Drafted = false },
            new DraftPlayer { Name = "Insanity", Position = Position.Mid, Drafted = false },
            new DraftPlayer { Name = "APA", Position = Position.Mid, Drafted = false },
        
            // Bot Players
            new DraftPlayer { Name = "Meech", Position = Position.Bot, Drafted = false },
            new DraftPlayer { Name = "Berserker", Position = Position.Bot, Drafted = false },
            new DraftPlayer { Name = "Tomo", Position = Position.Bot, Drafted = false },
            new DraftPlayer { Name = "Massu", Position = Position.Bot, Drafted = false },
            new DraftPlayer { Name = "Tactical", Position = Position.Bot, Drafted = false },
            new DraftPlayer { Name = "FBI", Position = Position.Bot, Drafted = false },
            new DraftPlayer { Name = "Bvoy", Position = Position.Bot, Drafted = false },
            new DraftPlayer { Name = "Yeon", Position = Position.Bot, Drafted = false },
        
            // Support Players
            new DraftPlayer { Name = "Eyla", Position = Position.Support, Drafted = false },
            new DraftPlayer { Name = "VULCAN", Position = Position.Support, Drafted = false },
            new DraftPlayer { Name = "Isles", Position = Position.Support, Drafted = false },
            new DraftPlayer { Name = "Busio", Position = Position.Support, Drafted = false },
            new DraftPlayer { Name = "Olleh", Position = Position.Support, Drafted = false },
            new DraftPlayer { Name = "huhi", Position = Position.Support, Drafted = false },
            new DraftPlayer { Name = "Zeyzal", Position = Position.Support, Drafted = false },
            new DraftPlayer { Name = "CoreJJ", Position = Position.Support, Drafted = false },
        };
    }

    public class MatchScheduleConstants
    {
        public static Dictionary<int, Tuple<Tuple<string,string>,DateTime>> MatchSchedule = new Dictionary<int, Tuple<Tuple<string, string>, DateTime>>
        {
            { 0, new (new ("Week 1", "Day 1"), new DateTime(2024, 1, 20)) },
            { 1, new (new ("Week 1", "Day 2"), new DateTime(2024, 1, 21)) },
            { 2, new (new ("Week 2", "Day 1"), new DateTime(2024, 1, 27)) },
            { 3, new (new ("Week 2", "Day 2"), new DateTime(2024, 1, 28)) },
            { 4, new (new ("Week 3", "Day 1"), new DateTime(2024, 2, 2))  },
            { 5, new (new ("Week 3", "Day 2"), new DateTime(2024, 2, 3))  },
            { 6, new (new ("Week 3", "Day 3"), new DateTime(2024, 2, 4))  },
            { 7, new (new ("Week 4", "Day 1"), new DateTime(2024, 2, 10)) },
            { 8, new (new ("Week 4", "Day 2"), new DateTime(2024, 2, 11)) },
            { 9, new (new ("Week 5", "Day 1"), new DateTime(2024, 3, 2)) },
            { 10, new (new ("Week 5", "Day 2"), new DateTime(2024, 3, 3)) },
            { 11, new (new ("Week 6", "Day 1"), new DateTime(2024, 2, 8)) },
            { 12, new (new ("Week 6", "Day 2"), new DateTime(2024, 2, 9)) },
            { 13, new (new ("Week 6", "Day 3"), new DateTime(2024, 2, 10))},
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