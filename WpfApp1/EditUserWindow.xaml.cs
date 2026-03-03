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
                    // Dossier cible
                    string saveFolder = IOPath.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Save");

                    // Crée le dossier si nécessaire
                    if (!Directory.Exists(saveFolder))
                        Directory.CreateDirectory(saveFolder);

                    // Nouveau nom unique pour éviter les conflits
                    string extension = IOPath.GetExtension(ofd.FileName);
                    string newFileName = $"profile_{Guid.NewGuid()}{extension}";
                    string destinationPath = IOPath.Combine(saveFolder, newFileName);

                    // Copie l'image
                    File.Copy(ofd.FileName, destinationPath, true);

                    // Chemin relatif pour sauvegarde JSON
                    string relativePath = IOPath.Combine("Images", "Save", newFileName);

                    // Mise à jour du profil
                    User.ProfileImage = new BitmapImage(
                        new Uri(relativePath, UriKind.Relative)
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la copie de l'image : " + ex.Message);
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
