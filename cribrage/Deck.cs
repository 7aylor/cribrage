using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cribrage
{
    public class Deck
    {
        public int Count { get; set; } = 52;
        public List<Card> Cards { get; set; }
        public List<int> DealtCardIndexes { get; set; }
        public int DrawX { get; set; }
        public int DrawY { get; set; }
        public bool IsDealing { get; set; }
        public double TimeBetweenCardsDealt { get; set; }

        public Deck(CardType nobsCard, int drawX, int drawY, double time)
        {
            Cards = new List<Card>();
            DealtCardIndexes = new List<int>();
            DrawX = drawX;
            DrawY = drawY;
            TimeBetweenCardsDealt = time;

            int x = 0;
            int y = 0;

            foreach(Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach(CardType card in Enum.GetValues(typeof(CardType)))
                {
                    bool isNobs = card == nobsCard? true : false;
                    Card c = new Card(suit, card, isNobs, x, y);
                    Cards.Add(c);
                    x++;
                    if(x > 12)
                    {
                        x = 0;
                    }
                }
                y++;
            }
        }

        /// <summary>
        /// Gets a random card from the deck that hasn't been used
        /// </summary>
        /// <returns>Random Card</returns>
        public Card GetTopRandomCard()
        {
            Random rand = new Random();
            int index = -1;

            do
            {
                index = rand.Next(Count);
            } while (DealtCardIndexes.Contains(index));

            DealtCardIndexes.Add(index);

            return Cards[index];
        }

        /// <summary>
        /// Deals 6 cards to each player
        /// </summary>
        /// <param name="players">List of players</param>
        public void Deal(List<Player> players, int count=6)
        {
            //TODO: if more than 2 players, this will need to be changed
            for (int i = 0; i < count; i++)
            {
                foreach(Player p in players)
                {
                    p.Hand.Cards.Add(GetTopRandomCard());
                }
            }
        }

        /// <summary>
        /// Really just clears the used cards so that when GetTopRandomCard 
        /// is called, it has the full deck to look through
        /// </summary>
        public void Shuffle()
        {
            foreach(Card c in Cards)
            {
                c.IsCutCard = false;
            }
            DealtCardIndexes.Clear();
        }

        public Card GetCard(CardType type, Suit suit)
        {
            return Cards.Find(c => c.Type == type && c.Suit == suit);
        }
    }
}
