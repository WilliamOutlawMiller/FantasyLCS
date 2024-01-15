using System.Text.Json.Serialization;

public class AddPlayerToTeamRequest
{
    [JsonPropertyName("teamID")]
    public int TeamID { get; set; }

    [JsonPropertyName("playerID")]
    public int PlayerID { get; set; }
}
