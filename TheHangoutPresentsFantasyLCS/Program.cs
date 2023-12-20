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


app.MapGet("/getmatch", () =>
{
    return GetProData.GetMatch();
})
.WithName("GetMatch")
.WithOpenApi();

app.MapGet("/getplayer", () =>
{
    return GetProData.GetPlayer();
})
.WithName("GetPlayer")
.WithOpenApi();

app.MapGet("/getteam", () =>
{
    return GetProData.GetTeam();
})
.WithName("GetTeam")
.WithOpenApi();

app.Run();