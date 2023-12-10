using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

public class PickBans
{
    public class Match
    {
        // The week or type of match played i.e. "Week 1" or "Tiebreakers"
        [JsonPropertyName("Phase")]
        public string Phase { get; set; }

        // Name of which team played on Blue Side
        [JsonPropertyName("Blue")]
        public string Blue { get; set; }

        // Name of which team played on Red Side
        [JsonPropertyName("Red")]
        public string Red { get; set; }

        // Result of the match i.e. "0 - 1", "1 - 0", "3 - 2" etc.
        // todo: we could refactor this to split on the '-' and store the winner and loser as enum values.
        [JsonPropertyName("Score")]
        public string Score { get; set; }

        // Lists the patch that the game was played on. Technically a float, but string is fine too.
        [JsonPropertyName("Patch")]
        public string Patch { get; set; }

        [JsonPropertyName("BB1")]
        public string BB1 { get; set; }

        [JsonPropertyName("RB1")]
        public string RB1 { get; set; }

        [JsonPropertyName("BB2")]
        public string BB2 { get; set; }

        [JsonPropertyName("RB2")]
        public string RB2 { get; set; }

        [JsonPropertyName("BB3")]
        public string BB3 { get; set; }

        [JsonPropertyName("RB3")]
        public string RB3 { get; set; }

        [JsonPropertyName("BP1")]
        public string BP1 { get; set; }

        [JsonPropertyName("RP1-2")]
        public string RP1_2 { get; set; }

        [JsonPropertyName("BP2-3")]
        public string BP2_3 { get; set; }

        [JsonPropertyName("RP3")]
        public string RP3 { get; set; }

        [JsonPropertyName("RB4")]
        public string RB4 { get; set; }

        [JsonPropertyName("BB4")]
        public string BB4 { get; set; }

        [JsonPropertyName("RB5")]
        public string RB5 { get; set; }

        [JsonPropertyName("BB5")]
        public string BB5 { get; set; }

        [JsonPropertyName("RP4")]
        public string RP4 { get; set; }

        [JsonPropertyName("BP4-5")]
        public string BP4_5 { get; set; }

        [JsonPropertyName("SB")]
        public string SB { get; set; }

        [JsonPropertyName("VOD")]
        public string VOD { get; set; }
    }
}