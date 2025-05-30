﻿using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic; // Needed for List
using System; // Needed for Random
using Constants;
using Microsoft.EntityFrameworkCore;
using FantasyLCS.DataObjects;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using FantasyLCS.API;
using System.Text.RegularExpressions;

public class DraftHub : Hub
{
    private readonly AppDbContext _context;

    public DraftHub(AppDbContext context)
    {
        _context = context;
    }

    public async Task StartDraft(int leagueId)
    {
        var league = _context.Leagues.Find(leagueId);

        List<User> users = _context.Users.Where(user => league.UserIDs.Contains(user.ID)).ToList();
        List<Team> teams = _context.Teams.ToList();

        List<int?> nullableTeamIDs = users.Select(user => user.TeamID).ToList();
        List<int> teamIDs = nullableTeamIDs.Where(id => id.HasValue).Select(id => id.Value).ToList();

        List<Team> leagueTeams = teams.Where(team => teamIDs.Contains(team.ID)).ToList();

        if (league != null && league.LeagueStatus == LeagueStatus.NotStarted)
        {
            league.LeagueStatus = LeagueStatus.DraftInProgress;
            _context.Update(league);
            List<DraftPlayer> draftPlayers = new List<DraftPlayer>();

            var draft = new Draft
            {
                LeagueID = leagueId,
                DraftOrder = InitializeDraftOrder(teamIDs),
                CurrentRound = 1,
                CurrentPickIndex = 0
            };

            foreach (var draftPlayer in DraftPlayerConstants.DraftPlayers)
            {
                draftPlayer.DraftID = draft.ID;
                draftPlayers.Add(draftPlayer);
            }

            draft.DraftPlayers = draftPlayers;

            _context.Drafts.Add(draft);
            await _context.SaveChangesAsync();

            await Clients.Group(leagueId.ToString()).SendAsync("DraftStarted");
        }
    }

    private List<int> InitializeDraftOrder(List<int> teamIDs)
    {
        var random = new Random();
        var draftOrder = teamIDs.OrderBy(x => random.Next()).ToList();
        return draftOrder;
    }

    public async Task PlayerDrafted(int leagueId, int playerId)
    {
        var draft = _context.Drafts
            .Include(d => d.DraftPlayers)
            .FirstOrDefault(d => d.LeagueID == leagueId);

        if (draft != null)
        {
            var draftPlayer = draft.DraftPlayers.FirstOrDefault(dp => dp.ID == playerId);
            if (draftPlayer != null && !draftPlayer.Drafted)
            {
                var currentTeam = _context.Teams.Find(draft.DraftOrder[draft.CurrentPickIndex]);

                draftPlayer.Drafted = true;
                draftPlayer.TeamID = currentTeam.ID;

                // Update current pick and round
                UpdateDraftTurn(draft);

                await _context.SaveChangesAsync();

                var nextTeam = _context.Teams.Find(draft.DraftOrder[draft.CurrentPickIndex]);
                var nextTeamDraftedPlayers = draft.DraftPlayers
                    .Where(dp => dp.Drafted && dp.TeamID == nextTeam.ID)
                    .Select(dp => new { dp.ID, dp.Name, dp.Position, dp.ImagePath })
                    .ToList();

                var draftedPlayerTeamLogos = draft.DraftPlayers
                    .Where(dp => dp.Drafted)
                    .Select(dp => new {
                        dp.ID,
                        dp.Position,
                        TeamLogoUrl = _context.Teams
                            .Where(t => t.ID == dp.TeamID)
                            .Select(t => t.LogoUrl)
                            .FirstOrDefault(),
                    })
                    .ToList();

                var playerDraftedUpdate = new
                {
                    DraftPlayerID = draftPlayer.ID,
                    DraftedPlayerTeamLogos = draftedPlayerTeamLogos,
                    TeamLogoUrl = currentTeam.LogoUrl, // Logo of the team that just picked
                    NextTeam = new
                    {
                        ID = nextTeam.ID,
                        Name = nextTeam.Name,
                        LogoUrl = nextTeam.LogoUrl,
                        OwnerName = nextTeam.OwnerName.ToLower(),
                        DraftedPlayerTeamLogos = nextTeamDraftedPlayers // All drafted players' logos for the next team
                    }
                };

                await Clients.Group(leagueId.ToString()).SendAsync("PlayerDrafted", playerDraftedUpdate);

                if (draft.DraftPlayers.All(dp => dp.TeamID != null))
                    EndDraft(leagueId);
            }
        }
    }

