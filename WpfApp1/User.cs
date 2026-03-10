using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class User : INotifyPropertyChanged
    {
        public ObservableCollection<Game> Games { get; set; }
        private string name { get; set; }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        private string description { get; set; }
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }



        public string Time {
            get 
            {
                return TimeInDay();
            }
        }
        public Game MostPlayed
        {
            get
            {
                return MostTime();
            }
        }

        private BitmapImage profileImage;
        public BitmapImage ProfileImage
        {
            get => profileImage;
            set
            {
                profileImage = value;
                OnPropertyChanged();
            }
        }
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
                return $"You have {day} days of gaming and {hour} of gaming";
            }
            if (day==1)
                return $"You have {day} days and {hour}h of gaming";
            return $"You have {hour} of gaming in total";
        }
        public Game MostTime()
        {
            if (Games.Count == 0) 
                return null;

            Game res = Games[0];
            foreach (Game game in Games)
            {
                if (game.Time > res.Time)
                    res = game;
            }
            return res;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
