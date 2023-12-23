using FantasyLCS.DataObjects;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseApiUrl; // Base URL of your API

    public ApiService(HttpClient httpClient, string baseApiUrl)
    {
        _httpClient = httpClient;
        _baseApiUrl = baseApiUrl;
    }

    public async Task<ObservableCollection<Player>> LoadAvailablePlayersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/getavailableplayers");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ObservableCollection<Player>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            // Handle not found or other unsuccessful status codes
        }
        catch (Exception ex)
        {
            // Handle exceptions
        }
        return new ObservableCollection<Player>();
    }

    public async Task<ObservableCollection<Team>> LoadTeamsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/getallteams");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ObservableCollection<Team>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            // Handle not found or other unsuccessful status codes
        }
        catch (Exception ex)
        {
            // Handle exceptions
        }
        return new ObservableCollection<Team>();
    }

    public async Task<bool> CreateTeamAsync(string name, string username)
    {
        try
        {
            // Create an object to send in the request body
            var requestData = new { name, username };

            // Serialize the requestData to JSON
            var jsonRequest = JsonSerializer.Serialize(requestData);

            // Create a StringContent from the JSON data
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Send a POST request to the /createteam/{name}/{username} endpoint
            var response = await _httpClient.PostAsync($"/createteam/{name}/{username}", content);

            // Check if the request was successful (status code 200)
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                // Handle the error here if needed
                return false;
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions here if needed
            return false;
        }
    }
}
