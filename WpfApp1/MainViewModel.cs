using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand OpenGameCommand { get; }
        private Game selectedGame;
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

        public MainViewModel()
        {
            AllGames = new ObservableCollection<Game>
        {
            new Game { Name = "Elden Ring" },
            new Game { Name = "Minecraft" },
            new Game { Name = "Hades" },
            new Game { Name = "Hades 2" },
            new Game { Name = "Star Wars The Old Republic" }
        };
            User = new User("New User");
            User.Description = "There are no description yet... However it can change when you want. Test Test Test Test Test Test Test Test Test Test Test Test Test Test";
            Testcontent(AllGames);
            foreach (var game in User.Games)
            {
                game.RemoveCommand = new RelayCommand(obj =>
                {
                    User.Games.Remove(game);
                });
            }


            FilteredGames = new ObservableCollection<Game>(AllGames);
            OpenGameCommand = new RelayCommand(game =>
            {
                if (game is Game g)
                {
                    var window = new GameDetailsWindowList(g, User.Games);
                    window.Show();
                }
            });
        }

        public void Testcontent(ObservableCollection<Game> Allgames)
        {

            Allgames[0].release = new List<string>{
                "February 25, 2022"
            }; 
            Allgames[0].genre= new List<string>{
                "Adventure"
            };
            Allgames[0].device = new List<string>{
                "PC"
            };
            Allgames[0].tags = new List<string>{
                "Die and Retry"
            };
            foreach (Game game in Allgames)
            {
                game.Illustration = new BitmapImage(new Uri("Images/GameTest.png", UriKind.Relative));
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
}
