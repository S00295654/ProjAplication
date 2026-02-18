using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class Game
    {
        public ICommand RemoveCommand { get; set; }
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

        private GameState state;
        public GameState State
        {
            get => state;
            set
            {
                state = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    
}
