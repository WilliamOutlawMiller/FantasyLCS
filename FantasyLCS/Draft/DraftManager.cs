using FantasyLCS.DataObjects;

public class DraftManager
{
    private readonly HttpClient _httpClient;
    private List<Team> _teams;
    private int _currentRound;

    public DraftManager(HttpClient httpClient, List<Team> teams)
    {
        _httpClient = httpClient;
        _teams = teams;
        _currentRound = 1;
    }

    public async Task StartDraftAsync()
    {
        int totalRounds = 5;
        while (_currentRound <= totalRounds)
        {
            foreach (var team in GetDraftOrderForRound(_currentRound))
            {
                // Here you'd have the UI logic to select a player.
                // For example, it could be a user input that returns the selected player's ID.
                int selectedPlayerID = await GetUserSelectedPlayerId(team);

                // Add the selected player to the team using the API endpoint.
                await AddPlayerToTeamAsync(team.ID, selectedPlayerID);

                // Remove the selected player from the available players list.
                await UpdateAvailablePlayerList(selectedPlayerID);
            }

            _currentRound++;
            ReverseDraftOrder();
        }
    }

    private IEnumerable<Team> GetDraftOrderForRound(int round)
    {
        return _teams;
    }

    private void ReverseDraftOrder()
    {
        _teams.Reverse();
    }

    private async Task<int> GetUserSelectedPlayerId(Team team)
    {
        // Placeholder for UI logic to get the selected player ID
        // This could be a method that waits for user input and returns the selected player's ID
        return 0; // Example return value
    }

    private async Task AddPlayerToTeamAsync(int teamId, int playerId)
    {
        // Call to /addplayertoteam/{teamID}/{playerID} API endpoint
        var response = await _httpClient.PostAsync($"api/addplayertoteam/{teamId}/{playerId}", null);
        response.EnsureSuccessStatusCode();
    }

    private async Task UpdateAvailablePlayerList(int playerId)
    {
        // Logic to remove the player from the available players list
        // This could involve updating a local list or making an API call if needed
    }
}
