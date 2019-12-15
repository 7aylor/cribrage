using System;
using System.Collections.Generic;
using System.Text;

namespace cribrage
{
    public enum GameState { Deal, Discard, Cut, Pegging, Counting, CountingCrib, GameOver }

    public class GameManager
    {
        public GameState State { get; set; }
        public bool PhaseDone { get; set; }
        public double WaitTime { get; set; }

        public GameManager(double wt=3)
        {
            State = GameState.Deal;
            WaitTime = wt;
        }


        public void GoToNextPhase()
        {
            State = State == GameState.CountingCrib? GameState.Deal : State + 1;
            PhaseDone = false;
        }
        
    }
}
