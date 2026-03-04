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
    /// Interaction logic for GameDetailsWindow.xaml
    /// </summary>
    public partial class GameDetailsWindow : Window
    {
        private User CurrentUser;
        private Game CurrentGame;
        private ObservableCollection<Game> Allgames;

        public GameDetailsWindow(Game game,User user, ObservableCollection<Game> allgames)
        {
            InitializeComponent();
            DataContext = game;

            CurrentUser = user;
            CurrentGame = game;
            Allgames = allgames;
        }
        private void AddToUserGames_Click(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.Games.Contains(CurrentGame))
            {
                CurrentUser.Games.Add(CurrentGame);
                if (!Allgames.Contains(CurrentGame))
                {
                    Allgames.Add(CurrentGame);
                }
                MessageBox.Show($"{CurrentGame.Name} as been added to your list !");
            }
            else
            {
                MessageBox.Show("This game is already in your list.");
            }
            this.Close();
        }
    }
}
