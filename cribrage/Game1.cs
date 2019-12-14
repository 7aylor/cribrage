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
        Player p1;
        Player p2;
        List<Player> players;


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
            p1 = new Player();
            p2 = new Player();
            players = new List<Player>() { p1, p2 };

            p1.Hand.CardDrawPosX = 275;
            p1.Hand.CardDrawPosY = 400;
            p2.Hand.CardDrawPosX = 275;
            p2.Hand.CardDrawPosY = 20;
            deck.Deal(players);

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

            //draw all cards
            //foreach(var card in deck.Cards)
            //{
            //    Rectangle cardRect = new Rectangle(card.SpriteX * Card.Width, card.SpriteY * Card.Height, Card.Width, Card.Height);

            //    float scalar = 1f;

            //    Rectangle destRect = new Rectangle((int)(cardRect.X * scalar) + (5 * card.SpriteX) + 5, (int)(cardRect.Y * scalar) + (5 * card.SpriteY) + 5, (int)(cardRect.Width * scalar), (int)(cardRect.Height * scalar));

            //    //spriteBatch.Draw(deckSprite, new Vector2(card.SpriteX * Card.Width, card.SpriteY * Card.Height), new Rectangle(card.SpriteX * Card.Width, card.SpriteY * Card.Height, Card.Width, Card.Height), Color.White);
            //    spriteBatch.Draw(deckSprite, destRect, cardRect, Color.White);
            //}

            int count = 0;
            foreach(Card card in p1.Hand.Cards)
            {
                Rectangle cardRect = new Rectangle(card.SpriteX * Card.Width, card.SpriteY * Card.Height, Card.Width, Card.Height);

                float scalar = 1f;

                Rectangle destRect = new Rectangle(p1.Hand.CardDrawPosX + (Card.Width * count) + 5, p1.Hand.CardDrawPosY, (int)(cardRect.Width * scalar), (int)(cardRect.Height * scalar));

                spriteBatch.Draw(deckSprite, destRect, cardRect, Color.White);
                count++;
            }

            count = 0;
            foreach (Card card in p2.Hand.Cards)
            {
                Rectangle cardRect = new Rectangle(card.SpriteX * Card.Width, card.SpriteY * Card.Height, Card.Width, Card.Height);

                float scalar = 1f;

                Rectangle destRect = new Rectangle(p2.Hand.CardDrawPosX + (Card.Width * count) + 5, p2.Hand.CardDrawPosY, (int)(cardRect.Width * scalar), (int)(cardRect.Height * scalar));

                spriteBatch.Draw(deckSprite, destRect, cardRect, Color.White);
                count++;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}