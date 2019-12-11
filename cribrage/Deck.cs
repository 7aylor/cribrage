using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
            foreach(Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach(CardType card in Enum.GetValues(typeof(CardType)))
                {
                    bool isNobs = card == nobsCard? true : false;
                    Card c = new Card(suit, card, isNobs);
                    Cards.Add(c);
                }
            }
        }

    }

    public class Card
    {
        public string Name { get; set; }
        public CardType Type { get; set; }
        public Suit Suit { get; set; }
        public int Value { get; set; }
        public bool GivesNobs { get; set; } = false;
        public Texture2D Texture { get; set; }

        public Card(Suit suit, CardType type, bool givesNobs)
        {
            this.Name = type.ToString();
            this.Type = type;
            this.Suit = suit;
            Value = (int)type < 10 ? (int)type + 1 : (int)type;
            this.GivesNobs = givesNobs;
        }
    }

    public enum Suit { Spades, Clubs, Hearts, Diamonds }
    public enum CardType { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };
}
