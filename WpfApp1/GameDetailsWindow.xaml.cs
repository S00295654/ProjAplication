using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using Path = System.IO.Path;

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
        private async void AddToUserGames_Click(object sender, RoutedEventArgs e)
        {
            if (!Allgames.Contains(CurrentGame))
            {
                Allgames.Add(CurrentGame);

                // Télécharger l’image seulement ici
                if (!string.IsNullOrEmpty(CurrentGame.OnlineImageUrl))
                {
                    string localPath = await DownloadImageAsync(
                        CurrentGame.OnlineImageUrl,
                        CurrentGame.Name);

                    string fullPath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        localPath);

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                    bitmap.EndInit();
                    bitmap.Freeze();

                    CurrentGame.Illustration = bitmap;

                    CurrentGame.OnlineImageUrl = null;
                }

                CurrentUser.Games.Add(CurrentGame);

                MessageBox.Show($"{CurrentGame.Name} has been added to your list!");
            }
            else
            {
                MessageBox.Show("This game is already in your list.");
            }

            this.Close();
        }

        private async Task<string> DownloadImageAsync(string imageUrl, string gameName)
        {
            string imagesFolder = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Images");

            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            foreach (char c in Path.GetInvalidFileNameChars())
                gameName = gameName.Replace(c, '_');

            string fileName = gameName.Replace(" ", "_") + ".jpg";
            string fullPath = Path.Combine(imagesFolder, fileName);

            if (!File.Exists(fullPath))
            {
                using (HttpClient client = new HttpClient())
                {
                    var imageBytes = await client.GetByteArrayAsync(imageUrl);
                    File.WriteAllBytes(fullPath, imageBytes); // ← ici
                }
            }

            return $"Images/{fileName}";
        }
    }
}
