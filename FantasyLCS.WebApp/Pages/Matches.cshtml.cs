using Constants;
using FantasyLCS.DataObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace FantasyLCS.WebApp.Pages
{
    public class MatchesModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly string _apiUrl;

        public MatchesModel(HttpClient httpClient, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _cache = memoryCache;
            _apiUrl = configuration["ApiSettings:BaseUrl"];
        }

        public List<LeagueMatch> Matches { get; set; }

        public HomePage HomePage { get; set; }

        public Dictionary<Tuple<int, int>, Score> LeagueMatchScores { get; set; } = new Dictionary<Tuple<int, int>, Score>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                string matchCacheKey = $"MatchPageData-{User.Identity.Name}";
                string scoresCacheKey = $"ScorePageData-{User.Identity.Name}";
                string homeCacheKey = $"HomePageData-{User.Identity.Name}";
                HomePage cachedHomePage;
                List<LeagueMatch> cachedMatches;
                Dictionary<Tuple<int, int>, Score> cachedScores;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                // Get HomePage
                if (_cache.TryGetValue(homeCacheKey, out cachedHomePage))
                    HomePage = cachedHomePage;
                else
                    return RedirectToPage("/Home");

                if (HomePage.UserLeague == null)
                        return RedirectToPage("/Home");
                if ((int)HomePage.UserLeague.LeagueStatus < 1)
                        return RedirectToPage("/Home");

                // Get LeagueMatches
                if (_cache.TryGetValue(matchCacheKey, out cachedMatches))
                    Matches = cachedMatches;
                else
                {
                    var response = await _httpClient.GetAsync(_apiUrl + $"/getleaguematches/{HomePage.UserLeague.ID}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        Matches = JsonSerializer.Deserialize<List<LeagueMatch>>(responseBody);
                    }
                }

                // Get Scores
                if (_cache.TryGetValue(scoresCacheKey, out cachedScores))
                {
                    LeagueMatchScores = cachedScores;
                }
                else
                {
                    if (Matches.Count > 0)
                    {
                        // todo: refactor
                        foreach (var match in Matches)
                        {
                            var response = await _httpClient.GetAsync(_apiUrl + $"/getleaguematchscores/{match.ID}");

                            if (response.IsSuccessStatusCode)
                            {
                                var responseBody = await response.Content.ReadAsStringAsync();
                                var scores = JsonSerializer.Deserialize<List<Score>>(responseBody);

                                foreach (var score in scores)
                                {
                                    Player player = HomePage.AllPlayers.SingleOrDefault(p => p.ID == score.PlayerID);
                                    DraftPlayer draftPlayer = HomePage.LeagueDraftPlayers.SingleOrDefault(dp => dp.Name.ToLower().Equals(player.Name.ToLower()));
                                    if (draftPlayer == null)
                                    {
                                        foreach (var entry in Substitutes.SubstitutePlayerIDs)
                                        {
                                            if (entry.Value == player.ID)
                                            {
                                                player = HomePage.AllPlayers.SingleOrDefault(p => p.ID == entry.Key);
                                                draftPlayer = HomePage.LeagueDraftPlayers.SingleOrDefault(dp => dp.Name.ToLower().Equals(player.Name.ToLower()));
                                            }
                                        }
                                    }
                                    Tuple<int, int> matchPlayerPairing = Tuple.Create(match.ID, draftPlayer.ID);
                                    LeagueMatchScores.Add(matchPlayerPairing, score);
                                }


                            }
                        }
                    }
                }

                _cache.Set(matchCacheKey, Matches, cacheEntryOptions);

                _cache.Set(scoresCacheKey, LeagueMatchScores, cacheEntryOptions);
            }
            else
            {
                // If user is not authenticated, redirect to login page
                return RedirectToPage("/Login");
            }

            return Page();
        }
    }
}
