using System;
using System.Collections.Generic;
using System.Text;

namespace cribrage
{
    public enum GameState { None, Deal, Discard, Cut, Pegging, Counting }

    public class GameManager
    {
        public GameState State { get; set; }

        public GameManager()
        {
            State = GameState.None;
        }
        
    }
}
