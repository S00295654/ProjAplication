using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class User
    {
        public ObservableCollection<Game> Games { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public BitmapImage ProfileImage { get; set; }
        public User(string name) 
        {
            Name = name;
            Games = new ObservableCollection<Game>();
            Description = "You have no description.";
            ProfileImage = new BitmapImage(new Uri("Images/UserImage.png", UriKind.Relative));
        }
    }
}
