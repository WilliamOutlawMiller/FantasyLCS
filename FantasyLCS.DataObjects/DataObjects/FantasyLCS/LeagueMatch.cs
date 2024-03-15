using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FantasyLCS.DataObjects;

public class LeagueMatch
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("leagueID")]
    public int LeagueID { get; set; }

    [JsonIgnore]
    public League League { get; set; }

    [JsonPropertyName("week")]
    public string Week { get; set; }

    [JsonPropertyName("matchDate")]
    public DateTime MatchDate { get; set; }

    [JsonPropertyName("teamOneID")]
    public int TeamOneID { get; set; }

    [JsonIgnore]
    public Team TeamOne { get; set; }

    [JsonPropertyName("teamTwoID")]
    public int TeamTwoID { get; set; }

    [JsonIgnore]
    public Team TeamTwo { get; set; }

    [JsonPropertyName("winner")]
    public Winner Winner { get; set; }
}

public enum Winner
{
    NotPlayed,
    TeamOne,
    TeamTwo
}
