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

        public double TotalTime()
        {
            if (Games == null)
                return 0;
            double res = 0;
            foreach (Game game in Games)
                res += game.Time;
            return res;
        }

        public string TimeInDay()
        {
            double t=TotalTime();
            int day = 0;
            int hour = 0;
            while (t>24)
            {
                day++;
                t = t - 24;
            }
            hour = (int)t;
            if (day>1)
            {
                return $"You have {day} days of gamming and {hour} of gamming";
            }
            if (day==1)
                return $"You have {day} day of gamming and {hour} of gamming";
            return $"You have {hour} of gamming in total";
        }
    }
}
