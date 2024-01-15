using System.Text.Json.Serialization;

public class Team
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("ownerName")]
    public string OwnerName { get; set; }

    [JsonPropertyName("logoUrl")]
    public string LogoUrl { get; set; }

    [JsonPropertyName("wins")]
    public int Wins { get; set; }

    [JsonPropertyName("losses")]
    public int Losses { get; set; }

    [JsonPropertyName("playerIDs")]
    public List<int> PlayerIDs { get; set; }
}
