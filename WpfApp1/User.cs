using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class User
    {
        public List<Game1> Games;
        public string Name { get; set; }
        public string Description;
        public User(string name) 
        {
            Name = name;
            Games = new List<Game1>();
            Description = "You have no description.";
        }
    }
}
