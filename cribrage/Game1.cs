using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace cribrage
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Deck deck;

        //SpriteFont defaultFont;
        Texture2D deckSprite;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            deck = new Deck(CardType.Jack);
            foreach (Card card in deck.Cards)
            {
                Debug.WriteLine(card.Name + " of " + card.Suit.ToString() + " : value - " + card.Value + " (" + card.SpriteX + ", " + card.SpriteY + ")");
            }

            Hand hand = new Hand();

            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.King),
                new Card(Suit.Hearts, CardType.King),
                new Card(Suit.Spades, CardType.Two),
                new Card(Suit.Spades, CardType.Ten),
                new Card(Suit.Diamonds, CardType.Seven)
            };

            hand.BuildSets();

            hand.GetRuns();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            deckSprite = Content.Load<Texture2D>("smallcards");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);

            spriteBatch.Begin();

            foreach(var card in deck.Cards)
            {
                Rectangle cardRect = new Rectangle(card.SpriteX * Card.Width, card.SpriteY * Card.Height, Card.Width, Card.Height);

                float scalar = 1f;

                Rectangle destRect = new Rectangle((int)(cardRect.X * scalar) + (5 * card.SpriteX) + 5, (int)(cardRect.Y * scalar) + (5 * card.SpriteY) + 5, (int)(cardRect.Width * scalar), (int)(cardRect.Height * scalar));

                //spriteBatch.Draw(deckSprite, new Vector2(card.SpriteX * Card.Width, card.SpriteY * Card.Height), new Rectangle(card.SpriteX * Card.Width, card.SpriteY * Card.Height, Card.Width, Card.Height), Color.White);
                spriteBatch.Draw(deckSprite, destRect, cardRect, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}