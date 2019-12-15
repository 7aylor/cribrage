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
        double cutDelay = 0;
        int spaceBetweenCards;
        List<Player> players;
        Player playerToBeDealtTo;
        int playerToBeDealtToIndex;
        Button actionButton;
        Random rng;

        SpriteFont defaultFont;
        SpriteFont actionButtonFont;
        Texture2D deckTexture;
        Texture2D backOfCardTexture;
        Texture2D pixel;
        Texture2D actionButtonTexture;
        Texture2D cribStarTexture;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            rng = new Random();
            spaceBetweenCards = 5;
            deck = new Deck(CardType.Jack, spaceBetweenCards * 2, (graphics.GraphicsDevice.Viewport.Height - Card.Height) / 2, 0.1);
            p1 = new Player("Player1", graphics.GraphicsDevice.Viewport.Height - 18);
            p2 = new Player("Player2", 2);
            players = new List<Player>() { p1, p2 };
            state = Mouse.GetState();
            gameManager = new GameManager();
            playerToBeDealtTo = players[0];
            playerToBeDealtToIndex = 0;

            players[rng.Next(players.Count)].GetsCrib = true;

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
            deckTexture = Content.Load<Texture2D>("smallcards");
            backOfCardTexture = Content.Load<Texture2D>("cardbacksmall");
            actionButtonTexture = Content.Load<Texture2D>("actionbutton");
            cribStarTexture = Content.Load<Texture2D>("cribstarsmall");
            defaultFont = Content.Load<SpriteFont>("default");
            actionButtonFont = Content.Load<SpriteFont>("actionbuttonfont");

            actionButton = new Button("Deal", graphics.GraphicsDevice.Viewport.Width - actionButtonTexture.Width - 5, graphics.GraphicsDevice.Viewport.Height - actionButtonTexture.Height - 5, Color.White, actionButtonTexture);


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
                    cutDelay += gameTime.ElapsedGameTime.TotalSeconds;
                    HandleCut();
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

        private void HandleCut()
        {
            MouseState prevState = state;
            state = Mouse.GetState();

            if(!p1.GetsCrib)
            {
                if(actionButton.IsMouseHovering(state.Position.ToVector2()))
                {
                    if(state.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
                    {
                        Cut();
                    }
                }
            }
            else
            {
                if(cutDelay > 2)
                {
                    Cut();
                }
            }
        }

        private void Cut()
        {
            cut = deck.GetTopRandomCard();
            cut.DrawX = (graphics.GraphicsDevice.Viewport.Width - Card.Width) / 2;
            cut.DrawY = (graphics.GraphicsDevice.Viewport.Height - Card.Height) / 2;
            cutDelay = 0;
            Mouse.SetCursor(MouseCursor.Arrow);
            Console.WriteLine("Cut " + cut.Name);
            gameManager.GoToNextPhase();
        }

        private void HandleDiscard()
        {
            MouseState prevState = state;
            state = Mouse.GetState();
            Mouse.SetCursor(MouseCursor.Arrow);

            highlightedCard = null;

            int numSelected = p1.Hand.Cards.Where(x => x.IsSelected).Count();
            actionButton.Highlighted = actionButton.IsMouseHovering(state.Position.ToVector2());

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

            if(actionButton.Highlighted)
            {
                actionButton.Color = Color.LightGray;
                Mouse.SetCursor(MouseCursor.Hand);

                if(state.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
                {
                    List<Card> p1discards = new List<Card>();
                    List<Card> p2discards = new List<Card>();

                    foreach (Card c in p1.Hand.Cards)
                    {
                        if (c.IsSelected)
                        {
                            p1discards.Add(c);
                        }
                    }

                    //TODO: Insert some AI logic in here, for now, it's just random
                    HandleAiDiscard(p2discards);

                    if (p1discards.Count == 2)
                    {
                        p1.Hand.Cards.Remove(p1discards[0]);
                        p1.Hand.Cards.Remove(p1discards[1]);
                    }

                    foreach (Player p in players)
                    {
                        if (p.GetsCrib == true)
                        {
                            p.Crib = new Hand();
                            p.Crib.Cards.Add(p1discards[0]);
                            p.Crib.Cards.Add(p1discards[1]);
                            p.Crib.Cards.Add(p2discards[0]);
                            p.Crib.Cards.Add(p2discards[1]);

                            Console.WriteLine(p1.Name + " discarded " + p.Crib.Cards[0].Name + " to " + p.Name + "'s crib");
                            Console.WriteLine(p1.Name + " discarded " + p.Crib.Cards[1].Name + " to " + p.Name + "'s crib");
                            Console.WriteLine(p2.Name + " discarded " + p.Crib.Cards[0].Name + " to " + p.Name + "'s crib");
                            Console.WriteLine(p2.Name + " discarded " + p.Crib.Cards[1].Name + " to " + p.Name + "'s crib");
                        }

                        p.Hand.CardDrawPosX = (graphics.GraphicsDevice.Viewport.Width - ((Card.Width + spaceBetweenCards) * 4)) / 2;
                        UpdatePlayerCardPositions(p1);
                    }

                    gameManager.GoToNextPhase();
                    Mouse.SetCursor(MouseCursor.Arrow);


                }
            }
        }

        private void UpdatePlayerCardPositions(Player p)
        {
            for (int i = 0; i < p.Hand.Cards.Count; i++)
            {
                Card c = p.Hand.Cards[i];
                c.DrawX = c.DrawX = p.Hand.CardDrawPosX +
                ((Card.Width + spaceBetweenCards) * i);
            }
        }

        private void HandleAiDiscard(List<Card> aiDiscards)
        {
            aiDiscards.Add(p2.Hand.Cards[rng.Next(6)]);
            p2.Hand.Cards.Remove(aiDiscards[0]);
            aiDiscards.Add(p2.Hand.Cards[rng.Next(5)]);
            p2.Hand.Cards.Remove(aiDiscards[1]);
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

                    Console.WriteLine("Dealt " + dealtCard.Name + " to " + playerToBeDealtTo.Name);

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
                MouseState prevState = state;
                state = Mouse.GetState();
                if(actionButton.IsMouseHovering(state.Position.ToVector2()))
                {
                    if(state.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
                    {
                        deck.IsDealing = true;
                    }
                }
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

            if ((int)gameManager.State > 1 && (int)gameManager.State < 5)
                DrawCribPile();

            if (cut != null)
                DrawCutCard();

            switch (gameManager.State)
            {
                case GameState.Deal:
                    actionButton.Text = "Deal";
                    if (!deck.IsDealing)
                        DrawActionButton();
                    break;
                case GameState.Discard:
                    actionButton.Text = "Discard";
                    HandleDiscardDraw();
                    break;
                case GameState.Cut:
                    actionButton.Text = "Cut";
                    if(!p1.GetsCrib)
                        DrawActionButton();
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

        private void DrawCribPile()
        {
            Player cribPlayer = new Player() ;
            foreach(Player p in players)
            {
                if (p.GetsCrib)
                    cribPlayer = p;
            }

            if(cribPlayer.Name != "")
            {
                for(int i = 0; i < cribPlayer.Crib.Cards.Count; i++)
                {
                    spriteBatch.Draw(backOfCardTexture, new Vector2(spaceBetweenCards * (i + 1), cribPlayer.Hand.CardDrawPosY), Color.White);
                }
            }
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
                DrawActionButton();
            }
        }

        private void DrawActionButton()
        {
            Vector2 pos = new Vector2(actionButton.DrawX, actionButton.DrawY);
            Vector2 textPos = new Vector2((actionButton.DrawX + (actionButton.Texture.Width / 2) -  ((actionButtonFont.MeasureString(actionButton.Text).X) / 2)), (actionButton.DrawY + (actionButton.Texture.Height / 2) - ((actionButtonFont.MeasureString(actionButton.Text).Y) / 2)));
            spriteBatch.Draw(actionButton.Texture, pos, actionButton.Color);
            spriteBatch.DrawString(actionButtonFont, actionButton.Text, textPos, actionButton.Color);
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
            spriteBatch.Draw(deckTexture, cutDestRect, cutRect, Color.White);
        }

        private void DrawPlayerScore(Player p)
        {
            spriteBatch.DrawString(defaultFont, p.Name + " score: " + p.Hand.Score.ToString(), new Vector2((graphics.GraphicsDevice.Viewport.Width - Card.Width) / 2, p.Hand.CardDrawPosY - 20), Color.White);
        }

        private void DrawPlayerNames(Player p)
        {
            //string text = p.Name + (p.GetsCrib? " - Crib" : "");
            int drawX = (int)(graphics.GraphicsDevice.Viewport.Width - defaultFont.MeasureString(p.Name).X);

            Vector2 pos = new Vector2(drawX / 2, p.NameY);

            if(p.GetsCrib)
            {
                float scalar = defaultFont.MeasureString(p.Name).Y / cribStarTexture.Height;
                Rectangle rect = new Rectangle(0, 0, cribStarTexture.Width, cribStarTexture.Height);
                Rectangle destRect = new Rectangle((drawX / 2) - (int)(cribStarTexture.Width * scalar) - 2, p.NameY - 2, (int)(cribStarTexture.Width * scalar), (int)(cribStarTexture.Height * scalar));

                spriteBatch.Draw(cribStarTexture, destRect, rect, Color.White);
            }

            spriteBatch.DrawString(defaultFont, p.Name, pos, Color.White);
        }

        private void DrawPlayerHand(Player p)
        {
            int count = 0;
            foreach (Card card in p.Hand.Cards)
            {
                Rectangle cardRect = new Rectangle(card.SpriteX * Card.Width, card.SpriteY * Card.Height, Card.Width, Card.Height);
                Rectangle destRect = new Rectangle(card.DrawX, card.DrawY, (int)(cardRect.Width * spriteScalar), (int)(cardRect.Height * spriteScalar));
                spriteBatch.Draw(deckTexture, destRect, cardRect, Color.White);
                count++;
            }
        }

        private void DrawPlayerHandHidden(Player p)
        {
            int count = 0;
            foreach (Card card in p.Hand.Cards)
            {
                Vector2 pos = new Vector2(p.Hand.CardDrawPosX + ((Card.Width + spaceBetweenCards) * count), p.Hand.CardDrawPosY);
                spriteBatch.Draw(backOfCardTexture, pos, Color.White);
                count++;
            }
        }

        private void DrawDeck()
        {
            spriteBatch.Draw(backOfCardTexture, new Vector2(deck.DrawX, deck.DrawY), Color.White);
        }
    }
}