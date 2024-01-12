using FantasyLCS.API;
using FantasyLCS.DataObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text.Json;

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

            endpoints.MapGet("/getallplayers", () =>
            {
                try
                {
                    var players = StorageManager.Get<Player>();
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
                    var players = DataManager.GetAvailablePlayers();
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
                    var player = StorageManager.Get<Player>(id);
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
                    var match = StorageManager.Get<Match>(id);
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
                    var team = StorageManager.Get<Team>(id);
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
                    // Implement logic to retrieve the team by username from your data
                    var team = DataManager.GetTeamByUsername(username);

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

            endpoints.MapGet("/getteamid/{name}", async (string name) =>
            {
                try
                {
                    int teamID = DataManager.GetTeamID(name);
                    if (teamID != 0)
                    {
                        return Results.Ok(teamID);
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
                    var teams = StorageManager.Get<Team>();
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
