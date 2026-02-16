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
    public partial class EditUserWindow : Window
    {
        public User User { get; set; }

        public EditUserWindow(User user)
        {
            InitializeComponent();
            User = user;
            DataContext = User; // Bind directement au User
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == true)
            {
                User.ProfileImage = new BitmapImage(new Uri(ofd.FileName));
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Les modifications sont déjà dans User via les bindings
            this.DialogResult = true;
            this.Close();
            MainWindow.reloadImage.Source = User.ProfileImage;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
