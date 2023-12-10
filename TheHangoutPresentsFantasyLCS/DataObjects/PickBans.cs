using System.Text.RegularExpressions;

public class PickBans
{
    public class Match
    {
        // The week or type of match played i.e. "Week 1" or "Tiebreakers"
        public string Phase { get; set; }

        // Name of which team played on Blue Side
        public string Blue { get; set; }

        // Name of which team played on Red Side
        public string Red { get; set; }

        // Result of the match i.e. "0 - 1", "1 - 0", "3 - 2" etc.
        // todo: we could refactor this to split on the '-' and store the winner and loser as enum values.
        public string Score { get; set; }

        // Lists the patch that the game was played on. Technically a float, but string is fine too.
        public string Patch { get; set; }

        public string BB1 { get; set; }

        public string RB1 { get; set; }

        public string BB2 { get; set; }

        public string RB2 { get; set; }

        public string BB3 { get; set; }

        public string RB3 { get; set; }

        public string BP1 { get; set; }

        public string RP1_2 { get; set; }

        public string BP2_3 { get; set; }

        public string RP3 { get; set; }

        public string RB4 { get; set; }

        public string BB4 { get; set; }

        public string RB5 { get; set; }

        public string BB5 { get; set; }

        public string RP4 { get; set; }

        public string BP4_5 { get; set; }

        public string SB { get; set; }

        public string VOD { get; set; }
    }
}