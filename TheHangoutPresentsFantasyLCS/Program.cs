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

app.MapGet("/updateplayerlist", () =>
{
    try
    {
        UpdateData.UpdatePlayerList();
        return "Success!";
    }
    catch
    {
        return "Failure!";
    }
    
})
.WithName("UpdatePlayerList")
.WithOpenApi();

app.MapGet("/updatematchdata", () =>
{
    try
    {
        UpdateData.UpdateMatchData();
        return "Success!";
    }
    catch
    {
        return "Failure!";
    }
})
.WithName("UpdateMatchData")
.WithOpenApi();

app.MapGet("/getplayer/{id}", (int id) =>
{
    try
    {   
        var player = UpdateData.GetPlayer(id);
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

app.Run();


// I am removing the API, as it feels unnecessary at this point. 
// The data returned from these two methods is far too big to be returned at an endpoint,
// so we might as well just stick to file IO until necessary.

