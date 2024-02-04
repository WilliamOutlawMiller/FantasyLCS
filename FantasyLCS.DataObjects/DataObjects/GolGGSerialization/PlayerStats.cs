using System.Text.RegularExpressions;
using System.Text.Json.Serialization;
using FantasyLCS.DataObjects;

namespace FantasyLCS.DataObjects.PlayerStats;

public class GeneralStats
{
    public int PlayerID { get; set; }

    [JsonIgnore]
    public Player Player { get; set; }

    [JsonPropertyName("Record")]
    public string Record { get; set; }

    [JsonPropertyName("Win Rate")]
    public string WinRate { get; set; }

    [JsonPropertyName("KDA")]
    public string KDA { get; set; }

    [JsonPropertyName("CS per Minute")]
    public string CsPerMin { get; set; }

    [JsonPropertyName("Gold Per Minute")]
    public string GoldPerMin { get; set; }

    [JsonPropertyName(@"Gold%")]
    public string GoldPercent { get; set; }

    [JsonPropertyName("Kill Participation")]
    public string KillParticipation { get; set; }
}

public class ChampionStats
{
    public int ChampionID { get; set; }
    public int PlayerID { get; set; }

    [JsonIgnore]
    public Player Player { get; set; }

    [JsonPropertyName("Champion")]
    public string Champion { get; set; }

    [JsonPropertyName("Nb games")]
    public string GamesPlayed { get; set; }

    [JsonPropertyName("Win Rate")]
    public string WinRate { get; set; }

    [JsonPropertyName("KDA")]
    public string KDA { get; set; }
}

public class AggressionStats
{
    public int PlayerID { get; set; }

    [JsonIgnore]
    public Player Player { get; set; }

    [JsonPropertyName("Damage Per Minute")]
    public string DPM { get; set; }

    [JsonPropertyName(@"Damage%")]
    public string DamagePercent { get; set; }

    [JsonPropertyName("K+A Per Minute")]
    public string KAPerMinute { get; set; }

    [JsonPropertyName("Solo kills")]
    public string SoloKills { get; set; }

    [JsonPropertyName("Pentakills")]
    public string Pentakills { get; set; }
}

public class EarlyGameStats
{
    public int PlayerID { get; set; }

    [JsonIgnore]
    public Player Player { get; set; }

    [JsonPropertyName("Ahead in CS at 15 min")]
    public string AheadInCSAt15Percent { get; set; }

    [JsonPropertyName("CS Differential at 15 min")]
    public string CSD15 { get; set; }

    [JsonPropertyName("Gold Differential at 15 min")]
    public string GD15 { get; set; }

    [JsonPropertyName("XP Differential at 15 min")]
    public string XPD15 { get; set; }

    [JsonPropertyName("First Blood Participation")]
    public string FirstBloodParticipantPercent { get; set; }
        
    [JsonPropertyName("First Blood Victim")]
    public string FirstBloodVictimPercent { get; set; }
}

public class VisionStats
{
    public int PlayerID { get; set; }

    [JsonIgnore]
    public Player Player { get; set; }

    [JsonPropertyName("Vision score Per Minute")]
    public string VSPM { get; set; }

    [JsonPropertyName("Ward Per Minute")]
    public string WPM { get; set; }

    [JsonPropertyName("Vision Ward Per Minute")]
    public string VWPM { get; set; }

    [JsonPropertyName("Ward Cleared Per Minute")]
    public string WCPM { get; set; }
}