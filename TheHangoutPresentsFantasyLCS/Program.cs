using static StorageManager;

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

app.MapGet("/getplayerlist", () =>
{
    return GetProData.GetPlayerList();
})
.WithName("GetPlayerList")
.WithOpenApi();
    {
        updatedPlayerData = GetProData.GetPlayerList();
        WriteData(updatedPlayerData);
        return updatedPlayerData;
    }
    else 
        return existingPlayerData;
})

app.MapGet("/getmatchids", () =>
{
    return GetProData.GetMatchIDs();
})
.WithName("GetMatchIDs")
.WithOpenApi();

app.Run();