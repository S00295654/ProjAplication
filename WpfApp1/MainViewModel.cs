using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WpfApp1
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly GameApiService _apiService = new GameApiService();
        public ICommand OpenGameCommand { get; }
        private Game selectedGame;
        public ICollectionView PlannedGamesView { get; }
        private CancellationTokenSource _searchCancellation;
        private DispatcherTimer _searchTimer;
        private string _lastApiSearch = "";
        public Game SelectedGame
        {
            get => selectedGame;
            set
            {
                selectedGame = value;
                OnPropertyChanged();
                OpenGameDetails();
            }
        }
        private void OpenGameDetails()
        {
            if (SelectedGame == null)
                return;
            var window = new GameDetailsWindow(SelectedGame, User, AllGames);
            window.Show();
        }

        public ObservableCollection<Game> AllGames { get; set; }
        public ObservableCollection<Game> FilteredGames { get; set; }
        public ICollectionView GamesGroupedView { get; }
        private User user;
        public User User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged();
            }
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();

                _searchTimer.Stop();
                _searchTimer.Start();
            }
        }
        public ICommand EditUserCommand { get; set; }
        public ICommand AddGameCommand { get; set; }
        private void Game_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Game.State))
            {
                GamesGroupedView.Refresh();
            }
        }

        private bool isSearching;
        public bool IsSearching
        {
            get => isSearching;
            set
            {
                isSearching = value;
                OnPropertyChanged();
            }
        }




        public MainViewModel()
        {
            var save = SaveManager.Load();
            
            if (save != null)
            {
                AllGames = new ObservableCollection<Game>(
                    save.AllGames.Select(g => FromGameData(g))
                );

                User = new User(save.User.Name)
                {
                    Description = save.User.Description,
                    ProfileImage = !string.IsNullOrEmpty(save.User.ProfileImagePath)
                    ? LoadImage(save.User.ProfileImagePath)
                    : null
                };

                foreach (GameData g in save.User.Games)
                {
                    var existingGame = AllGames.FirstOrDefault(x => x.Name == g.Name);
                    if (existingGame != null)
                    {
                        User.Games.Add(existingGame);
                    }
                }

                foreach (var g in User.Games)
                {
                    g.PropertyChanged += Game_PropertyChanged;
                }
            }
            else
            {
                AllGames = new ObservableCollection<Game> { };
                /*{
                    new Game { Name = "Elden Ring" },
                    new Game { Name = "Minecraft", Illustration=new BitmapImage(new Uri("Images/Minecraft.jpg", UriKind.Relative)) },
                    new Game { Name = "Hades", Illustration=new BitmapImage(new Uri("Images/Hades.jpg", UriKind.Relative)) },
                    new Game { Name = "Hades 2", Illustration=new BitmapImage(new Uri("Images/Hades_2.jpeg", UriKind.Relative))},
                    new Game { Name = "Star Wars The Old Republic", Illustration=new BitmapImage(new Uri("Images/SWTOR.jpg", UriKind.Relative)) },
                    new Game { Name = "Overwatch", Illustration=new BitmapImage(new Uri("Images/overwatch.jpg", UriKind.Relative)) },
                    new Game { Name = "CS2",  Illustration=new BitmapImage(new Uri("Images/CS2.png", UriKind.Relative)) },
                    new Game { Name = "Tetris" }
                };*/
                User = new User("New User");
                User.Description = "There are no description yet... However it can change when you want.";
                //Testcontent(AllGames);
            }
            foreach (var game in User.Games)
            {
                game.RemoveCommand = new RelayCommand(obj => 
                {
                    User.Games.Remove(game);
                });
            }
            GamesGroupedView = new ListCollectionView(User.Games);

            GamesGroupedView.GroupDescriptions.Add(
                new PropertyGroupDescription(nameof(Game.State))
            );

            GamesGroupedView.SortDescriptions.Add(
                new SortDescription(nameof(Game.State), ListSortDirection.Ascending)
            );

            FilteredGames = new ObservableCollection<Game>(AllGames);
            OpenGameCommand = new RelayCommand(game =>
            {
                if (game is Game g)
                {
                    if (!AllGames.Contains(g))
                    { 
                        AllGames.Add(g);
                        if (g.release == null || g.release[0] == "")
                            g.release = new List<string> {"Unknown" };
                    }
                    var window = new GameDetailsWindowList(g, User.Games, User, AllGames);
                    window.Show();
                }
            });
            EditUserCommand = new RelayCommand(obj =>
            {
                var window = new EditUserWindow(User);
                window.ShowDialog(); // Modal
                                        // Les modifications sont déjà appliquées directement via bindings
            });
            AddGameCommand = new RelayCommand(obj =>
            {
                var window = new AddGameWindow();

                if (window.ShowDialog() == true)
                {
                    Game newGame = window.NewGame;

                    // Sécurisation
                    if (newGame.genre == null)
                        newGame.genre = new List<string>();
                    if (newGame.device == null)
                        newGame.device = new List<string>();
                    if (newGame.tags == null)
                        newGame.tags = new List<string>();
                    if (newGame.release == null || newGame.release[0] == "")
                    {
                        newGame.release = new List<string>() {"Unknown"};
                    }

                    if (newGame.Illustration == null)
                        newGame.Illustration = new BitmapImage(
                            new Uri("Images/GameTest.png", UriKind.Relative));

                    newGame.State = GameState.Playing;
                    newGame.Score = newGame.Score;
                    newGame.Time = newGame.Time;

                    AllGames.Add(newGame);
                    _ = FilterAsync();
                }
            });
            User.Games.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (Game g in e.NewItems)
                    {
                        g.PropertyChanged += Game_PropertyChanged;
                    }
                }
            };

            _searchTimer = new DispatcherTimer();
            _searchTimer.Interval = TimeSpan.FromMilliseconds(500);
            _searchTimer.Tick += async (s, e) =>
            {
                _searchTimer.Stop();
                await FilterAsync();
            };

        }

        private Game FromGameData(GameData g)
        {
            return new Game
            {
                Name = g.Name,
                tags = g.tags ?? new List<string>(),
                genre = g.genre ?? new List<string>(),
                device = g.device ?? new List<string>(),
                release = g.release ?? new List<string>() { "Unknown"},
                Illustration = LoadImage(g.ImagePath),
                Time = g.Time,
                Score = g.Score,
                State = g.State
            };
        }

        private BitmapImage LoadImage(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            string fullPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                path);

            if (!File.Exists(fullPath))
                return null;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }


        public void Testcontent(ObservableCollection<Game> Allgames)
        {

            Allgames[0].release = new List<string>{
                "February 25, 2022"
            }; 
            Allgames[0].genre= new List<string>{
                "Action","role-playing"
            };
            Allgames[0].device = new List<string>{
                "PlayStation 4","PlayStation 5","Windows","Xbox One","Xbox Series X/S","Nintendo Switch 2"
            };
            Allgames[0].tags = new List<string>{
                "Windows, PS4, PS5, Xbox One, Series X/S :February 25 2022", "Nintendo Switch 2 2026"
            };
            foreach (Game game in Allgames)
            {
                if (game.Illustration==null)
                    game.Illustration = new BitmapImage(new Uri("Images/GameTest.png", UriKind.Relative));
                game.Time = 0;
                if (game.genre==null)
                    game.genre = new List<string>();
                if (game.device==null)
                    game.device = new List<string>();

                game.State = GameState.Playing;
                game.Score = 0;
                game.Time = 0;
            }
            Allgames[0].Illustration= new BitmapImage(new Uri("Images/EldenRing.jpg", UriKind.Relative));

        }

        private async Task FilterAsync()
        {
            _searchCancellation?.Cancel();
            _searchCancellation = new CancellationTokenSource();
            var token = _searchCancellation.Token;

            FilteredGames.Clear();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var g in AllGames)
                    FilteredGames.Add(g);
                return;
            }

            foreach (var g in AllGames)
            {
                if (IsMatch(g, SearchText))
                {
                    FilteredGames.Add(g);
                }
            }

            //API seulement si +3 lettres et si la recherche est nouvelle
            if (SearchText.Length < 3 || SearchText == _lastApiSearch)
                return;

            _lastApiSearch = SearchText;

            IsSearching = true;
            var apiGames = await _apiService.SearchGames(SearchText);
            IsSearching = false;

            if (token.IsCancellationRequested)
                return;

            foreach (var game in apiGames)
            {
                bool alreadyExists =
                    AllGames.Any(x => x.Name.Equals(game.Name, StringComparison.OrdinalIgnoreCase)) ||
                    FilteredGames.Any(x => x.Name.Equals(game.Name, StringComparison.OrdinalIgnoreCase));

                if (!alreadyExists)
                    FilteredGames.Add(game);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private bool IsMatch(Game game, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return true;

            var terms = search.ToLower()
                .Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);

            foreach (var term in terms)
            {
                bool match = false;

                // 🎮 Nom
                if (!string.IsNullOrEmpty(game.Name) &&
                    game.Name.ToLower().Contains(term))
                {
                    match = true;
                }

                // Plateformes
                if (game.device != null &&
                    game.device.Any(d => d.ToLower().Contains(term)))
                {
                    match = true;
                }

                // Date (année)
                if (game.release != null &&
                    game.release.Any(r => r != null && r.Contains(term)))
                {
                    match = true;
                }

                // Si un mot ne correspond à rien : on exclut
                if (!match)
                    return false;
            }

            return true;
        }

    }
    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        
    }

    public enum GameState
    {
        Playing = 0,
        Planned = 1,
        Finished = 2,
        Stop = 3,
        Dropped = 4
    }
}
