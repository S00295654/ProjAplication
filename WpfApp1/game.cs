using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class Game
    {
        public string Name { get; set; }
        public List<string> tags { get; set; }
        public List<string> genre { get; set; }
        public List<string> device { get; set; }
        public List<string> release { get; set; }
        public BitmapImage Illustration { get; set; }
        private double time;
        public double Time
        {
            get => time;
            set { time = value; OnPropertyChanged(); }
        }

        private double score;
        public double Score
        {
            get => score;
            set { score = value; OnPropertyChanged(); }
        }

        private string state;
        public string State
        {
            get => state;
            set { state = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public class Game1
    {
        public string Name;
        public int release;
        public List<string> tags;
        public List<string> genre;
        public List<string> device;
        public int Time;

        public Game1(string name, int release, List<string> tags, List<string> device, List<string> genre, int time)
        {
            Name = name;
            this.release = release;
            this.tags = tags;
            this.device = device;
            this.genre = genre;
            if (time < 0)
                this.Time = -time;
            else 
                this.Time = time;
        }
    }
}
