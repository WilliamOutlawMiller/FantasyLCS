using FantasyLCS.DataObjects;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapPost("/updateplayerlist", async (HttpContext context) =>
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

app.MapPost("/updatematchdata", async (HttpContext context) =>
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

app.MapPost("/createteam", async (HttpContext context) =>
{
    try
    {
        using var reader = new StreamReader(context.Request.Body);
        var requestBody = await reader.ReadToEndAsync();

        // Deserialize the JSON data to get the name and username
        var requestData = JsonSerializer.Deserialize<CreateTeamRequest>(requestBody);

        if (requestData != null)
        {
            DataManager.CreateTeam(requestData.Name, requestData.Username);
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

app.MapPost("/addplayertoteam", async (HttpContext context) =>
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

app.MapPost("/removeplayerfromteam", async (HttpContext context) =>
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

app.MapGet("/getallplayers", () =>
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

app.MapGet("/getavailableplayers", () =>
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

app.MapGet("/getplayer/{id}", async (int id) =>
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

app.MapGet("/getmatch/{id}", async (int id) =>
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

app.MapGet("/getteam/{id}", async (int id) =>
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

app.MapGet("/getteambyusername/{username}", async (string username) =>
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

app.MapGet("/getteamid/{name}", async (string name) =>
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

app.MapGet("/getallteams", () =>
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

app.Run();