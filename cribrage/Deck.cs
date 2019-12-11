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

        public Deck(CardType nobsCard)
        {
            Cards = new List<Card>();

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
    }
}
