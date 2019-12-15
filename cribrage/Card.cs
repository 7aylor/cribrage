using Microsoft.Xna.Framework;
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
        public int Ordinal { get; set; } //value in suit (for runs)
        public bool GivesNobs { get; set; } = false;
        public int SpriteX { get; set; } //position x in the sprite map
        public int SpriteY { get; set; } //position y in the sprite map
        public int DrawX { get; set; }
        public int DrawY { get; set; }
        public bool CanBePlayed { get; set; } //for pegging
        public bool IsCutCard { get; set; } = false;
        public bool IsSelected { get; set; }

        public static int Width = 40;
        public static int Height = 60;

        public Card() { }

        public Card(Suit suit, CardType type)
        {
            this.Name = type.ToString();
            this.Type = type;
            this.Suit = suit;
            SetCardValue(type);
            SetCardOrdinal(type);
        }

        public Card(Suit suit, CardType type, bool givesNobs, int x, int y)
        {
            this.Type = type;
            this.Suit = suit;
            this.Name = type.ToString() + " of " + suit.ToString();
            SetCardValue(type);
            SetCardOrdinal(type);
            this.GivesNobs = givesNobs;
            SpriteX = x;
            SpriteY = y;
        }

        private void SetCardValue(CardType type)
        {
            Value = (int)type < 10 ? (int)type + 1 : 10;
        }

        private void SetCardOrdinal(CardType type)
        {
            Ordinal = (int)type;
        }

        public bool IsMouseHovering(Vector2 mPos)
        {
            if(mPos.X >= DrawX &&
               mPos.X <= DrawX + Width &&
               mPos.Y >= DrawY &&
               mPos.Y <= DrawY + Height)
            {
                return true;
            }
            return false;
        }
    }
}
