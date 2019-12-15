using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        Card highlightedCard;
        float spriteScalar = 1f;
        MouseState state;
        GameManager gameManager;
        double delayBetweenCards = 0;
        int spaceBetweenCards;
        List<Player> players;
        Player playerToBeDealtTo;
        int playerToBeDealtToIndex;

        SpriteFont defaultFont;
        Texture2D deckSprite;
        Texture2D backOfCard;
        Texture2D pixel;
        Texture2D discardBtn;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            spaceBetweenCards = 5;
            deck = new Deck(CardType.Jack, spaceBetweenCards * 2, (graphics.GraphicsDevice.Viewport.Height - Card.Height) / 2, 0.1);
            p1 = new Player("Player1", graphics.GraphicsDevice.Viewport.Height - 18);
            p2 = new Player("Player2", 2);
            players = new List<Player>() { p1, p2 };
            state = Mouse.GetState();
            gameManager = new GameManager();
            playerToBeDealtTo = players[0];
            playerToBeDealtToIndex = 0;

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
            discardBtn = Content.Load<Texture2D>("discardbtn");
            defaultFont = Content.Load<SpriteFont>("default");

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            delayBetweenCards += gameTime.ElapsedGameTime.TotalSeconds;

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
                    HandleDeal();
                    break;
                case GameState.Discard:
                    HandleDiscard();
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

        private void HandleDiscard()
        {
            MouseState prevState = state;
            state = Mouse.GetState();

            highlightedCard = null;

            int numSelected = p1.Hand.Cards.Where(x => x.IsSelected).Count();

            foreach (Card c in p1.Hand.Cards)
            {
                if(c.IsMouseHovering(state.Position.ToVector2()))
                {
                    //if user has clicked on hovered card
                    if(state.LeftButton == ButtonState.Pressed && prevState.LeftButton != ButtonState.Pressed)
                    {
                        if (c.IsSelected)
                            c.IsSelected = false;
                        else if (numSelected < 2)
                            c.IsSelected = true;
                    }
                    else
                    {
                        highlightedCard = c;
                    }
                }
            }
        }

        private void HandleDeal()
        {
            if (deck.IsDealing)
            {
                if (delayBetweenCards > deck.TimeBetweenCardsDealt)
                {
                    delayBetweenCards = 0;
                    Card dealtCard = deck.GetTopRandomCard();
                    playerToBeDealtTo.Hand.Cards.Add(dealtCard);

                    dealtCard.DrawX = playerToBeDealtTo.Hand.CardDrawPosX + 
                        ((Card.Width + spaceBetweenCards) *     
                            (playerToBeDealtTo.Hand.Cards.Count - 1));

                    dealtCard.DrawY = playerToBeDealtTo.Hand.CardDrawPosY;

                    if (playerToBeDealtTo == players[players.Count - 1])
                    {
                        playerToBeDealtTo = players[0];
                        playerToBeDealtToIndex = 0;
                        if (playerToBeDealtTo.Hand.Cards.Count == 6)
                        {
                            deck.IsDealing = false;
                            gameManager.GoToNextPhase();
                        }
                    }
                    else
                    {
                        playerToBeDealtToIndex++;
                        playerToBeDealtTo = players[playerToBeDealtToIndex];
                    }
                }
            }
            else
            {
                deck.IsDealing = true;
            }
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
            DrawDeck();
            DrawPlayerNames(p1);
            DrawPlayerNames(p2);
            spriteBatch.DrawString(defaultFont, "Phase: " + gameManager.State.ToString(), new Vector2(2, 2), Color.White);
            DrawPlayerHand(p1);
            DrawPlayerHandHidden(p2);


            switch (gameManager.State)
            {
                case GameState.Deal:
                    break;
                case GameState.Discard:
                    HandleDiscardDraw();
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

            //DrawCutCard(spriteBatch);
            //DrawPlayerScore(p1);
            //DrawPlayerScore(p2);


            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleDiscardDraw()
        {
            foreach (Card c in p1.Hand.Cards)
            {
                if (c.IsSelected)
                    HightlightCard(c, Color.Red, 3);
            }
            if (highlightedCard != null)
            {
                Color color;
                if (highlightedCard.IsSelected)
                    color = Color.DarkRed;
                else
                    color = Color.Cyan;
                HightlightCard(highlightedCard, color, 3);
            }
            
            if(p1.Hand.Cards.Where(x => x.IsSelected).Count() == 2)
            {
                Vector2 pos = new Vector2(graphics.GraphicsDevice.Viewport.Width - discardBtn.Width - 5, graphics.GraphicsDevice.Viewport.Height - discardBtn.Height - 5);
                spriteBatch.Draw(discardBtn, pos, Color.White);
            }
        }

        private void HightlightCard(Card card, Color color, int lineWidth)
        {
            Rectangle top = new Rectangle(card.DrawX, card.DrawY, Card.Width, lineWidth);
            Rectangle right = new Rectangle(card.DrawX + Card.Width - lineWidth, card.DrawY, lineWidth, Card.Height);
            Rectangle bottom = new Rectangle(card.DrawX, card.DrawY + Card.Height - lineWidth, Card.Width, lineWidth);
            Rectangle left = new Rectangle(card.DrawX, card.DrawY, lineWidth, Card.Height);

            spriteBatch.Draw(pixel, top, color);
            spriteBatch.Draw(pixel, right, color);
            spriteBatch.Draw(pixel, bottom, color);
            spriteBatch.Draw(pixel, left, color);

        }

        private void DrawCutCard()
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

        private void DrawPlayerHand(Player p)
        {
            int count = 0;
            foreach (Card card in p.Hand.Cards)
            {
                Rectangle cardRect = new Rectangle(card.SpriteX * Card.Width, card.SpriteY * Card.Height, Card.Width, Card.Height);
                Rectangle destRect = new Rectangle(card.DrawX, card.DrawY, (int)(cardRect.Width * spriteScalar), (int)(cardRect.Height * spriteScalar));
                spriteBatch.Draw(deckSprite, destRect, cardRect, Color.White);
                count++;
            }
        }

        private void DrawPlayerHandHidden(Player p)
        {
            int count = 0;
            foreach (Card card in p.Hand.Cards)
            {
                Vector2 pos = new Vector2(p.Hand.CardDrawPosX + ((Card.Width + spaceBetweenCards) * count), p.Hand.CardDrawPosY);
                spriteBatch.Draw(backOfCard, pos, Color.White);
                count++;
            }
        }

        private void DrawDeck()
        {
            spriteBatch.Draw(backOfCard, new Vector2(deck.DrawX, deck.DrawY), Color.White);
        }
    }
}