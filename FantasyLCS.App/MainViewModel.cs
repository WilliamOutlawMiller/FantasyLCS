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
            LoadDataAsync();
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
                _userTeam = value;
                OnPropertyChanged(nameof(UserTeam));
                // Additional logic when a team is selected
            }
        }

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

        // Method to load data (e.g., players and teams) asynchronously
        private async void LoadDataAsync()
        {
            AvailablePlayers = await _apiService.LoadAvailablePlayersAsync();
            Teams = await _apiService.LoadTeamsAsync();
            UserTeam = Teams.Where(team => team.OwnerName.Equals(Username)).SingleOrDefault();

            if (UserTeam == null)
                IsCreateTeamButtonVisible = true;
            else
                IsCreateTeamButtonVisible = false;
        }

        // Method to raise the PropertyChanged event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
