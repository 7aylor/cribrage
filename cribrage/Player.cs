﻿using System;
using System.Collections.Generic;
using System.Text;

namespace cribrage
{
    public class Player
    {
        public int TotalScore { get; set; } = 0;
        public string Name { get; set; }
        public Hand Hand { get; set; }
        public Hand Crib { get; set; }
        public int NameY { get; set; } //position of name to be drawn to screen
        public bool GetsCrib { get; set; } = false;
        public int AlertY { get; set; }
        public string Alert { get; set; } = "";

        public Player()
        {
            Hand = new Hand();
        }

        public Player(string name, int y, int aY)
        {
            Hand = new Hand();
            Name = name;
            NameY = y;
            AlertY = aY;
        }

    }
}
