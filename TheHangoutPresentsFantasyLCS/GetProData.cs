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

        GolGGController controller = new GolGGController(url);
        
        match.FullStats = controller.GetMatchFullStats();
        return match;
    }

    public static Team GetTeam()
    {
        // todo: right now this is reading a specific team.
        
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