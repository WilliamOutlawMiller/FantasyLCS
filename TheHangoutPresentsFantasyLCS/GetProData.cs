using HtmlAgilityPack;
using Constants;
using System.Text.Json.Nodes;

public class GetProData
{
    public static Match GetMatch()
    {
        // todo: right now this is reading the fullstats page of one game. This will need to be refactored for BO5 that reads from multiple matches
        Match match = new Match();
        
        // string url = SeasonInfo.DOMAIN + SeasonInfo.SEASON + SeasonInfo.YEAR; 
        string url = "https://gol.gg/game/stats/53263/page-fullstats/";

        GolGGController controller = new GolGGController();
        
        match.FullStats = controller.GetMatchFullStats(url);
        return match;
    }

    public static Team GetTeam()
    {
        // todo: right now this is reading a specific team.
        
        Team team = new Team();
        string url = "https://gol.gg/teams/team-stats/1799/split-ALL/tournament-ALL/";

        GolGGController controller = new GolGGController();
        team = controller.GetTeam(url);
        return team;
    }

    public static Player GetPlayer()
    { 
        Player player = new Player();
        string url = "https://gol.gg/players/list/season-ALL/split-ALL/tournament-LCS%20Championship%202023/";

        GolGGController controller = new GolGGController();
        player = controller.GetPlayer(url);
        return player;
    }
}