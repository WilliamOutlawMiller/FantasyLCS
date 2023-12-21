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
    List<Player> players = GetProData.GetPlayerList();
    UpdateStorageFiles(players);
    return players;
})
.WithName("GetPlayerList")
.WithOpenApi();

app.MapGet("/getmatchids", () =>
{
    return GetProData.GetMatchIDs();
})
.WithName("GetMatchIDs")
.WithOpenApi();

app.Run();