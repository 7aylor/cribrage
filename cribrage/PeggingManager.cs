using System;
using System.Collections.Generic;
using System.Text;

namespace cribrage
{
    public class PeggingManager
    {
        public int Count { get; set; } = 0;
        public int MaxCount { get; set; } = 31;
        public List<Card> PlayedCards { get; set; }
        public List<Card> PlayedRoundCards { get; set; }

        public Player TurnPlayer { get; set; } //The player who has the current turn
        public Player PrevTurnPlayer { get; set; }
        public int DrawX { get; set; }
        public int DrawY { get; set; }
        public int PeggingRound { get; set; }
        public bool IsGo { get; set; } = false;

        public PeggingManager(int x, int y)
        {
            PlayedCards = new List<Card>();
            DrawX = x;
            DrawY = y;
            PeggingRound = 1;
        }

        public void PlayCard(Card card)
        {
            card.PeggingRound = PeggingRound;

            TurnPlayer.Alert = "";
            if(PrevTurnPlayer != null)
                PrevTurnPlayer.Alert = "";

            Count += card.Value;
            if (Count == 15)
            {
                TurnPlayer.TotalScore += 2;
                TurnPlayer.Alert = "Fifteen for 2";
                Console.WriteLine(TurnPlayer.Name + ": Fifteen for 2");
            }
            else if (Count == 31)
            {
                TurnPlayer.TotalScore += 2;
                TurnPlayer.Alert = "Thirty One for 2";
                Console.WriteLine(TurnPlayer.Name + ": Thirty-One for 2");
                EndOfRound();
            }

            //TODO: Fix bug where scores happen between rounds (ie I finished a round with an 8 and the next card is an 8 in the following round, that shouldn't give you 2 points)
            if (PlayedCards.Count > 2 && 
                card.Ordinal == PlayedCards[PlayedCards.Count - 3].Ordinal &&
                card.Ordinal == PlayedCards[PlayedCards.Count - 2].Ordinal &&
                card.Ordinal == PlayedCards[PlayedCards.Count - 1].Ordinal)
            {
                TurnPlayer.TotalScore += 12;
                TurnPlayer.Alert = TurnPlayer.Alert == ""? "Quads for 12!" : ", Quads for 12!";
                Console.WriteLine(TurnPlayer.Name + ": Quads for 12");
            }
            else if(PlayedCards.Count > 1 &&
                card.Ordinal == PlayedCards[PlayedCards.Count - 2].Ordinal &&
                card.Ordinal == PlayedCards[PlayedCards.Count - 1].Ordinal)
            {
                TurnPlayer.TotalScore += 6;
                TurnPlayer.Alert = TurnPlayer.Alert == "" ? "Trips for 6!" : ", Trips for 6!";
                Console.WriteLine(TurnPlayer.Name + ": Trips for 6");
            }
            else if (PlayedCards.Count > 0 &&
                card.Ordinal == PlayedCards[PlayedCards.Count - 1].Ordinal)
            {
                TurnPlayer.TotalScore += 2;
                TurnPlayer.Alert = TurnPlayer.Alert == "" ? "Pair for 2!" : ", Pair for 2!";
                Console.WriteLine(TurnPlayer.Name + ": Pair for 2");
            }

            //TODO:runs
            if (PlayedCards.Count > 2)
            {

            }

            Console.WriteLine(TurnPlayer.Name + " played " + card.Name + " - Count: " + Count);
            PlayedCards.Add(card);

            if (PlayedCards.Count == 8 && Count != 31)
            {
                TurnPlayer.TotalScore += 1;
                TurnPlayer.Alert = "Last card for 1";
                Console.WriteLine(TurnPlayer.Name + ": Last card for 1");
                EndOfRound();
            }
            //TODO: Win scenario
            //if(PrevTurnPlayer.TotalScore > 120)

        }

        public bool CheckPlayerCanPlay(Player p)
        {
            bool canPlay = false;
            foreach(Card c in p.Hand.Cards)
            {
                if(!c.WasPlayed)
                {
                    if(c.Value + Count <= 31)
                    {
                        canPlay =  true;
                    }
                    else
                    {
                        c.CanBePlayed = false;
                    }
                }
            }

            return canPlay;
        }

        public void Go(Player p)
        {
            p.TotalScore += 1;
            Console.WriteLine("Go for 1 to " + p.Name);
            p.Alert = "Go for 1";
            EndOfRound();
        }

        public void SwapPlayers()
        {
            Player temp = PrevTurnPlayer;
            PrevTurnPlayer = TurnPlayer;
            TurnPlayer = temp;
        }

        public void EndOfRound()
        {
            PeggingRound++;

            if(PlayedCards.Count < 8)
                Count = 0;

            foreach (Card c in TurnPlayer.Hand.Cards)
            {
                if (!c.WasPlayed)
                    c.CanBePlayed = true;
            }
            foreach (Card c in PrevTurnPlayer.Hand.Cards)
            {
                if (!c.WasPlayed)
                    c.CanBePlayed = true;
            }
        }

        public void Reset()
        {
            foreach(Card c in PlayedCards)
            {
                c.PeggingRound = 0;
                c.CanBePlayed = true;
                c.WasPlayed = false;
                Count = 0;
            }

            TurnPlayer.Alert = "";
            PrevTurnPlayer.Alert = "";
            SwapPlayers();
            PlayedCards.Clear();
            PeggingRound = 1;
        }
    }
}
