using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class GameData
    {
        public string Name { get; set; }
        public List<string> tags { get; set; }
        public List<string> genre { get; set; }
        public List<string> device { get; set; }
        public List<string> release { get; set; }

        public string ImagePath { get; set; }

        public double Time { get; set; }
        public double Score { get; set; }
        public GameState State { get; set; }
    }

    public class UserData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProfileImagePath { get; set; }

        public List<GameData> Games { get; set; }
    }
}
