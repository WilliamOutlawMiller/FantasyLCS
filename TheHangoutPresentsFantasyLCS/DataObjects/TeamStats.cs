using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

namespace TeamStats
{
    public class TeamSummary
    {
        [JsonPropertyName("Region")]
        public string Region { get; set; }

        [JsonPropertyName("Season")]
        public string Season { get; set; }

        [JsonPropertyName("Win Rate")]
        public string WinRate { get; set; }

        [JsonPropertyName("Average game duration")]
        public string AvgGameDuration { get; set; }
    }

    public class BannedBy
    {
        [JsonPropertyName("Overall")]
        public Dictionary<string, string> Overall { get; set; }

        [JsonPropertyName("Blue Side")]
        public Dictionary<string, string> BlueSide { get; set; }

        [JsonPropertyName("Red Side")]
        public Dictionary<string, string> RedSide { get; set; }
    }

    public class BannedAgainst
    {
        [JsonPropertyName("Overall")]
        public Dictionary<string, string> Overall { get; set; }

        [JsonPropertyName("Blue Side")]
        public Dictionary<string, string> BlueSide { get; set; }

        [JsonPropertyName("Red Side")]
        public Dictionary<string, string> RedSide { get; set; }
    }

    public class Economy
    {
        [JsonPropertyName("Gold Per Minute")]
        public string GPM { get; set; }

        [JsonPropertyName("Gold Differential per Minute")]
        public string GDPM { get; set; }

        [JsonPropertyName("Gold Differential at 15 min")]
        public string GD15 { get; set; }

        [JsonPropertyName("Win Rate when ahead at 15 min")]
        public string WRAhead15 { get; set; }

        [JsonPropertyName("CS Per Minute")]
        public string CSPM { get; set; }
        
        [JsonPropertyName("CS Differential at 15 min")]
        public string CSD15 { get; set; }

        [JsonPropertyName("Tower Differential at 15 min")]
        public string TD15 { get; set; }

        [JsonPropertyName("Avg. Tower Difference")]
        public string AVGTD { get; set; }

        [JsonPropertyName("First Tower")]
        public string FirstTowerPercent { get; set; }
    }

    public class Aggression
    {
        [JsonPropertyName("Damage Per Minute")]
        public string GPM { get; set; }

        [JsonPropertyName("First Blood")]
        public string FirstBlood { get; set; }

        [JsonPropertyName("Kills Per Game")]
        public string KillsPerGame { get; set; }

        [JsonPropertyName("Deaths Per Game")]
        public string DeathsPerGame { get; set; }

        [JsonPropertyName("Average Kill / Death Ratio")]
        public string AVGKD { get; set; }
        
        [JsonPropertyName("Average Assists / Kill")]
        public string AVGKA { get; set; }
    }

    public class Objectives
    {
        [JsonPropertyName("Plates / game (TOP|MID|BOT)")]
        public string AVGPlates { get; set; }

        [JsonPropertyName("Dragons / game")]
        public string AVGDragons { get; set; }

        [JsonPropertyName("Dragons at 15 min")]
        public string AVGDragons15 { get; set; }

        [JsonPropertyName("Herald / Game")]
        public string AVGHeralds { get; set; }

        [JsonPropertyName("Nashors / game")]
        public string AVGBarons { get; set; }
    }

    public class Vision
    {
        [JsonPropertyName("Vision Score Per Minute")]
        public string VSPM { get; set; }

        [JsonPropertyName("Wards Per Minute")]
        public string WPM { get; set; }

        [JsonPropertyName("Vision Wards Per Minute")]
        public string VWPM { get; set; }

        [JsonPropertyName("Wards Cleared Per Minute")]
        public string WCPM { get; set; }

        [JsonPropertyName(@"% Wards Cleared")]
        public string PercentWardsCleared { get; set; }
    }  

    public class Roster
    {
        List<Player> Players { get; set; }

        List<Player> Subs { get; set; }
    }
}

