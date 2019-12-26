using System;
using System.Collections.Generic;
using System.Text;
using cribrage;
using NUnit.Framework;

namespace CribbageTest
{
    [TestFixture]
    class PeggingTests
    {
        PeggingManager peggingManager;
        [SetUp]
        public void Init()
        {
            peggingManager = new PeggingManager(0, 0);
        }

        [Test]
        public void Fifteen_scoredCorrectly()
        {
            Player p = new Player("Test Player", 0, 0);
            peggingManager.TurnPlayer = p;
            peggingManager.PlayCard(new Card(Suit.Clubs, CardType.Eight));
            peggingManager.PlayCard(new Card(Suit.Hearts, CardType.Seven));

            Assert.AreEqual(p.TotalScore, 2);
        }

        [Test]
        public void ThiryOne_scoredCorrectly()
        {
            Player p = new Player("Test Player", 0, 0);
            peggingManager.TurnPlayer = p;
            peggingManager.PlayCard(new Card(Suit.Clubs, CardType.Ten));
            peggingManager.PlayCard(new Card(Suit.Clubs, CardType.King));
            peggingManager.PlayCard(new Card(Suit.Diamonds, CardType.Ten));

            Player p1 = new Player("Test Player1", 0, 0);
            peggingManager.PrevTurnPlayer = p1;

            peggingManager.PlayCard(new Card(Suit.Hearts, CardType.Ace));

            Assert.AreEqual(p.TotalScore, 2);
        }

        [Test]
        public void Pairs_scoredCorrectly()
        {
            Player p = new Player("Test Player", 0, 0);
            peggingManager.PlayedCards.Add(new Card(Suit.Clubs, CardType.Eight));
            peggingManager.TurnPlayer = p;
            peggingManager.PlayCard(new Card(Suit.Hearts, CardType.Eight));

            Assert.AreEqual(p.TotalScore, 2);
        }

        [Test]
        public void Trips_scoredCorrectly()
        {
            Player p = new Player("Test Player", 0, 0);
            peggingManager.PlayedCards.Add(new Card(Suit.Clubs, CardType.Seven));
            peggingManager.PlayedCards.Add(new Card(Suit.Diamonds, CardType.Seven));
            peggingManager.TurnPlayer = p;
            peggingManager.PlayCard(new Card(Suit.Hearts, CardType.Seven));

            Assert.AreEqual(p.TotalScore, 6);
        }

        [Test]
        public void Quads_scoredCorrectly()
        {
            Player p = new Player("Test Player", 0, 0);
            peggingManager.PlayedCards.Add(new Card(Suit.Clubs, CardType.Three));
            peggingManager.PlayedCards.Add(new Card(Suit.Diamonds, CardType.Three));
            peggingManager.PlayedCards.Add(new Card(Suit.Spades, CardType.Three));
            peggingManager.TurnPlayer = p;
            peggingManager.PlayCard(new Card(Suit.Hearts, CardType.Three));

            Assert.AreEqual(p.TotalScore, 12);
        }

    }
}
