using System;
using System.Collections.Generic;
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

        public GameDetailsWindow(Game game,User user)
        {
            InitializeComponent();
            DataContext = game;

            CurrentUser = user;
            CurrentGame = game;
        }
        private void AddToUserGames_Click(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.Games.Contains(CurrentGame))
            {
                CurrentUser.Games.Add(CurrentGame);
                MessageBox.Show($"{CurrentGame.Name} a été ajouté à votre liste !");
            }
            else
            {
                MessageBox.Show("Ce jeu est déjà dans votre liste.");
            }
        }
    }
}
