using FantasyLCS.DataObjects;
using System.Collections.ObjectModel;
using System.Net.Http;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ObservableCollection<Player>> LoadAvailablePlayersAsync()
    {
        // Logic to call the API and get available players
        // Deserialize the response and return the players collection
    }

    public async Task<ObservableCollection<Team>> LoadTeamsAsync()
    {
        // Logic to call the API and get teams
        // Deserialize the response and return the teams collection
    }
}
