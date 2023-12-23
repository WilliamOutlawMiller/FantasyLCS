using FantasyLCS.DataObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FantasyLCS.App
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

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel(ApiService apiService, string username)
        {
            _apiService = apiService;
            _username = username;
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

        public async Task InitializeAsync()
        {
            try
            {
                AvailablePlayers = await _apiService.LoadAvailablePlayersAsync();
                Teams = await _apiService.LoadTeamsAsync();
                UserTeam = Teams.FirstOrDefault(team => team.OwnerName.Equals(Username));
            }
            catch (Exception ex)
            {
                // Handle exceptions, maybe log the error
                // Optionally, set a property to indicate that data loading failed
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
            }
            return result;
        }

        // Method to raise the PropertyChanged event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
