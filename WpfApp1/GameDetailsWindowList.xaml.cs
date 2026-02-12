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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for GameDetailsWindowList.xaml
    /// </summary>
    public partial class GameDetailsWindowList : Window
    {
        private ObservableCollection<Game> _games;

        public GameDetailsWindowList(Game game, ObservableCollection<Game> games)
        {
            

            // DataContext = Game pour bindings simples
            this.DataContext = game;

            _games = games; // garder la collection pour le Remove
            InitializeComponent();
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
    }
 }
