using FantasyLCS.DataObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.IO;

using static FantasyLCS.App.Classes.LogoHelper;

namespace FantasyLCS.App.Classes
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private ObservableCollection<Player> _availablePlayers;
        private ObservableCollection<Team> _teams;
        private Player _selectedPlayer;
        private Team _selectedTeam;

        private Team _userTeam;
        private string _username;

        private string _imagesFolderPath 
        { 
            get
            {
                string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Path.Combine(basePath, "Images");
            } 
        }

        public MainViewModel(ApiService apiService, string username)
        {
            _apiService = apiService;
            _username = username;

            InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Load AvailablePlayers only if it's not already loaded
                if (AvailablePlayers == null || !AvailablePlayers.Any())
                {
                    AvailablePlayers = await _apiService.LoadAvailablePlayersAsync();
                }

                // Load Teams only if it's not already loaded
                if (Teams == null || !Teams.Any())
                {
                    Teams = await _apiService.LoadTeamsAsync();
                }

                if (UserTeam == null)
                {
                    UserTeam = Teams.FirstOrDefault(team => team.OwnerName.Equals(Username));
                    if (UserTeam != null)
                    {
                        UserTeam.LogoPath = Path.Combine(_imagesFolderPath, $"{UserTeam.Name}.png");
                        OnPropertyChanged(nameof(UserTeam)); // Notify the View of the change
                    }
                }

                if (UserTeam != null && UserTeam.LogoPath != null && !File.Exists(UserTeam.LogoPath))
                {
                    await LoadAndDisplayImage();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, maybe log the error
                // Optionally, set a property to indicate that data loading failed
            }
        }


        public ObservableCollection<Player> AvailablePlayers
        {
            get => _availablePlayers;
            set
            {
                _availablePlayers = value;
                OnPropertyChanged(nameof(AvailablePlayers));
            }
        }

        public ObservableCollection<Team> Teams
        {
            get => _teams;
            set
            {
                _teams = value;
                OnPropertyChanged(nameof(Teams));
            }
        }

        public Player SelectedPlayer
        {
            get => _selectedPlayer;
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));
                // Additional logic when a player is selected
            }
        }

        public Team SelectedTeam
        {
            get => _selectedTeam;
            set
            {
                _selectedTeam = value;
                OnPropertyChanged(nameof(SelectedTeam));
                // Additional logic when a team is selected
            }
        }
        public Team UserTeam
        {
            get => _userTeam;
            set
            {
                if (_userTeam != value)
                {
                    _userTeam = value;
                    OnPropertyChanged(nameof(UserTeam));
                    OnPropertyChanged(nameof(IsUserTeamAvailable)); // Notify that IsUserTeamAvailable has changed
                    OnPropertyChanged(nameof(IsCreateTeamButtonVisible));
                }
            }
        }

        public bool IsUserTeamAvailable => UserTeam != null;
        public bool IsCreateTeamButtonVisible => !IsUserTeamAvailable;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                // Additional logic when a team is selected
            }
        }

        public async Task<bool> CreateTeam(string teamName, string logoUrl)
        {
            // Call the ApiService to create a team
            var result = await _apiService.CreateTeamAsync(teamName, logoUrl, _username);
            if (result)
            {
                Teams = await _apiService.LoadTeamsAsync();
                UserTeam = Teams.FirstOrDefault(team => team.OwnerName.Equals(Username));

                UserTeam.LogoPath = Path.Combine(_imagesFolderPath, $"{teamName}.png");
            }
            return result;
        }

        public async Task<bool> DeleteTeam()
        {
            // Call the ApiService to create a team
            var result = await _apiService.DeleteTeamAsync(UserTeam.Name, UserTeam.OwnerName);
            if (result)
            {
                Teams = await _apiService.LoadTeamsAsync();
                UserTeam = null;
            }
            return result;
        }

        public async Task LoadAndDisplayImage()
        {
            await DownloadImageAsync(UserTeam.LogoUrl, UserTeam.LogoPath);
            OnPropertyChanged(nameof(UserTeam)); // Notify the View of the change
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
