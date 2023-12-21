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
        UpdateData.UpdatePlayerList();
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
        UpdateData.UpdateMatchData();
        return Results.Ok("Success!");
    }
    catch (Exception ex)
    {
        return Results.Problem("Failure: " + ex.Message);
    }
})
.WithName("UpdateMatchData")
.WithOpenApi();

app.MapGet("/getplayer/{id}", (int id) =>
{
    try
    {   
        var player = GetData.Get<Player>(id);
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
        var match = GetData.Get<Match>(id);
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

app.MapGet("/getroster/{id}", (int id) =>
{
    try
    {   
        var roster = GetData.Get<Roster>(id);
        if (roster != null)
        {
            return Results.Ok(roster);
        }
        else
        {
            return Results.NotFound("Roster not found.");
        }
    }
    catch (Exception ex)
    {
        return Results.Problem("An error occurred: " + ex.Message);
    }
})
.WithName("GetRoster")
.WithOpenApi();

app.Run();