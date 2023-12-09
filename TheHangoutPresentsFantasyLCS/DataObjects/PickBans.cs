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
    }
}