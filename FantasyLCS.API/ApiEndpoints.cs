using FantasyLCS.DataObjects;
using FantasyLCS.DataObjects.DataObjects.RequestData;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Serilog;
using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml;
using System.Collections.Generic;


namespace FantasyLCS.API;

public static class ApiEndpoints
{
    private static object _updateDataLock = new object();

    public static async Task<IResult> Login(HttpContext context, AppDbContext dbContext)
    {
        try
        {
            using var reader = new StreamReader(context.Request.Body);
            var requestBody = await reader.ReadToEndAsync();

            // Deserialize the JSON data to get username and password
            var loginData = JsonSerializer.Deserialize<LoginRequest>(requestBody);

            if (loginData != null)
            {
                bool isAuthenticated = LoginManager.ValidateLogin(loginData.Username, loginData.Password, dbContext);

                if (isAuthenticated)
                {
                    return Results.Ok("Login successful!");
                }
                else
                {
                    return Results.Unauthorized();
                }
            }
            else
            {
                return Results.Problem("Invalid JSON data.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("Failure: " + ex.Message);
        }
    }

    public static async Task<IResult> Signup(HttpContext context, AppDbContext dbContext)
    {
        try
        {
            using var reader = new StreamReader(context.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var signupData = JsonSerializer.Deserialize<SignupRequest>(requestBody);

            if (signupData != null)
            {
                string result = LoginManager.RegisterUser(signupData, dbContext);

                if (result == "Signup successful!")
                {
                    return Results.Ok(result);
                }
                else
                {
                    return Results.Problem(result);
                }
            }
            else
            {
                return Results.Problem("Invalid JSON data.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("Failure: " + ex.Message);
        }
    }

    public static async Task<IResult> GetHomePage(string username, AppDbContext dbContext)
    {
        try        
        {
            List<Team> leagueTeams = new List<Team>();
            List<Team> teams = dbContext.Teams.ToList();
            List<Player> userTeamPlayers = new List<Player>();
            List<Player> allPlayers = new List<Player>();
            List<DraftPlayer> leagueDraftPlayers = new List<DraftPlayer>();

            User user = dbContext.Users.FirstOrDefault(user => user.Username.ToLower().Equals(username.ToLower()));
            Team userTeam = teams.FirstOrDefault(team => team.ID == user.TeamID);

            if (user == null)
                return Results.Problem("User not found. Try deleting cookies?");

            League userLeague = dbContext.Leagues.SingleOrDefault(league => league.ID == user.LeagueID);

            if (userLeague != null)
            {
                var leagueUserIDs = userLeague.UserIDs;
                var leagueUsers = dbContext.Users.Where(user => leagueUserIDs.Contains(user.ID)).ToList();

                var leagueTeamIDs = leagueUsers.Select(user => user.TeamID).ToList();

                leagueTeams = teams.Where(team => leagueTeamIDs.Contains(team.ID)).ToList();

                if (userLeague.LeagueStatus == LeagueStatus.SeasonInProgress)
                {
                    List<DraftPlayer> teamDraftPlayers = dbContext.DraftPlayers.Where(draftPlayer => userTeam.DraftPlayerIDs.Contains(draftPlayer.ID)).ToList();

                    foreach (var teamDraftPlayer in teamDraftPlayers)
                    {
                        userTeamPlayers.Add(dbContext.Players
                            .Include(player => player.GeneralStats)
                            .Include(player => player.AggressionStats)
                            .Include(player => player.VisionStats)
                            .Include(player => player.EarlyGameStats)
                            .Include(player => player.ChampionStats)
                            .FirstOrDefault(player => player.Name.ToLower().Equals(teamDraftPlayer.Name.ToLower())));
                    }

                    allPlayers = dbContext.Players.ToList();
                    leagueDraftPlayers = dbContext.DraftPlayers.Where(dp => leagueTeamIDs.Contains(dp.TeamID)).ToList();
                }
            }

            // Create an instance of HomePageData and populate its properties
            var homePage = new HomePage
            {
                UserTeam = userTeam,
                UserLeague = userLeague,
                LeagueTeams = leagueTeams,
                UserTeamPlayers = userTeamPlayers,
                AllPlayers = allPlayers,
                LeagueDraftPlayers = leagueDraftPlayers
            };

            // Serialize the HomePageData object to JSON and return it
            return Results.Ok(homePage);
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Failure: " + ex);
            return Results.Problem("Failure: " + ex.Message);
        }
    }

    public static async Task<IResult> UpdatePlayerList(HttpContext context, AppDbContext dbContext)
    {
        try
        {
            // Implement logic to update player list
            DataManager.UpdatePlayerList(dbContext);
            return Results.Ok("Success!");
        }
        catch (Exception ex)
        {
            return Results.Problem("Failure: " + ex.Message);
        }
    }

    public static async Task<IResult> UpdateMatchData(HttpContext context, AppDbContext dbContext)
    {
        try
        {
            // Implement logic to update match data
            DataManager.UpdateMatchData(dbContext);
            return Results.Ok("Success!");
        }
        catch (Exception ex)
        {
            return Results.Problem("Failure: " + ex.Message);
        }
    }

    public static async Task<IResult> CreateTeam(HttpContext context, AppDbContext dbContext)
    {
        try
        {
            using var reader = new StreamReader(context.Request.Body);
            var requestBody = await reader.ReadToEndAsync();

            // Deserialize the JSON data to get the name and username
            var requestData = JsonSerializer.Deserialize<CreateTeamRequest>(requestBody);

            if (requestData != null)
            {
                Team team = DataManager.CreateTeam(requestData.Name, requestData.LogoUrl, requestData.Username, dbContext);
                return Results.Ok(team);
            }
            else
            {
                return Results.Problem("Invalid JSON data.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("Failure: " + ex.Message);
        }
    }

    public static async Task<IResult> DeleteTeam(HttpContext context, AppDbContext dbContext)
    {
        try
        {
            using var reader = new StreamReader(context.Request.Body);
            var requestBody = await reader.ReadToEndAsync();

            // Deserialize the JSON data to get the name and username
            var requestData = JsonSerializer.Deserialize<DeleteTeamRequest>(requestBody);

            if (requestData != null)
            {
                DataManager.DeleteTeam(requestData.Name, requestData.OwnerName, dbContext);
                return Results.Ok("Success!");
            }
            else
            {
                return Results.Problem("Invalid JSON data.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("Failure: " + ex.Message);
        }
    }

    public static async Task<IResult> AddPlayerToTeam(HttpContext context, AppDbContext dbContext)
    {
        try
        {
            using var reader = new StreamReader(context.Request.Body);
            var requestBody = await reader.ReadToEndAsync();

            // Deserialize the JSON data to get teamID and playerID
            var requestData = JsonSerializer.Deserialize<AddPlayerToTeamRequest>(requestBody);

            if (requestData != null)
            {
                DataManager.AddPlayerToTeam(requestData.TeamID, requestData.PlayerID, dbContext);
                return Results.Ok("Success!");
            }
            else
            {
                return Results.Problem("Invalid JSON data.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("Failure: " + ex.Message);
        }
    }

    public static async Task<IResult> RemovePlayerFromTeam(HttpContext context, AppDbContext dbContext)
    {
        try
        {
            using var reader = new StreamReader(context.Request.Body);
            var requestBody = await reader.ReadToEndAsync();

            // Deserialize the JSON data to get teamID and playerID
            var requestData = JsonSerializer.Deserialize<RemovePlayerFromTeamRequest>(requestBody);

            if (requestData != null)
            {
                DataManager.RemovePlayerFromTeam(requestData.TeamID, requestData.PlayerID, dbContext);
                return Results.Ok("Success!");
            }
            else
            {
                return Results.Problem("Invalid JSON data.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("Failure: " + ex.Message);
        }
    }

    public static async Task<IResult> CreateLeague(HttpContext context, AppDbContext dbContext)
    {
        try
        {
            using var reader = new StreamReader(context.Request.Body);
            var requestBody = await reader.ReadToEndAsync();

            // Deserialize the JSON data to get the name and username
            var requestData = JsonSerializer.Deserialize<CreateLeagueRequest>(requestBody);

            if (requestData != null)
            {
                League league = DataManager.CreateLeague(requestData.Name, requestData.LeagueOwner, dbContext);
                return Results.Ok(league);
            }
            else
            {
                return Results.Problem("Invalid JSON data.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("Failure: " + ex.Message);
        }
    }

    public static async Task<IResult> JoinLeague(HttpContext context, AppDbContext dbContext)
    {
        try
        {
            using var reader = new StreamReader(context.Request.Body);
            var requestBody = await reader.ReadToEndAsync();

            // Deserialize the JSON data to get the name and username
            var requestData = JsonSerializer.Deserialize<JoinLeagueRequest>(requestBody);

            if (requestData != null)
            {
                League league = DataManager.JoinLeague(requestData.Username, requestData.JoinCode, dbContext);
                return Results.Ok(league);
            }
            else
            {
                return Results.Problem("Invalid JSON data.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("Failure: " + ex.Message);
        }
    }

    public static async Task<IResult> RemoveUserFromLeague(string username, AppDbContext dbContext)
    {
        try
        {
            if (username != null)
            {
                DataManager.RemoveUserFromLeague(username, dbContext);
                return Results.Ok("Success!");
            }
            else
            {
                return Results.Problem("No username sent.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("Failure: " + ex.Message);
        }
    }

    public static async Task<IResult> GetDraftPlayers(int leagueID, AppDbContext dbContext)
    {
        try
        {
            var league = dbContext.Leagues.Find(leagueID);

            if (league != null)
            {
                var draft = dbContext.Drafts.Include(draft => draft.DraftPlayers).FirstOrDefault(draft => draft.LeagueID == leagueID);

                if (draft == null)
                {
                    return Results.Problem("No draft found for this league.");
                }

                return Results.Ok(draft.DraftPlayers);
            }
            else
            {
                return Results.NotFound("League not found.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("An error occurred: " + ex.Message);
        }
    }

    public static async Task<IResult> GetAllPlayers(AppDbContext dbContext)
    {
        try
        {
            var players = dbContext.Players.ToList();

            if (players != null)
            {
                return Results.Ok(players);
            }
            else
            {
                return Results.NotFound("Player list not found.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("An error occurred: " + ex.Message);
        }
    }

    public static async Task<IResult> GetAvailablePlayers(AppDbContext dbContext)
    {
        try
        {
            var players = dbContext.Players.Where(player => player.TeamID == 0).ToList();

            if (players != null)
            {
                return Results.Ok(players);
            }
            else
            {
                return Results.NotFound("Players not found.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("An error occurred: " + ex.Message);
        }
    }

    public static async Task<IResult> GetPlayer(int id, AppDbContext dbContext)
    {
        try
        {
            var player = dbContext.Players.FirstOrDefault(player => player.ID == id);
            if (player != null)
            {
                return Results.Ok(player);
            }
            else
            {
                return Results.NotFound("Player not found.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("An error occurred: " + ex.Message);
        }
    }

    public static async Task<IResult> GetMatch(int id, AppDbContext dbContext)
    {
        try
        {
            var match = dbContext.Matches.FirstOrDefault(match => match.ID == id);
            if (match != null)
            {
                return Results.Ok(match);
            }
            else
            {
                return Results.NotFound("Match not found.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("An error occurred: " + ex.Message);
        }
    }

    public static async Task<IResult> GetTeam(int id, AppDbContext dbContext)
    {
        try
        {
            var team = dbContext.Teams.FirstOrDefault(team => team.ID == id);
            if (team != null)
            {
                return Results.Ok(team);
            }
            else
            {
                return Results.NotFound("Team not found.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("An error occurred: " + ex.Message);
        }
    }

    public static async Task<IResult> GetTeamByUsername(string username, AppDbContext dbContext)
    {
        try
        {
            var team = dbContext.Teams.FirstOrDefault(team => team.OwnerName.Equals(username));

            if (team != null)
            {
                return Results.Ok(team);
            }
            else
            {
                return Results.NotFound("No team found for the username.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("An error occurred: " + ex.Message);
        }
    }

    public static async Task<IResult> GetLeagueByUsername(string username, AppDbContext dbContext)
    {
        try
        {
            User user = dbContext.Users.SingleOrDefault(user => user.Username.ToLower().Equals(username.ToLower()));
            League league = dbContext.Leagues.SingleOrDefault(league => league.ID == user.LeagueID);

            if (league != null)
            {
                return Results.Ok(league);
            }
            else
            {
                return Results.NotFound("No league found for the username.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("An error occurred: " + ex.Message);
        }
    }

    public static async Task<IResult> GetLeagueMatches(int id, AppDbContext dbContext)
    {
        try
        {
            League league = dbContext.Leagues.SingleOrDefault(league => league.ID == id);

            if (league == null)
                return Results.Problem("Invalid League... Maybe clear your cookies?");

            List<LeagueMatch> leagueMatches = dbContext.LeagueMatches
                .Include(lm => lm.TeamOne)
                .Include(lm => lm.TeamTwo)
                .Where(leagueMatch => leagueMatch.LeagueID == league.ID).ToList();

            if (leagueMatches == null || leagueMatches.Count == 0)
                return Results.Problem("League has no matches created.");

            return Results.Ok(leagueMatches);

        }
        catch (Exception ex)
        {
            return Results.Problem("An error occurred: " + ex.Message);
        }
    }

    /// <summary>
    /// It's important to remember that DraftPlayers are different than Players in the following ways:
    ///     Player:
    ///         exactly 8 Players per LCS team
    ///         each plays 1 match each week
    ///         uses a unique ID set from where the data is pulled in from(API, Gol.GG, etc.)
    ///      DraftPlayer:
    ///         each league has a copy of each player as a DraftPlayer object
    ///         each DraftPlayer is associated with an artificial team
    ///         uses a unique ID created in fantasy-lcs domain
    /// And finally, the stats object.
    ///      FullStats: 
    ///         ten FullStats per match, associated with real world players in a real world match(i.e.C9 vs FLY)
    ///         stores PlayerID from the Player object, NOT the DraftPlayer object
    ///         contains stats that we use to apply scores, which need to be relayed to the business data object


    /// Maintaining our own data object with unique IDs is better for keeping our business data separate from external data objects.
    /// We cannot rely on external data to be available before the season starts, but we can update ours with their data when it becomes available via the scraping interface.

    /// The merging of these two data objects occurs here.This step is necessary, as the match stats are associated with the player IDs, but teams are associated with DraftPlayer IDs.
    /// Ideally we could fix this by just storing the other IDs on each other.
    /// Regardless, here is my current implementation.
    /// </summary>
    public static async Task<IResult> GetLeagueMatchScores(int id, AppDbContext dbContext)
    {
        try
        {
            List<Score> scores = DataManager.GetLeagueMatchScores(id, dbContext);

            if (scores.Count == 0) 
                return Results.Problem("No scores found for that League Match.");

            return Results.Ok(scores);
        }
        catch (Exception ex)
        {
            return Results.Problem("An error occurred: " + ex.Message);
        }
    }

    public static async Task<IResult> GetTeamsByLeagueID(int id, AppDbContext dbContext)
    {
        try
        {
            League league = dbContext.Leagues.SingleOrDefault(league => league.ID == id);

            if (league == null)
                return Results.Problem("Invalid League... Maybe clear your cookies?");

            List<User> users = dbContext.Users.Where(user => league.UserIDs.Contains(user.ID)).ToList();

            if (users == null)
                return Results.Problem("League has no players... Maybe clear your cookies?");

            List<int?> teamIDs = users.Select(user => user.TeamID).ToList();

            if (teamIDs == null || teamIDs.Count == 0)
                return Results.Problem("No teams associated with league players... Maybe clear your cookies?");

            List<Team> teams = dbContext.Teams.Where(team => teamIDs.Contains(team.ID)).ToList();

            if (teams != null && teams.Count > 0)
            {
                teams.OrderByDescending(team => team.Wins);
                return Results.Ok(teams);
            }
            else
            {
                return Results.Problem("No teams associated with league players... Maybe clear your cookies?");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("An error occurred: " + ex.Message);
        }
    }

    public static async Task<IResult> GetTeamID(string name, AppDbContext dbContext)
    {
        try
        {
            var team = dbContext.Teams.FirstOrDefault(team => team.Name.Equals(name));

            if (team.ID != 0)
            {
                return Results.Ok(team.ID);
            }
            else
            {
                return Results.NotFound("Team not found.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("An error occurred: " + ex.Message);
        }
    }

    public static async Task<IResult> GetAllTeams(AppDbContext dbContext)
    {
        try
        {
            var teams = dbContext.Teams.ToList();
            if (teams.Count > 0)
            {
                return Results.Ok(teams);
            }
            else
            {
                return Results.NotFound("No teams were returned.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem("An error occurred: " + ex.Message);
        }
    }
}