    private void UpdateDraftTurn(Draft draft)
    {
        draft.CurrentPickIndex++;

        // Check if we need to start a new round
        if (draft.CurrentPickIndex >= draft.DraftOrder.Count)
        {
            draft.CurrentRound++;
            draft.CurrentPickIndex = 0;

            draft.DraftOrder.Reverse();
        }
    }

    public async Task GetCurrentDraftState(int leagueId)
    {
        var league = _context.Leagues.Find(leagueId);
        if (league != null)
        {
            var draft = _context.Drafts
                .Include(d => d.DraftPlayers)
                .FirstOrDefault(d => d.LeagueID == leagueId);

            if (draft != null)
            {
                var currentTeamId = draft.DraftOrder[draft.CurrentPickIndex];
                var currentTeam = _context.Teams.Find(currentTeamId);
                var currentTeamDraftedPlayers = draft.DraftPlayers
                    .Where(dp => dp.Drafted && dp.TeamID == currentTeamId)
                    .Select(dp => new { dp.ID, dp.Name, dp.Position, dp.ImagePath })
                    .ToList();

                var draftedPlayerTeamLogos = draft.DraftPlayers
                    .Where(dp => dp.Drafted)
                    .Select(dp => new {
                        dp.ID,
                        dp.Name,
                        dp.Position,
                        TeamLogoUrl = _context.Teams
                            .Where(t => t.ID == dp.TeamID)
                            .Select(t => t.LogoUrl)
                            .FirstOrDefault()
                    })
                    .ToList();

                var draftStatus = new
                {
                    LeagueStatus = league.LeagueStatus,
                    DraftedPlayerTeamLogos = draftedPlayerTeamLogos,
                    CurrentTeam = new
                    {
                        ID = currentTeam.ID,
                        Name = currentTeam.Name,
                        LogoUrl = currentTeam.LogoUrl,
                        OwnerName = currentTeam.OwnerName.ToLower(),
                        DraftedPlayerTeamLogos = currentTeamDraftedPlayers // Roster for the current team
                    }
                };

                await Clients.Caller.SendAsync("UpdateDraftState", draftStatus);
            }
            else
            {
                await Clients.Caller.SendAsync("UpdateDraftState", new { LeagueStatus = league.LeagueStatus });
            }
        }
    }

    public async Task EndDraft(int leagueId)
    {
        var league = _context.Leagues.Find(leagueId);
        var draft = _context.Drafts.Include(draft => draft.DraftPlayers).FirstOrDefault(draft => draft.LeagueID == leagueId);
        var users = _context.Users.Where(user => league.UserIDs.Contains(user.ID)).ToList();
        var teamIDs = users.Select(user => user.TeamID).ToList();
        var teams = _context.Teams.Where(team => teamIDs.Contains(team.ID)).ToList();
        var random = new Random();
        if (league != null && league.LeagueStatus == LeagueStatus.DraftInProgress)
        {
            league.LeagueStatus = LeagueStatus.SeasonInProgress;
            _context.Update(league);

            foreach (var draftPlayer in draft.DraftPlayers)
            {
                var team = teams.FirstOrDefault(team => team.ID == draftPlayer.TeamID);
                team.DraftPlayerIDs.Add(draftPlayer.ID);
            }

            var leagueMatches = MatchScheduler.GenerateLeagueMatches(league, teams, _context);

            foreach (var leagueMatch in leagueMatches)
            {
                _context.LeagueMatches.Add(leagueMatch);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            await Clients.Group(leagueId.ToString()).SendAsync("DraftEnded");
        }
    }

    public async Task JoinGroup(int leagueID)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, leagueID.ToString());
    }
}