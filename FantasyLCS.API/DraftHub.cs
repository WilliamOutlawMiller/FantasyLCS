using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic; // Needed for List
using System; // Needed for Random
using Constants;
using Microsoft.EntityFrameworkCore;
using FantasyLCS.DataObjects;

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

        List<Team> leagueTeams = teams.Where(team => nullableTeamIDs.Contains(team.ID)).ToList();

        if (league != null && league.LeagueStatus == LeagueStatus.NotStarted)
        {
            league.LeagueStatus = LeagueStatus.DraftInProgress;
            _context.Update(league);

            var draft = new Draft
            {
                LeagueID = leagueId,
                DraftPlayers = DraftPlayerConstants.DraftPlayers,
                DraftOrder = InitializeDraftOrder(teamIDs),
                CurrentRound = 1,
                CurrentPickIndex = 0
            };

            _context.Drafts.Add(draft);
            await _context.SaveChangesAsync();

            var firstTeam = _context.Teams.Find(draft.DraftOrder[0]);

            await Clients.Group(leagueId.ToString()).SendAsync("DraftStarted");
            await Clients.Group(leagueId.ToString()).SendAsync("UpdateCurrentTeam", firstTeam);
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
        var draft = _context.Drafts.FirstOrDefault(d => d.LeagueID == leagueId);
        var draftPlayers = _context.DraftPlayers.Where(dp => dp.DraftID == draft.ID).ToList();
        if (draft != null)
        {
            var draftPlayer = draftPlayers.FirstOrDefault(dp => dp.ID == playerId);
            if (draftPlayer != null && !draftPlayer.Drafted)
            {
                draftPlayer.Drafted = true;

                // Update current pick and round
                UpdateDraftTurn(draft);

                _context.Update(draft);
                await _context.SaveChangesAsync();

                var nextTeam = _context.Teams.Find(draft.DraftOrder[draft.CurrentPickIndex]);
                var playerDrafted = new
                {
                    draftPlayerID = draftPlayer.ID,
                    nextTeam = nextTeam
                };
                await Clients.Group(leagueId.ToString()).SendAsync("PlayerDrafted", playerDrafted);
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

            // Reverse the draft order for the next round if it's an even round (snake draft)
            if (draft.CurrentRound % 2 == 0)
            {
                draft.DraftOrder.Reverse();
            }
        }
    }

    public async Task GetCurrentDraftState(int leagueId)
    {
        var league = _context.Leagues.Find(leagueId);
        if (league != null)
        {
            var draft = _context.Drafts.FirstOrDefault(d => d.LeagueID == leagueId);
            if (draft != null)
            {
                var draftPlayers = _context.DraftPlayers.Where(dp => dp.DraftID == draft.ID).ToList();
                var currentTeam = _context.Teams.Find(draft.DraftOrder[draft.CurrentPickIndex]);
                List<int> draftedPlayerIDs = draftPlayers.Where(dp => dp.Drafted).Select(dp => dp.ID).ToList();
                var draftStatus = new
                {
                    LeagueStatus = league.LeagueStatus,
                    CurrentTeam = currentTeam,
                    DraftedPlayerIDs = draftedPlayerIDs
                };

                await Clients.Caller.SendAsync("UpdateDraftState", draftStatus);
            }
        }
    }

    public async Task JoinGroup(int leagueID)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, leagueID.ToString());
    }
}
