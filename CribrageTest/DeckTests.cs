using System;
using System.Collections.Generic;
using System.Text;
using cribrage;
using NUnit.Framework;

namespace CribbageTest
{
    [TestFixture]
    class DeckTests
    {
        Player p1;
        Player p2;
        Deck deck;

        [SetUp]
        public void Init()
        {
            p1 = new Player();
            p2 = new Player();
            deck = new Deck(CardType.Jack, 0, 0, 0);
        }

        [Test]
        public void NewDeckHasProperCountOfUniqueCards()
        {
            Assert.AreEqual(deck.Cards.Count, deck.Count);

            int uniqueCards = 0;
            foreach (Card c in deck.Cards)
            {
                Console.WriteLine(c.Name + " of " + c.Suit);
                for (int i = 0; i < deck.Cards.Count; i++)
                {
                    if (deck.Cards[i] == c)
                        uniqueCards++;
                }
            }

            Assert.AreEqual(uniqueCards, deck.Count);
        }

        [Test]
        public void DealToTwoPlayerEachGetSixCards()
        {
            List<Player> players = new List<Player> { p1, p2 };
            deck.Deal(players);

            Assert.AreEqual(p1.Hand.Cards.Count, 6);
            Assert.AreEqual(p2.Hand.Cards.Count, 6);
        }

        [Test]
        public void DealToTwoPlayersGivesUniqueCards()
        {
            List<Player> players = new List<Player> { p1, p2 };
            deck.Deal(players);

            int p1UniqueCards = 0;
            int p2UniqueCards = 0;

            foreach(Card c in p1.Hand.Cards)
            {
                Console.WriteLine(c.Name + " of " + c.Suit);
                for (int i = 0; i < p1.Hand.Cards.Count; i++)
                {
                    if (p1.Hand.Cards[i] == c)
                        p1UniqueCards++;
                }
            }

            Assert.AreEqual(p1UniqueCards, 6);

            foreach (Card c in p2.Hand.Cards)
            {
                Console.WriteLine(c.Name + " of " + c.Suit);

                for (int i = 0; i < p2.Hand.Cards.Count; i++)
                {
                    if (p2.Hand.Cards[i] == c)
                        p2UniqueCards++;
                }
            }

            Assert.AreEqual(p2UniqueCards, 6);
        }

        [Test]
        public void ShuffleClearsUsedCards()
        {
            List<Player> players = new List<Player> { p1, p2 };
            deck.Deal(players);
            p1.Hand.Reset();
            p2.Hand.Reset();
            deck.Shuffle();

            Assert.AreEqual(p1.Hand.Cards.Count, 0);
            Assert.AreEqual(p2.Hand.Cards.Count, 0);
            Assert.AreEqual(deck.DealtCardIndexes.Count, 0);
        }
    }
}
