using System;
using System.Collections.Generic;
using System.Text;

namespace cribrage
{
    public enum Suit { Clubs, Hearts, Spades, Diamonds }
    public enum CardType { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };

    public class Card
    {
        public string Name { get; set; }
        public CardType Type { get; set; }
        public Suit Suit { get; set; }
        public int Value { get; set; }
        public bool GivesNobs { get; set; } = false;
        public int SpriteX { get; set; }
        public int SpriteY { get; set; }

        public static int Width = 40;
        public static int Height = 60;

        public Card(Suit suit, CardType type, bool givesNobs, int x, int y)
        {
            this.Name = type.ToString();
            this.Type = type;
            this.Suit = suit;
            Value = (int)type < 10 ? (int)type + 1 : 10;
            this.GivesNobs = givesNobs;
            SpriteX = x;
            SpriteY = y;
        }
    }
}
