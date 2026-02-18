using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for GameDetailsWindowList.xaml
    /// </summary>
    public partial class GameDetailsWindowList : Window
    {
        private Game _game;
        private ObservableCollection<Game> _games;
        private User User;

        public GameDetailsWindowList(Game game, ObservableCollection<Game> games, User user)
        {

            InitializeComponent();
            // DataContext = Game pour bindings simples
            StateComboBox.ItemsSource = Enum.GetValues(typeof(GameState));
            this.DataContext = game;

            _games = games;
            _game= game;
            User = user;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Tout est déjà modifié via les bindings
            this.Close();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is Game game && _games.Contains(game))
            {
                _games.Remove(game);
                this.Close();
            }
        }
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new GameDetailsWindow(_game, User);
            window.Show();
        }
    }
 }
