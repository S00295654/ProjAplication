using Microsoft.Win32;
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
    /// Interaction logic for AddGameWindow.xaml
    /// </summary>
    public partial class AddGameWindow : Window
    {
        public Game NewGame { get; private set; }

        public AddGameWindow()
        {
            InitializeComponent();
            NewGame = new Game();
            DataContext = NewGame;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NewGame.genre = GenreBox.Text.Split(',')
                            .Select(x => x.Trim()).ToList();

            NewGame.device = DeviceBox.Text.Split(',')
                            .Select(x => x.Trim()).ToList();

            NewGame.release = new List<string> { ReleaseBox.Text };

            DialogResult = true;
            Close();
        }

        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

            if (dialog.ShowDialog() == true)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(dialog.FileName);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();

                NewGame.Illustration = image;
            }
        }
    }
}
