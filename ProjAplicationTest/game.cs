using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input.StylusPlugIns;

namespace ProjAplication
{
    public class Game
    {
        public string Name;
        public int release;
        public List<string> tags;
        public List<string> genre;
        public List<string> device;
        public int Time;

        public Game(string name, int release, List<string> tags, List<string> device, List<string> genre, int time)
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
