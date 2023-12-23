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

app.MapPost("/updateplayerlist", () =>
{
    try
    {
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

app.MapPost("/updatematchdata", () =>
{
    try
    {
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

app.MapPost("/createteam/{name}", (string name) =>
{
    try
    {
        DataManager.CreateTeam(name);
        return Results.Ok("Success!");
    }
    catch (Exception ex)
    {
        return Results.Problem("Failure: " + ex.Message);
    }
})
.WithName("CreateTeam")
.WithOpenApi();

app.MapPost("/addplayertoteam/{teamID}/{playerID}", (int teamID, int playerID) =>
{
    try
    {
        DataManager.AddPlayerToTeam(teamID, playerID);
        return Results.Ok("Success!");
    }
    catch (Exception ex)
    {
        return Results.Problem("Failure: " + ex.Message);
    }
})
.WithName("AddPlayerToTeam")
.WithOpenApi();

app.MapPost("/removeplayerfromteam/{teamID}/{playerID}", (int teamID, int playerID) =>
{
    try
    {
        DataManager.RemovePlayerFromTeam(teamID, playerID);
        return Results.Ok("Success!");
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
.WithName("GetPlayer")
.WithOpenApi();

app.MapGet("/getplayer/{id}", (int id) =>
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

app.MapGet("/getmatch/{id}", (int id) =>
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

app.MapGet("/getteam/{id}", (int id) =>
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

app.MapGet("/getavailableplayers", () =>
{
    try
    {   
        return Results.Ok(DataManager.GetAvailablePlayers());
    }
    catch (Exception ex)
    {
        return Results.Problem("An error occurred: " + ex.Message);
    }
})
.WithName("GetAvailablePlayers")
.WithOpenApi();

app.MapGet("/getteamid/{name}", (string name) =>
{
    try
    {
        int teamID = DataManager.GetTeamID(name);
        return Results.Ok(teamID);
    }
    catch (Exception ex)
    {
        return Results.Problem("An error occurred: " + ex.Message);
    }
})
.WithName("GetTeamID")
.WithOpenApi();

app.Run();