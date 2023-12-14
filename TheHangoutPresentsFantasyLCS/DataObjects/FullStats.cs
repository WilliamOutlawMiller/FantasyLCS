using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

public class FullStats
{
    [JsonPropertyName("Player")]
    public string Player { get; set; }
    [JsonPropertyName("Role")]
    public string Role { get; set; }

    [JsonPropertyName("Level")]
    public string Level { get; set; }

    [JsonPropertyName("Kills")]
    public string Kills { get; set; }

    [JsonPropertyName("Deaths")]
    public string Deaths { get; set; }

    [JsonPropertyName("Assists")]
    public string Assists { get; set; }

    [JsonPropertyName("KDA")]
    public string KDA { get; set; }

    [JsonPropertyName("CS")]
    public string CS { get; set; }

    [JsonPropertyName("CSTeamJG")]
    public string CSTeamJG { get; set; }

    [JsonPropertyName("CSEnemyJG")]
    public string CSEnemyJG { get; set; }

    [JsonPropertyName("CSM")]
    public string CSM { get; set; }

    [JsonPropertyName("Gold")]
    public string Gold { get; set; }

    [JsonPropertyName("GPM")]
    public string GPM { get; set; }

    [JsonPropertyName("GoldPercent")]
    public string GoldPercent { get; set; }

    [JsonPropertyName("VisionScore")]
    public string VisionScore { get; set; }

    [JsonPropertyName("WardsPlaced")]
    public string WardsPlaced { get; set; }

    [JsonPropertyName("WardsDestroyed")]
    public string WardsDestroyed { get; set; }

    [JsonPropertyName("ControlWardsPurchased")]
    public string ControlWardsPurchased { get; set; }

    [JsonPropertyName("ControlWardsPlaced")]
    public string ControlWardsPlaced { get; set; }

    [JsonPropertyName("VisionScorePerMinute")]
    public string VisionScorePerMinute { get; set; }

    [JsonPropertyName("WardsPerMinute")]
    public string WardsPerMinute { get; set; }

    [JsonPropertyName("ControlWardsPerMinute")]
    public string ControlWardsPerMinute { get; set; }

    [JsonPropertyName("WardsKilledPerMinute")]
    public string WardsKilledPerMinute { get; set; }

    [JsonPropertyName("VisionScorePercent")]
    public string VisionScorePercent { get; set; }

    [JsonPropertyName("TotalDamageToChampions")]
    public string TotalDamageToChampions { get; set; }

    [JsonPropertyName("PhysicalDamage")]
    public string PhysicalDamage { get; set; }

    [JsonPropertyName("MagicDamage")]
    public string MagicDamage { get; set; }

    [JsonPropertyName("TrueDamage")]
    public string TrueDamage { get; set; }

    [JsonPropertyName("DPM")]
    public string DPM { get; set; }

    [JsonPropertyName("DamagePercent")]
    public string DamagePercent { get; set; }

    [JsonPropertyName("KillsPlusAssistsPerMinute")]
    public string KillsPlusAssistsPerMinute { get; set; }

    [JsonPropertyName("KillPercent")]
    public string KillPercent { get; set; }

    [JsonPropertyName("SoloKills")]
    public string SoloKills { get; set; }

    [JsonPropertyName("DoubleKills")]
    public string DoubleKills { get; set; }

    [JsonPropertyName("TripleKills")]
    public string TripleKills { get; set; }

    [JsonPropertyName("QuadraKills")]
    public string QuadraKills { get; set; }

    [JsonPropertyName("PentaKills")]
    public string PentaKills { get; set; }

    [JsonPropertyName("GD15")]
    public string GD15 { get; set; }

    [JsonPropertyName("CSD15")]
    public string CSD15 { get; set; }

    [JsonPropertyName("XPD15")]
    public string XPD15 { get; set; }

    [JsonPropertyName("LVLD15")]
    public string LVLD15 { get; set; }

    [JsonPropertyName("ObjectivesStolen")]
    public string ObjectivesStolen { get; set; }

    [JsonPropertyName("DmgToTurrets")]
    public string DmgToTurrets { get; set; }

    [JsonPropertyName("TotalHeal")]
    public string TotalHeal { get; set; }

    [JsonPropertyName("TotalHealOnTeammates")]
    public string TotalHealOnTeammates { get; set; }

    [JsonPropertyName("DamageSelfMitigated")]
    public string DamageSelfMitigated { get; set; }

    [JsonPropertyName("TotalDamageShieldedOnTeammates")]
    public string TotalDamageShieldedOnTeammates { get; set; }

    // I think this means how many occurrences of CC, not actual time.
    [JsonPropertyName("TimeCCingOthers")]
    public string TimeCCingOthers { get; set; }

    // This is how much time CCed was inflicted
    [JsonPropertyName("TotalTimeCCDealt")]
    public string TotalTimeCCDealt { get; set; }

    [JsonPropertyName("TotalDamageTaken")]
    public string TotalDamageTaken { get; set; }

    [JsonPropertyName("TotalTimeSpentDead")]
    public string TotalTimeSpentDead { get; set; }

    [JsonPropertyName("ConsumablesPurchased")]
    public string ConsumablesPurchased { get; set; }

    [JsonPropertyName("ItemsPurchased")]
    public string ItemsPurchased { get; set; }

    [JsonPropertyName("ShutdownBountyCollected")]
    public string ShutdownBountyCollected { get; set; }

    [JsonPropertyName("ShutdownBountyLost")]
    public string ShutdownBountyLost { get; set; }
}