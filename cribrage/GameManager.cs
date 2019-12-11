using System;
using System.Collections.Generic;
using System.Text;

namespace cribrage
{
    public enum GameState { None, Deal, Discard, Cut, Pegging, Counting }
    class GameManager
    {
        public GameState State { get; set; }

        public Card Cut { get; set; }
    }
}
