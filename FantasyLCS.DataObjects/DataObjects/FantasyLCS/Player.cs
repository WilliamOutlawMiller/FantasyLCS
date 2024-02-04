using FantasyLCS.DataObjects.PlayerStats;
using System.Text.Json.Serialization;

namespace FantasyLCS.DataObjects;

public class Player
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("teamID")]
    public int TeamID { get; set; }

    [JsonPropertyName("generalStats")]
    public GeneralStats GeneralStats { get; set; }

    [JsonPropertyName("aggressionStats")]
    public AggressionStats AggressionStats { get; set; }

    [JsonPropertyName("earlyGameStats")]
    public EarlyGameStats EarlyGameStats { get; set; }

    [JsonPropertyName("visionStats")]
    public VisionStats VisionStats { get; set; }

    [JsonPropertyName("championStats")]
    public List<ChampionStats> ChampionStats { get; set; }

    [JsonPropertyName("imagePath")]
    public string ImagePath
    {
        get
        {
            return "/headshots/" + Name + ".webp";
        }
    }

}