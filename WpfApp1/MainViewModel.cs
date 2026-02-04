using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Game> AllGames { get; set; }
        public ObservableCollection<Game> FilteredGames { get; set; }

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
            new Game { Name = "Hades" }
        };

            FilteredGames = new ObservableCollection<Game>(AllGames);
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
}
