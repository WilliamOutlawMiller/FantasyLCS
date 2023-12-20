using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;

public class GetProData
{
    
    public static List<int> GetMatchIDs()
    {
        List<int> matchIDs = new List<int>();

        string url = "https://gol.gg/tournament/tournament-matchlist/LCS%20Summer%202023/";

        GolGGController controller = new GolGGController(url);

        return controller.GetMatchIDs();
    }

    public static List<FullStats> GetMatchFullStats()
    {
        // todo: right now this is reading the fullstats page of one game. This will need to be refactored for BO5 that reads from multiple matches
        List<FullStats> matchFullStats = new List<FullStats>();
        
        // string url = SeasonInfo.DOMAIN + SeasonInfo.SEASON + SeasonInfo.YEAR; 
        string url = "https://gol.gg/game/stats/53263/page-fullstats/";

        GolGGController controller = new GolGGController(url);
        
        matchFullStats = controller.GetMatchFullStats();
        return matchFullStats;
    }

    /// <summary>
    /// This method gets an LCS team stats. Probably useless, but we do need it to get an official list of players and subs, so there's that.
    /// </summary>
    /// <returns></returns>
    public static Team GetTeam()
    {
        Team team = new Team();
        string url = "https://gol.gg/teams/team-stats/1799/split-ALL/tournament-ALL/";

        GolGGController controller = new GolGGController(url);
        team = controller.GetTeam();
        return team;
    }

    public static Player GetPlayer()
    { 
        Player player = new Player();
        string url = "https://gol.gg/players/player-stats/107/season-S13/split-Summer/tournament-ALL/champion-ALL/";

        GolGGController controller = new GolGGController(url);
        player = controller.GetPlayer();
        return player;
    }
}