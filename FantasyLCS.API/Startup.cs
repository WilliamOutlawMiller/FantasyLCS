using FantasyLCS.API;
using FantasyLCS.DataObjects;
using FantasyLCS.DataObjects.DataObjects.RequestData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        services.AddSignalR();
        services.AddDbContextFactory<AppDbContext>();

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.WithOrigins("http://localhost:5001") // The client URL
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials());
            });
        }
        else
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.WithOrigins("https://localhost:7184") // The client URL
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials());
            });
        }

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("CorsPolicy");

        var dbContextFactory = app.ApplicationServices.GetRequiredService<IDbContextFactory<AppDbContext>>();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {

            endpoints.MapHub<DraftHub>("/draftHub");

            endpoints.MapPost("/login", async (HttpContext context) =>
                { return await ApiEndpoints.Login(context, dbContextFactory.CreateDbContext()); })
                .WithName("Login")
                .WithOpenApi();

            endpoints.MapPost("/signup", async (HttpContext context) =>
                { return await ApiEndpoints.Signup(context, dbContextFactory.CreateDbContext()); })
                .WithName("Signup")
                .WithOpenApi();

            endpoints.MapPost("/updateplayerlist", async (HttpContext context) =>
                { return await ApiEndpoints.UpdatePlayerList(context, dbContextFactory.CreateDbContext()); })
                .WithName("UpdatePlayerList")
                .WithOpenApi();

            endpoints.MapPost("/updatematchdata", async (HttpContext context) =>
                { return await ApiEndpoints.UpdateMatchData(context, dbContextFactory.CreateDbContext()); })
                .WithName("UpdateMatchData")
                .WithOpenApi();

            endpoints.MapPost("/createteam", async (HttpContext context) =>
                { return await ApiEndpoints.CreateTeam(context, dbContextFactory.CreateDbContext()); })
                .WithName("CreateTeam")
                .WithOpenApi();

            endpoints.MapPost("/deleteteam", async (HttpContext context) =>
                { return await ApiEndpoints.DeleteTeam(context, dbContextFactory.CreateDbContext()); })
                .WithName("DeleteTeam")
                .WithOpenApi();

            endpoints.MapPost("/addplayertoteam", async (HttpContext context) =>
                { return await ApiEndpoints.AddPlayerToTeam(context, dbContextFactory.CreateDbContext()); })
                .WithName("AddPlayerToTeam")
                .WithOpenApi();

            endpoints.MapPost("/removeplayerfromteam", async (HttpContext context) =>
                { return await ApiEndpoints.RemovePlayerFromTeam(context, dbContextFactory.CreateDbContext()); })
                .WithName("RemovePlayerFromTeam")
                .WithOpenApi();

            endpoints.MapPost("/createleague", async (HttpContext context) =>
                { return await ApiEndpoints.CreateLeague(context, dbContextFactory.CreateDbContext()); })
                .WithName("CreateLeague")
                .WithOpenApi();

            endpoints.MapPost("/joinleague", async (HttpContext context) =>
                { return await ApiEndpoints.JoinLeague(context, dbContextFactory.CreateDbContext()); })
                .WithName("JoinLeague")
                .WithOpenApi();

            endpoints.MapPost("/removeuserfromleague/{username}", async (string username) =>
                { return await ApiEndpoints.RemoveUserFromLeague(username, dbContextFactory.CreateDbContext()); })
                .WithName("RemoveUserFromLeague")
                .WithOpenApi();

            endpoints.MapGet("/gethomepage/{username}", async (string username) =>
                { return await ApiEndpoints.GetHomePage(username, dbContextFactory.CreateDbContext()); })
                .WithName("GetHomePage")
                .WithOpenApi();

            endpoints.MapGet("/getallteams", async () =>
                { return await ApiEndpoints.GetAllTeams(dbContextFactory.CreateDbContext()); })
                .WithName("GetAllTeams")
                .WithOpenApi();

            endpoints.MapGet("/getallplayers", async () =>
                { return await ApiEndpoints.GetAllPlayers(dbContextFactory.CreateDbContext()); })
                .WithName("GetAllPlayers")
                .WithOpenApi();

            endpoints.MapGet("/getavailableplayers", async () =>
                { return await ApiEndpoints.GetAvailablePlayers(dbContextFactory.CreateDbContext()); })
                .WithName("GetAvailablePlayers")
                .WithOpenApi();

            endpoints.MapGet("/getplayer/{id}", async (int id) =>
                { return await ApiEndpoints.GetPlayer(id, dbContextFactory.CreateDbContext()); })
                .WithName("GetPlayer")
                .WithOpenApi();

            endpoints.MapGet("/getmatch/{id}", async (int id) =>
                { return await ApiEndpoints.GetMatch(id, dbContextFactory.CreateDbContext()); })
                .WithName("GetMatch")
                .WithOpenApi();

            endpoints.MapGet("/getleaguebyusername/{username}", async (string username) =>
                { return await ApiEndpoints.GetLeagueByUsername(username, dbContextFactory.CreateDbContext()); })
                .WithName("GetLeagueByUsername")
                .WithOpenApi();

            endpoints.MapGet("/getleaguematches/{id}", async (int id) =>
            { return await ApiEndpoints.GetLeagueMatches(id, dbContextFactory.CreateDbContext()); })
                .WithName("GetLeagueMatches")
                .WithOpenApi();

            endpoints.MapGet("/getteam/{id}", async (int id) =>
                { return await ApiEndpoints.GetTeam(id, dbContextFactory.CreateDbContext()); })
                .WithName("GetTeam")
                .WithOpenApi();

            endpoints.MapGet("/getteambyusername/{username}", async (string username) =>
                { return await ApiEndpoints.GetTeamByUsername(username, dbContextFactory.CreateDbContext()); })
                .WithName("GetTeamByUsername")
                .WithOpenApi();

            endpoints.MapGet("/getteamsbyleagueid/{id}", async (int id) =>
                { return await ApiEndpoints.GetTeamsByLeagueID(id, dbContextFactory.CreateDbContext()); })
                .WithName("GetTeamsByLeagueID")
                .WithOpenApi();

            endpoints.MapGet("/getteamid/{name}", async (string name) =>
                { return await ApiEndpoints.GetTeamID(name, dbContextFactory.CreateDbContext()); })
                .WithName("GetTeamID")
                .WithOpenApi();          
        });
    }
}
