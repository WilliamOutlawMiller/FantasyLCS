using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

namespace FantasyLCS.DataObjects;

public class FullStats
{
    public int MatchID { get; set; }

    [JsonIgnore]
    public Match Match { get; set; }

    public int PlayerID { get; set; }

    [JsonPropertyName("Player")]
    public string Name { get; set; }

    [JsonPropertyName("Champion")]
    public string Champion { get; set; }

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

    [JsonPropertyName(@"CS in Team's Jungle")]
    public string CSTeamJG { get; set; }

    [JsonPropertyName("CS in Enemy Jungle")]
    public string CSEnemyJG { get; set; }

    [JsonPropertyName("CSM")]
    public string CSM { get; set; }

    [JsonPropertyName("Golds")]
    public string Gold { get; set; }

    [JsonPropertyName("GPM")]
    public string GPM { get; set; }

    [JsonPropertyName(@"GOLD%")]
    public string GoldPercent { get; set; }

    [JsonPropertyName("Vision Score")]
    public string VisionScore { get; set; }

    [JsonPropertyName("Wards placed")]
    public string WardsPlaced { get; set; }

    [JsonPropertyName("Wards destroyed")]
    public string WardsDestroyed { get; set; }

    [JsonPropertyName("Control Wards Purchased")]
    public string ControlWardsPurchased { get; set; }

    [JsonPropertyName("Detector Wards Placed")]
    public string ControlWardsPlaced { get; set; }

    [JsonPropertyName("VSPM")]
    public string VisionScorePerMinute { get; set; }

    [JsonPropertyName("WPM")]
    public string WardsPerMinute { get; set; }

    [JsonPropertyName("VWPM")]
    public string ControlWardsPerMinute { get; set; }

    [JsonPropertyName("WCPM")]
    public string WardsKilledPerMinute { get; set; }

    [JsonPropertyName(@"VS%")]
    public string VisionScorePercent { get; set; }

    [JsonPropertyName("Total damage to Champion")]
    public string TotalDamageToChampions { get; set; }

    [JsonPropertyName("Physical Damage")]
    public string PhysicalDamage { get; set; }

    [JsonPropertyName("Magic Damage")]
    public string MagicDamage { get; set; }

    [JsonPropertyName("True Damage")]
    public string TrueDamage { get; set; }

    [JsonPropertyName("DPM")]
    public string DPM { get; set; }

    [JsonPropertyName(@"DMG%")]
    public string DamagePercent { get; set; }

    [JsonPropertyName("K+A Per Minute")]
    public string KillsPlusAssistsPerMinute { get; set; }

    [JsonPropertyName(@"KP%")]
    public string KillPercent { get; set; }

    [JsonPropertyName("Solo kills")]
    public string SoloKills { get; set; }

    [JsonPropertyName("Double kills")]
    public string DoubleKills { get; set; }

    [JsonPropertyName("Triple kills")]
    public string TripleKills { get; set; }

    [JsonPropertyName("Quadra kills")]
    public string QuadraKills { get; set; }

    [JsonPropertyName("Penta kills")]
    public string PentaKills { get; set; }

    [JsonPropertyName("GD@15")]
    public string GD15 { get; set; }

    [JsonPropertyName("CSD@15")]
    public string CSD15 { get; set; }

    [JsonPropertyName("XPD@15")]
    public string XPD15 { get; set; }

    [JsonPropertyName("LVLD@15")]
    public string LVLD15 { get; set; }

    [JsonPropertyName("Objectives Stolen")]
    public string ObjectivesStolen { get; set; }

    [JsonPropertyName("Damage dealt to turrets")]
    public string DmgToTurrets { get; set; }

    // I think this is the same as damage to turrets most of the time, likely bad data
    [JsonPropertyName("Damage dealt to buildings")]
    public string DmgToBuildings { get; set; }

    [JsonPropertyName("Total heal")]
    public string TotalHeal { get; set; }

    [JsonPropertyName("Total Heals On Teammates")]
    public string TotalHealOnTeammates { get; set; }

    [JsonPropertyName("Damage self mitigated")]
    public string DamageSelfMitigated { get; set; }

    [JsonPropertyName("Total Damage Shielded On Teammates")]
    public string TotalDamageShieldedOnTeammates { get; set; }

    // I think this means how many occurrences of CC, not actual time.
    [JsonPropertyName("Time ccing others")]
    public string TimeCCingOthers { get; set; }

    // This is how much time CCed was inflicted
    [JsonPropertyName("Total Time CC Dealt")]
    public string TotalTimeCCDealt { get; set; }

    [JsonPropertyName("Total damage taken")]
    public string TotalDamageTaken { get; set; }

    [JsonPropertyName("Total Time Spent Dead")]
    public string TotalTimeSpentDead { get; set; }

    [JsonPropertyName("Consumables purchased")]
    public string ConsumablesPurchased { get; set; }

    [JsonPropertyName("Items Purchased")]
    public string ItemsPurchased { get; set; }

    [JsonPropertyName("Shutdown bounty collected")]
    public string ShutdownBountyCollected { get; set; }

    [JsonPropertyName("Shutdown bounty lost")]
    public string ShutdownBountyLost { get; set; }
}