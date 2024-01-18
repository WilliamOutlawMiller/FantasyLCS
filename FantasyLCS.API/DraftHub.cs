using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic; // Needed for List
using System; // Needed for Random
using Constants;

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
        if (league != null && league.LeagueStatus == LeagueStatus.NotStarted)
        {
            league.LeagueStatus = LeagueStatus.DraftInProgress;
            _context.Update(league);

            var draft = new Draft
            {
                LeagueID = leagueId,
                DraftPlayers = DraftPlayerConstants.DraftPlayers,
                DraftOrder = InitializeDraftOrder(league.UserIDs),
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

    private List<int> InitializeDraftOrder(List<int> userIds)
    {
        var random = new Random();
        var draftOrder = userIds.OrderBy(x => random.Next()).ToList();
        return draftOrder;
    }

    public async Task PlayerDrafted(int leagueId, int playerId)
    {
        var draft = _context.Drafts.FirstOrDefault(d => d.LeagueID == leagueId);
        if (draft != null)
        {
            var draftPlayer = draft.DraftPlayers.FirstOrDefault(dp => dp.ID == playerId);
            if (draftPlayer != null && !draftPlayer.Drafted)
            {
                draftPlayer.Drafted = true;

                // Update current pick and round
                UpdateDraftTurn(draft);

                _context.Update(draft);
                await _context.SaveChangesAsync();

                var nextTeam = _context.Teams.Find(draft.DraftOrder[draft.CurrentPickIndex]);

                await Clients.Group(leagueId.ToString()).SendAsync("PlayerDrafted", draftPlayer);
                await Clients.Group(leagueId.ToString()).SendAsync("UpdateCurrentTeam", nextTeam);
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
}
