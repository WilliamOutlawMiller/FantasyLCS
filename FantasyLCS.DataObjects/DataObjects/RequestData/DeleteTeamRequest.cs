using System.Text.Json.Serialization;

public class DeleteTeamRequest
{
    [JsonPropertyName("teamName")]
    public string Name { get; set; }

    [JsonPropertyName("ownerName")]
    public string OwnerName { get; set; }
}