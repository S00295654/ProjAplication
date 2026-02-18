using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand OpenGameCommand { get; }
        private Game selectedGame;
        public ICollectionView PlannedGamesView { get; }
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

            var window = new GameDetailsWindow(SelectedGame, User);
            window.Show();
        }

        public ObservableCollection<Game> AllGames { get; set; }
        public ObservableCollection<Game> FilteredGames { get; set; }
        public ICollectionView GamesGroupedView { get; }
        public User User { get; set; }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
                Filter();
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
                        ? new BitmapImage(new Uri(save.User.ProfileImagePath, UriKind.Relative))
                        : null
                };

                foreach (var g in save.User.Games)
                {
                    User.Games.Add(FromGameData(g));
                }
            }
            else
            {
                AllGames = new ObservableCollection<Game>
                {
                    new Game { Name = "Elden Ring" },
                    new Game { Name = "Minecraft", Illustration=new BitmapImage(new Uri("Images/Minecraft.jpg", UriKind.Relative)) },
                    new Game { Name = "Hades", Illustration=new BitmapImage(new Uri("Images/Hades.jpg", UriKind.Relative)) },
                    new Game { Name = "Hades 2", Illustration=new BitmapImage(new Uri("Images/Hades_2.jpeg", UriKind.Relative))},
                    new Game { Name = "Star Wars The Old Republic", Illustration=new BitmapImage(new Uri("Images/SWTOR.jpg", UriKind.Relative)) },
                    new Game { Name = "Overwatch", Illustration=new BitmapImage(new Uri("Images/overwatch.jpg", UriKind.Relative)) },
                    new Game { Name = "CS2",  Illustration=new BitmapImage(new Uri("Images/CS2.png", UriKind.Relative)) },
                    new Game { Name = "Tetris" }
                };
                User = new User("New User");
                User.Description = "There are no description yet... However it can change when you want. Test Test Test Test Test Test Test Test Test Test Test Test Test Test";
                Testcontent(AllGames);
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
                        var window = new GameDetailsWindowList(g, User.Games, User);
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
                        if (newGame.release == null)
                            newGame.release = new List<string>();

                        if (newGame.Illustration == null)
                            newGame.Illustration = new BitmapImage(
                                new Uri("Images/GameTest.png", UriKind.Relative));

                        newGame.State = GameState.Playing;
                        newGame.Score = newGame.Score;
                        newGame.Time = newGame.Time;

                        AllGames.Add(newGame);
                        Filter();
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
                
            
        }
        private Game FromGameData(GameData g)
        {
            return new Game
            {
                Name = g.Name,
                tags = g.tags ?? new List<string>(),
                genre = g.genre ?? new List<string>(),
                device = g.device ?? new List<string>(),
                release = g.release ?? new List<string>(),
                Illustration = !string.IsNullOrEmpty(g.ImagePath)
                    ? new BitmapImage(new Uri(g.ImagePath, UriKind.Relative))
                    : null,
                Time = g.Time,
                Score = g.Score,
                State = g.State
            };
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

        private void Filter()
        {
            FilteredGames.Clear();

            foreach (var g in AllGames)
            {
                if (string.IsNullOrEmpty(SearchText) ||
                    g.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    FilteredGames.Add(g);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

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
        Stop = 3
    }
}
