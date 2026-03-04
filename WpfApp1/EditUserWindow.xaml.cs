using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using IOPath = System.IO.Path;

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
                try
                {
                    string saveFolder = IOPath.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Images",
                        "Save"
                    );

                    if (!Directory.Exists(saveFolder))
                        Directory.CreateDirectory(saveFolder);

                    string extension = IOPath.GetExtension(ofd.FileName);
                    string newFileName = $"profile_{Guid.NewGuid()}{extension}";
                    string destinationPath = IOPath.Combine(saveFolder, newFileName);

                    File.Copy(ofd.FileName, destinationPath, true);

                    // 🔹 Charger avec chemin ABSOLU
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(destinationPath, UriKind.Absolute);
                    bitmap.EndInit();
                    bitmap.Freeze(); // évite les problèmes de lock

                    User.ProfileImage = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur copie de l'image : " + ex.Message);
                }
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Les modifications sont déjà dans User via les bindings
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
