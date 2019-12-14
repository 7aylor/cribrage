using System;
using System.Collections.Generic;
using System.Text;

namespace cribrage
{
    class PeggingManager
    {
        public int Score { get; set; } = 0;
        public int MaxCount { get; set; } = 31;
        public List<Card> PlayedCards { get; set; }
        public Player TurnPlayer { get; set; } //The player who has the current turn
        public Player PrevTurnPlayer { get; set; }

        public PeggingManager()
        {
            PlayedCards = new List<Card>();
        }

        public void PlayCard(Card card)
        {
            if(Score + card.Value <= MaxCount)
            {
                PlayedCards.Add(card);
                Score += card.Value;
                if (Score == 15)
                    TurnPlayer.TotalScore += 2;
                else if (Score == 31)
                    TurnPlayer.TotalScore += 2;
            }
            else
            {
                //go
                //start over
                //change game phase to counting
            }
        }

        //add card to playedCards
        //handle fifteen
        //handle 31
        //handle goes
        //handle pairs
        //handle runs
        //end pegging phase
    }
}
