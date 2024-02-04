using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class Score
{
    [JsonPropertyName("matchID")]
    public int MatchID { get; set; }

    [JsonPropertyName("matchDate")]
    public DateTime MatchDate { get; set; }

    [JsonPropertyName("playerID")]
    public int PlayerID { get; set; }

    [JsonPropertyName("finalScore")]
    public double FinalScore { get; set; }

    [JsonPropertyName("kdaScore")]
    public double KDAScore { get; set; }

    [JsonPropertyName("csd15Score")]
    public double CSD15Score { get; set; }

    [JsonPropertyName("gd15Score")]
    public double GD15Score { get; set; }

    [JsonPropertyName("vspmScore")]
    public double VSPMScore { get; set; }

    [JsonPropertyName("dpmScore")]
    public double DPMScore { get; set; }

    [JsonPropertyName("kpScore")]
    public double KPScore { get; set; }

    [JsonPropertyName("turretDamageScore")]
    public double TurretDamageScore { get; set; }

    [JsonPropertyName("damageTakenScore")]
    public double DamageTakenScore { get; set; }

    [JsonPropertyName("teamHealingScore")]
    public double TeamHealingScore { get; set; }

    [JsonPropertyName("teamShieldingScore")]
    public double TeamShieldingScore { get; set; }

    [JsonPropertyName("soloKillScore")]
    public double SoloKillScore { get; set; }

    [JsonPropertyName("doubleKillScore")]
    public double DoubleKillScore { get; set; }

    [JsonPropertyName("tripleKillScore")]
    public double TripleKillScore { get; set; }

    [JsonPropertyName("quadraKillScore")]
    public double QuadraKillScore { get; set; }

    [JsonPropertyName("pentaKillScore")]
    public double PentaKillScore { get; set; }

    [JsonPropertyName("objectiveStealScore")]
    public double ObjectiveStealScore { get; set; }

    [JsonPropertyName("ccInstancesScore")]
    public double CCInstancesScore { get; set; }

    [JsonPropertyName("ccTimeScore")]
    public double CCTimeScore { get; set; }

    [JsonPropertyName("bountyCollectedScore")]
    public double BountyCollectedScore { get; set; }

    [JsonPropertyName("bountyLostScore")]
    public double BountyLostScore { get; set; }
}

