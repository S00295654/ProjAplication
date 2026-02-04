using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjAplication
{
    public class User
    {
        public List<Game> Games;
        public string Name;
        public string Description;
        public User(string name) 
        {
            Name = name;
            Games = new List<Game>();
            Description = "You have no description.";
        }
    }
}
