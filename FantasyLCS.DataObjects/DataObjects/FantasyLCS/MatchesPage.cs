using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class MatchesPage
{
    [JsonPropertyName("leagueMatches")]
    public List<LeagueMatch> LeagueMatches { get; set; }

    [JsonPropertyName("leagueMatchScores")]
    public List<LeagueMatchPlayerScore> LeagueMatchPlayerScores { get; set; } = new List<LeagueMatchPlayerScore>();
}

public class LeagueMatchPlayerScore
{
    [JsonPropertyName("matchID")]
    public int LeagueMatchID { get; set; }

    [JsonPropertyName("matchDate")]
    public DateTime MatchDate { get; set; }

    [JsonPropertyName("Week")]
    public string Week { get; set; }

    [JsonPropertyName("teamOneID")]
    public int TeamOneID { get; set; }

    [JsonPropertyName("teamTwoID")]
    public int TeamTwoID { get; set; }

    [JsonPropertyName("draftPlayerID")]
    public int DraftPlayerID { get; set; }

    [JsonPropertyName("finalScore")]
    public double FinalScore { get; set; }
}
