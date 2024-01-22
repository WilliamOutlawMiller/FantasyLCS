using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class DraftPlayer
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("drafted")]
    public bool Drafted { get; set; }

    [JsonPropertyName("position")]
    public Position Position { get; set; }

    [JsonPropertyName("draftID")]
    public int DraftID { get; set; }

    [JsonPropertyName("teamID")]
    public int? TeamID { get; set; }

    [JsonPropertyName("imagePath")]
    public string ImagePath
    {
        get
        {
            return "/headshots/" + Name + ".webp";
        }
    }
}

public enum Position
{
    Top,
    Jungle,
    Mid,
    Bot,
    Support
}