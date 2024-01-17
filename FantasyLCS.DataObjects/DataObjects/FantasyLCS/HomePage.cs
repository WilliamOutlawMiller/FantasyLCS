using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FantasyLCS.DataObjects;

public class HomePage
{
    [JsonPropertyName("userTeam")]
    public Team UserTeam { get; set; }

    [JsonPropertyName("userLeague")]
    public League UserLeague { get; set; }

    [JsonPropertyName("leagueTeams")]
    public List<Team> LeagueTeams { get; set; }
}
