using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjAplication
{
    public class Library
    {
        public Dictionary<string, Game> Games;
        public Library() 
        {
            Games = new Dictionary<string, Game>();
        }

        public void AddGame(Game game)
        {
            Games[game.Name] = game;
        }
    }
}
