using System.Text.Json.Serialization;

public class CreateTeamRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("logoUrl")]
    public string LogoUrl { get; set; }
}