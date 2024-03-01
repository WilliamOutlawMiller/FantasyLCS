using FantasyLCS.DataObjects;
using System.Text.Json.Serialization;

public class League
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    [JsonPropertyName("joinCode")]
    public string JoinCode { get; set; }

    [JsonPropertyName("userIDs")]
    public List<int> UserIDs { get; set; }

    [JsonPropertyName("leagueStatus")]
    public LeagueStatus LeagueStatus { get; set; }
}

public enum LeagueStatus
{
    NotStarted,
    DraftInProgress,
    SeasonInProgress,
    SeasonFinished
}
