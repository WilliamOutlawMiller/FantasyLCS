using FantasyLCS.DataObjects;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseApiUrl;
    private readonly string _apiKey;

    public ApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _baseApiUrl = configuration["ApiSettings:BaseApiUrl"];
        _httpClient = httpClient;
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

    public async Task<bool> CreateTeamAsync(string name, string logoUrl, string username)
    {
        try
        {
            // Create an object to send in the request body
            var requestData = new { name, logoUrl, username };

            // Serialize the requestData to JSON
            var jsonRequest = JsonSerializer.Serialize(requestData);

            // Create a StringContent from the JSON data
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Send a POST request to the /createteam endpoint
            var response = await _httpClient.PostAsync($"{_baseApiUrl}/createteam", content);

            // Check if the request was successful (status code 200)
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions here if needed
            return false;
        }
    }

    public async Task<bool> DeleteTeamAsync(string teamName, string ownerName)
    {
        try
        {
            // Create an object to send in the request body
            var requestData = new { teamName, ownerName };

            // Serialize the requestData to JSON
            var jsonRequest = JsonSerializer.Serialize(requestData);

            // Create a StringContent from the JSON data
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Send a POST request to the /createteam endpoint
            var response = await _httpClient.PostAsync($"{_baseApiUrl}/deleteteam", content);

            // Check if the request was successful (status code 200)
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions here if needed
            return false;
        }
    }

    public async Task<Team> LoadTeamInformationAsync(string username)
    {
        try
        {
            // Call the new API endpoint to get the team by username
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/getteambyusername/{username}");

            // Check if the request was successful (status code 200)
            if (response.IsSuccessStatusCode)
            {
                var teamJson = await response.Content.ReadAsStringAsync();
                var team = JsonSerializer.Deserialize<Team>(teamJson);

                return team;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Handle the case where no team was found for the username
                return null;
            }
            else
            {
                // Handle other error cases here if needed
                return null;
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions here if needed
            return null;
        }
    }
}
