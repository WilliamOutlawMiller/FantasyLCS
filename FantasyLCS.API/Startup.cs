using FantasyLCS.API;
using FantasyLCS.DataObjects;
using FantasyLCS.DataObjects.DataObjects.RequestData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("/login", async (HttpContext context) =>
            {
                try
                {
                    using var reader = new StreamReader(context.Request.Body);
                    var requestBody = await reader.ReadToEndAsync();

                    // Deserialize the JSON data to get username and password
                    var loginData = JsonSerializer.Deserialize<LoginRequest>(requestBody);

                    if (loginData != null)
                    {
                        bool isAuthenticated = LoginManager.ValidateLogin(loginData.Username, loginData.Password);

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
            })
            .WithName("Login")
            .WithOpenApi();

            endpoints.MapPost("/signup", async (HttpContext context) =>
            {
                try
                {
                    using var reader = new StreamReader(context.Request.Body);
                    var requestBody = await reader.ReadToEndAsync();
                    var signupData = JsonSerializer.Deserialize<SignupRequest>(requestBody);

                    if (signupData != null)
                    {
                        string result = LoginManager.RegisterUser(signupData);

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
            })
            .WithName("Signup")
            .WithOpenApi();

            endpoints.MapPost("/updateplayerlist", async (HttpContext context) =>
            {
                try
                {
                    // Implement logic to update player list
                    DataManager.UpdatePlayerList();
                    return Results.Ok("Success!");
                }
                catch (Exception ex)
                {
                    return Results.Problem("Failure: " + ex.Message);
                }
            })
            .WithName("UpdatePlayerList")
            .WithOpenApi();

            endpoints.MapPost("/updatematchdata", async (HttpContext context) =>
            {
                try
                {
                    // Implement logic to update match data
                    DataManager.UpdateMatchData();
                    return Results.Ok("Success!");
                }
                catch (Exception ex)
                {
                    return Results.Problem("Failure: " + ex.Message);
                }
            })
            .WithName("UpdateMatchData")
            .WithOpenApi();

            endpoints.MapPost("/createteam", async (HttpContext context) =>
            {
                try
                {
                    using var reader = new StreamReader(context.Request.Body);
                    var requestBody = await reader.ReadToEndAsync();

                    // Deserialize the JSON data to get the name and username
                    var requestData = JsonSerializer.Deserialize<CreateTeamRequest>(requestBody);

                    if (requestData != null)
                    {
                        DataManager.CreateTeam(requestData.Name, requestData.LogoUrl, requestData.Username);
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
            })
            .WithName("CreateTeam")
            .WithOpenApi();

            endpoints.MapPost("/deleteteam", async (HttpContext context) =>
            {
                try
                {
                    using var reader = new StreamReader(context.Request.Body);
                    var requestBody = await reader.ReadToEndAsync();

                    // Deserialize the JSON data to get the name and username
                    var requestData = JsonSerializer.Deserialize<DeleteTeamRequest>(requestBody);

                    if (requestData != null)
                    {
                        DataManager.DeleteTeam(requestData.Name, requestData.OwnerName);
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
            })
            .WithName("DeleteTeam")
            .WithOpenApi();

            endpoints.MapPost("/addplayertoteam", async (HttpContext context) =>
            {
                try
                {
                    using var reader = new StreamReader(context.Request.Body);
                    var requestBody = await reader.ReadToEndAsync();

                    // Deserialize the JSON data to get teamID and playerID
                    var requestData = JsonSerializer.Deserialize<AddPlayerToTeamRequest>(requestBody);

                    if (requestData != null)
                    {
                        DataManager.AddPlayerToTeam(requestData.TeamID, requestData.PlayerID);
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
            })
            .WithName("AddPlayerToTeam")
            .WithOpenApi();

            endpoints.MapPost("/removeplayerfromteam", async (HttpContext context) =>
            {
                try
                {
                    using var reader = new StreamReader(context.Request.Body);
                    var requestBody = await reader.ReadToEndAsync();

                    // Deserialize the JSON data to get teamID and playerID
                    var requestData = JsonSerializer.Deserialize<RemovePlayerFromTeamRequest>(requestBody);

                    if (requestData != null)
                    {
                        DataManager.RemovePlayerFromTeam(requestData.TeamID, requestData.PlayerID);
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
            })
            .WithName("RemovePlayerFromTeam")
            .WithOpenApi();

            endpoints.MapPost("/createleague", async (HttpContext context) =>
            {
                try
                {
                    using var reader = new StreamReader(context.Request.Body);
                    var requestBody = await reader.ReadToEndAsync();

                    // Deserialize the JSON data to get the name and username
                    var requestData = JsonSerializer.Deserialize<CreateLeagueRequest>(requestBody);

                    if (requestData != null)
                    {
                        DataManager.CreateLeague(requestData.Name, requestData.LeagueOwner);
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
            })
            .WithName("CreateLeague")
            .WithOpenApi();

            endpoints.MapPost("/joinleague", async (HttpContext context) =>
            {
                try
                {
                    using var reader = new StreamReader(context.Request.Body);
                    var requestBody = await reader.ReadToEndAsync();

                    // Deserialize the JSON data to get the name and username
                    var requestData = JsonSerializer.Deserialize<JoinLeagueRequest>(requestBody);

                    if (requestData != null)
                    {
                        DataManager.JoinLeague(requestData.Username, requestData.JoinCode);
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
            })
            .WithName("JoinLeague")
            .WithOpenApi();

            endpoints.MapPost("/removeuserfromleague/{username}", async (string username) =>
            {
                try
                {
                    if (username != null)
                    {
                        DataManager.RemoveUserFromLeague(username);
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
            })
            .WithName("RemoveUserFromLeague")
            .WithOpenApi();

            endpoints.MapGet("/getallplayers", async () =>
            {
                try
                {
                    using var dbContext = new AppDbContext();
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
            })
            .WithName("GetAllPlayers")
            .WithOpenApi();

            endpoints.MapGet("/getavailableplayers", () =>
            {
                try
                {
                    using var dbContext = new AppDbContext();
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
            })
            .WithName("GetAvailablePlayers")
            .WithOpenApi();

            endpoints.MapGet("/getplayer/{id}", async (int id) =>
            {
                try
                {
                    using var dbContext = new AppDbContext();
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
            })
            .WithName("GetPlayer")
            .WithOpenApi();

            endpoints.MapGet("/getmatch/{id}", async (int id) =>
            {
                try
                {
                    using var dbContext = new AppDbContext();
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
            })
            .WithName("GetMatch")
            .WithOpenApi();

            endpoints.MapGet("/getteam/{id}", async (int id) =>
            {
                try
                {
                    using var dbContext = new AppDbContext();
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
            })
            .WithName("GetTeam")
            .WithOpenApi();

            endpoints.MapGet("/getteambyusername/{username}", async (string username) =>
            {
                try
                {
                    using var dbContext = new AppDbContext();
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
            })
            .WithName("GetTeamByUsername")
            .WithOpenApi();

            endpoints.MapGet("/getleaguebyusername/{username}", async (string username) =>
            {
                try
                {
                    using var dbContext = new AppDbContext();

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
            })
            .WithName("GetLeagueByUsername")
            .WithOpenApi();

            endpoints.MapGet("/getteamsbyleagueid/{id}", async (int id) =>
            {
                try
                {
                    using var dbContext = new AppDbContext();

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
            })
            .WithName("GetTeamsByLeagueID")
            .WithOpenApi();

            endpoints.MapGet("/getteamid/{name}", async (string name) =>
            {
                try
                {
                    using var dbContext = new AppDbContext();
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
            })
            .WithName("GetTeamID")
            .WithOpenApi();

            endpoints.MapGet("/getallteams", () =>
            {
                try
                {
                    using var dbContext = new AppDbContext();
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
            })
            .WithName("GetAllTeams")
            .WithOpenApi();
        });
    }
}
