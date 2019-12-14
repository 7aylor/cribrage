using System;
using System.Collections.Generic;
using System.Text;

namespace cribrage
{
    public class Player
    {
        public int TotalScore { get; set; } = 0;
        public string Name { get; set; }
        public Hand Hand { get; set; }

        public Player()
        {
            Hand = new Hand();
        }

        public Player(string name)
        {
            Hand = new Hand();
            Name = name;
        }

    }
}
