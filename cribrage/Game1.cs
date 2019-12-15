using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        Card cut;
        float spriteScalar = 1f;
        MouseState state;
        GameManager gameManager;
        double delay = 0f;
        int spaceBetweenCards;
        List<Player> players;

        SpriteFont defaultFont;
        Texture2D deckSprite;
        Texture2D backOfCard;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            spaceBetweenCards = 5;
            deck = new Deck(CardType.Jack, spaceBetweenCards * 2, (graphics.GraphicsDevice.Viewport.Height - Card.Height) / 2);
            p1 = new Player("Player1", graphics.GraphicsDevice.Viewport.Height - 18);
            p2 = new Player("Player2", 2);
            players = new List<Player>() { p1, p2 };
            state = Mouse.GetState();
            gameManager = new GameManager();

            p1.Hand.CardDrawPosX = (graphics.GraphicsDevice.Viewport.Width - ((Card.Width + spaceBetweenCards) * 6)) / 2;
            p1.Hand.CardDrawPosY = 400;
            p2.Hand.CardDrawPosX = (graphics.GraphicsDevice.Viewport.Width - ((Card.Width + spaceBetweenCards) * 6)) / 2; ;
            p2.Hand.CardDrawPosY = 20;

            #region testHands
            //deck.Deal(players, 4);

            //cut = deck.GetCard(CardType.Jack, Suit.Clubs);

            //cut.DrawX = (graphics.GraphicsDevice.Viewport.Width - Card.Width) / 2;
            //cut.DrawY = (graphics.GraphicsDevice.Viewport.Height - Card.Height) / 2;

            //p1.Hand.GetScore(cut);
            //p2.Hand.GetScore(cut);

            //p1.TotalScore += p1.Hand.Score;
            //p2.TotalScore += p2.Hand.Score;
            #endregion
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            deckSprite = Content.Load<Texture2D>("smallcards");
            backOfCard = Content.Load<Texture2D>("cardbacksmall");
            defaultFont = Content.Load<SpriteFont>("default");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            delay += gameTime.ElapsedGameTime.TotalSeconds;

            #region testGenerateNewHandOnClick
            //MouseState prevState = state;
            //state = Mouse.GetState();

            //if (state.LeftButton == ButtonState.Pressed && prevState.LeftButton != ButtonState.Pressed)
            //{
            //    p1.Hand.Reset();
            //    p2.Hand.Reset();
            //    deck.Shuffle();
            //    deck.Deal(players, 4);
            //    cut = deck.GetTopRandomCard();
            //    cut.DrawX = (graphics.GraphicsDevice.Viewport.Width - Card.Width) / 2;
            //    cut.DrawY = (graphics.GraphicsDevice.Viewport.Height - Card.Height) / 2;
            //    p1.Hand.GetScore(cut);
            //    p2.Hand.GetScore(cut);
            //}
            #endregion


            switch (gameManager.State)
            {
                case GameState.Deal:
                    if(!gameManager.PhaseDone)
                    {
                        //TODO: refactor to show one card at a time
                        deck.Deal(players);
                        gameManager.PhaseDone = true;
                    }
                    else
                    {
                        if (delay > gameManager.WaitTime)
                        {
                            delay = 0;
                            gameManager.GoToNextPhase();
                        }
                    }
                    break;
                case GameState.Discard:
                    if (!gameManager.PhaseDone)
                    {

                    }
                    break;
                case GameState.Cut:

                    break;
                case GameState.Pegging:

                    break;
                case GameState.Counting:

                    break;
                case GameState.CountingCrib:

                    break;
                default:

                    break;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);

            spriteBatch.Begin();

            #region draw all cards
            //foreach(var card in deck.Cards)
            //{
            //    Rectangle cardRect = new Rectangle(card.SpriteX * Card.Width, card.SpriteY * Card.Height, Card.Width, Card.Height);

            //    float scalar = 1f;

            //    Rectangle destRect = new Rectangle((int)(cardRect.X * scalar) + (5 * card.SpriteX) + 5, (int)(cardRect.Y * scalar) + (5 * card.SpriteY) + 5, (int)(cardRect.Width * scalar), (int)(cardRect.Height * scalar));

            //    //spriteBatch.Draw(deckSprite, new Vector2(card.SpriteX * Card.Width, card.SpriteY * Card.Height), new Rectangle(card.SpriteX * Card.Width, card.SpriteY * Card.Height, Card.Width, Card.Height), Color.White);
            //    spriteBatch.Draw(deckSprite, destRect, cardRect, Color.White);
            //}
            #endregion
            DrawDeck(spriteBatch);
            DrawPlayerNames(p1);
            DrawPlayerNames(p2);
            spriteBatch.DrawString(defaultFont, "Phase: " + gameManager.State.ToString(), new Vector2(2, 2), Color.White);
            DrawPlayerHand(p1, spriteBatch);
            DrawPlayerHand(p2, spriteBatch);


            //DrawCutCard(spriteBatch);
            //DrawPlayerScore(p1);
            //DrawPlayerScore(p2);


            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawCutCard(SpriteBatch spriteBatch)
        {
            Rectangle cutRect = new Rectangle(cut.SpriteX * Card.Width, cut.SpriteY * Card.Height, Card.Width, Card.Height);
            Rectangle cutDestRect = new Rectangle(cut.DrawX, cut.DrawY, (int)(cutRect.Width * spriteScalar), (int)(cutRect.Height * spriteScalar));
            spriteBatch.Draw(deckSprite, cutDestRect, cutRect, Color.White);
        }

        private void DrawPlayerScore(Player p)
        {
            spriteBatch.DrawString(defaultFont, p.Name + " score: " + p.Hand.Score.ToString(), new Vector2((graphics.GraphicsDevice.Viewport.Width - Card.Width) / 2, p.Hand.CardDrawPosY - 20), Color.White);
        }

        private void DrawPlayerNames(Player p)
        {
            spriteBatch.DrawString(defaultFont, p.Name, new Vector2((graphics.GraphicsDevice.Viewport.Width - Card.Width) / 2, p.NameY), Color.White);
        }

        private void DrawPlayerHand(Player p, SpriteBatch spriteBatch)
        {
            int count = 0;
            foreach (Card card in p.Hand.Cards)
            {
                Rectangle cardRect = new Rectangle(card.SpriteX * Card.Width, card.SpriteY * Card.Height, Card.Width, Card.Height);
                Rectangle destRect = new Rectangle(p.Hand.CardDrawPosX + ((Card.Width + spaceBetweenCards) * count), p.Hand.CardDrawPosY, (int)(cardRect.Width * spriteScalar), (int)(cardRect.Height * spriteScalar));
                spriteBatch.Draw(deckSprite, destRect, cardRect, Color.White);
                count++;
            }
        }

        private void DrawDeck(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backOfCard, new Vector2(deck.DrawX, deck.DrawY), Color.White);
        }
    }
}