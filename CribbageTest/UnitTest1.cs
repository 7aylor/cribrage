using cribrage;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        Deck deck;
        Hand hand;
        [SetUp]
        public void Setup()
        {
            deck = new Deck(CardType.Jack);
            hand = new Hand();
        }

        [Test]
        public void SetsOfTwoCardsAreUnique()
        {
            hand.Cards.Add(deck.Cards[0]);
            hand.Cards.Add(deck.Cards[1]);
            hand.Cards.Add(deck.Cards[2]);
            hand.Cards.Add(deck.Cards[3]);
            hand.Cards.Add(deck.Cards[4]);

            hand.BuildSets();

            foreach (var set in hand.setOfTwos)
            {
                int count = 0;
                for(int i = 0; i < hand.setOfTwos.Length; i++)
                {
                    if (set == hand.setOfTwos[i])
                        count++;
                }
                Assert.AreEqual(count, 1);
            }
        }

        [Test]
        public void SetsOfThreesCardsAreUnique()
        {
            hand.Cards.Add(deck.Cards[0]);
            hand.Cards.Add(deck.Cards[1]);
            hand.Cards.Add(deck.Cards[2]);
            hand.Cards.Add(deck.Cards[3]);
            hand.Cards.Add(deck.Cards[4]);

            hand.BuildSets();

            foreach (var set in hand.setOfThrees)
            {
                int count = 0;
                for (int i = 0; i < hand.setOfThrees.Length; i++)
                {
                    if (set == hand.setOfThrees[i])
                        count++;
                }
                Assert.AreEqual(count, 1);
            }
        }

        [Test]
        public void SetsOfFoursCardsAreUnique()
        {
            hand.Cards.Add(deck.Cards[0]);
            hand.Cards.Add(deck.Cards[1]);
            hand.Cards.Add(deck.Cards[2]);
            hand.Cards.Add(deck.Cards[3]);
            hand.Cards.Add(deck.Cards[4]);

            hand.BuildSets();

            foreach (var set in hand.setOfFours)
            {
                int count = 0;
                for (int i = 0; i < hand.setOfFours.Length; i++)
                {
                    if (set == hand.setOfFours[i])
                        count++;
                }
                Assert.AreEqual(count, 1);
            }
        }

        [Test]
        public void KingQueenJackTenFive_CorrectFifteenScore()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.King),
                new Card(Suit.Hearts, CardType.Queen),
                new Card(Suit.Spades, CardType.Jack),
                new Card(Suit.Spades, CardType.Ten),
                new Card(Suit.Diamonds, CardType.Five)
            };

            hand.BuildSets();

            int total = hand.GetFifteens();

            Assert.AreEqual(total, 8);
            Assert.AreEqual(hand.fifteens.Count, 4);
        }

        [Test]
        public void SixFiveFiveFourAce_CorrectFifteenScore()
        {
            Card six = new Card(Suit.Clubs, CardType.Six);
            Card five1 = new Card(Suit.Hearts, CardType.Five);
            Card five2 = new Card(Suit.Spades, CardType.Five);
            Card four = new Card(Suit.Spades, CardType.Four);
            Card ace = new Card(Suit.Diamonds, CardType.Ace);

            hand.Cards = new List<Card>() { six, five1, five2, four, ace };

            hand.BuildSets();

            int total = hand.GetFifteens();

            Assert.AreEqual(total, 6);
            Assert.AreEqual(hand.fifteens.Count, 3);
        }

        [Test]
        public void AceTwoThreeFourFive_CorrectFifteenScore()
        {
            Card six = new Card(Suit.Clubs, CardType.Ace);
            Card five1 = new Card(Suit.Hearts, CardType.Two);
            Card five2 = new Card(Suit.Spades, CardType.Three);
            Card four = new Card(Suit.Spades, CardType.Four);
            Card ace = new Card(Suit.Diamonds, CardType.Five);

            hand.Cards = new List<Card>() { six, five1, five2, four, ace };

            hand.BuildSets();

            int total = hand.GetFifteens();

            Assert.AreEqual(total, 2);
            Assert.AreEqual(hand.fifteens.Count, 1);
            Assert.AreEqual(hand.fifteens[0].Length, 5);
        }

        [Test]
        public void OnePair_CorrectScore()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.King),
                new Card(Suit.Hearts, CardType.King),
                new Card(Suit.Spades, CardType.Two),
                new Card(Suit.Spades, CardType.Ten),
                new Card(Suit.Diamonds, CardType.Seven)
            };

            hand.BuildSets();

            int total = hand.GetPairs();

            Assert.AreEqual(total, 2);
        }

        [Test]
        public void TwoPair_CorrectScore()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.King),
                new Card(Suit.Hearts, CardType.King),
                new Card(Suit.Spades, CardType.Two),
                new Card(Suit.Spades, CardType.Two),
                new Card(Suit.Diamonds, CardType.Seven)
            };

            hand.BuildSets();

            int total = hand.GetPairs();

            Assert.AreEqual(total, 4);
        }

        [Test]
        public void Trips_CorrectScore()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.King),
                new Card(Suit.Hearts, CardType.King),
                new Card(Suit.Spades, CardType.King),
                new Card(Suit.Spades, CardType.Two),
                new Card(Suit.Diamonds, CardType.Seven)
            };

            hand.BuildSets();

            int total = hand.GetPairs();

            Assert.AreEqual(total, 6);
        }

        [Test]
        public void Quads_CorrectScore()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.King),
                new Card(Suit.Hearts, CardType.King),
                new Card(Suit.Spades, CardType.King),
                new Card(Suit.Diamonds, CardType.King),
                new Card(Suit.Diamonds, CardType.Seven)
            };

            hand.BuildSets();

            int total = hand.GetPairs();

            Assert.AreEqual(total, 12);
        }

        [Test]
        public void RunOfFive_CorrectScore()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.Ace),
                new Card(Suit.Hearts, CardType.Two),
                new Card(Suit.Spades, CardType.Three),
                new Card(Suit.Diamonds, CardType.Four),
                new Card(Suit.Diamonds, CardType.Five)
            };

            hand.BuildSets();

            int total = hand.GetRuns();

            Assert.AreEqual(total, 5);
        }

        [Test]
        public void RunOfFour_CorrectScore()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.Three),
                new Card(Suit.Hearts, CardType.Ten),
                new Card(Suit.Spades, CardType.Jack),
                new Card(Suit.Diamonds, CardType.Queen),
                new Card(Suit.Diamonds, CardType.King)
            };

            hand.BuildSets();

            int total = hand.GetRuns();

            Assert.AreEqual(total, 4);
        }

        [Test]
        public void RunOfOneThree_CorrectScore()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.Ace),
                new Card(Suit.Hearts, CardType.Two),
                new Card(Suit.Spades, CardType.Three),
                new Card(Suit.Diamonds, CardType.Six),
                new Card(Suit.Diamonds, CardType.Jack)
            };

            hand.BuildSets();

            int total = hand.GetRuns();

            Assert.AreEqual(total, 3);
        }

        [Test]
        public void RunOfDoubleThree_CorrectScore()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.Ace),
                new Card(Suit.Hearts, CardType.Two),
                new Card(Suit.Spades, CardType.Three),
                new Card(Suit.Diamonds, CardType.Ace),
                new Card(Suit.Diamonds, CardType.Jack)
            };

            hand.BuildSets();

            int total = hand.GetRuns();

            Assert.AreEqual(total, 6);
        }

        [Test]
        public void RunOfThreeThrees_CorrectScore()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.Ace),
                new Card(Suit.Hearts, CardType.Two),
                new Card(Suit.Spades, CardType.Three),
                new Card(Suit.Diamonds, CardType.Ace),
                new Card(Suit.Diamonds, CardType.Ace)
            };

            hand.BuildSets();

            int total = hand.GetRuns();

            Assert.AreEqual(total, 9);
        }

        [Test]
        public void RunOfFourThrees_CorrectScore()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.Ace),
                new Card(Suit.Hearts, CardType.Two),
                new Card(Suit.Spades, CardType.Three),
                new Card(Suit.Diamonds, CardType.Ace),
                new Card(Suit.Diamonds, CardType.Two)
            };

            hand.BuildSets();

            int total = hand.GetRuns();

            Assert.AreEqual(total, 12);
        }

        [Test]
        public void RunDoesNotWrap()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.Ace),
                new Card(Suit.Hearts, CardType.Two),
                new Card(Suit.Spades, CardType.Jack),
                new Card(Suit.Diamonds, CardType.King),
                new Card(Suit.Diamonds, CardType.Eight)
            };

            hand.BuildSets();

            int total = hand.GetRuns();

            Assert.AreEqual(total, 0);
        }

        [Test]
        public void FlushNotCribWithCut_ScoresCorrectly()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Diamonds, CardType.Ace),
                new Card(Suit.Diamonds, CardType.Two),
                new Card(Suit.Diamonds, CardType.Jack),
                new Card(Suit.Diamonds, CardType.King),
                new Card(Suit.Diamonds, CardType.Eight)
            };

            hand.Cards[4].IsCutCard = true;

            hand.BuildSets();

            int total = hand.GetFlushes();

            Assert.AreEqual(total, 5);
        }

        [Test]
        public void FlushNotCribWithoutCut_ScoresCorrectly()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Diamonds, CardType.Ace),
                new Card(Suit.Diamonds, CardType.Two),
                new Card(Suit.Diamonds, CardType.Jack),
                new Card(Suit.Diamonds, CardType.King),
                new Card(Suit.Clubs, CardType.Eight)
            };

            hand.Cards[4].IsCutCard = true;

            hand.BuildSets();

            int total = hand.GetFlushes();

            Assert.AreEqual(total, 4);
        }

        [Test]
        public void FlushNotCribNoflush_ScoresCorrectly()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Diamonds, CardType.Ace),
                new Card(Suit.Diamonds, CardType.Two),
                new Card(Suit.Diamonds, CardType.Jack),
                new Card(Suit.Spades, CardType.King),
                new Card(Suit.Clubs, CardType.Eight)
            };

            hand.Cards[4].IsCutCard = true;

            hand.BuildSets();

            int total = hand.GetFlushes();

            Assert.AreEqual(total, 0);
        }

        [Test]
        public void FlushNotCribNoflushWithCut_ScoresCorrectly()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Diamonds, CardType.Ace),
                new Card(Suit.Diamonds, CardType.Two),
                new Card(Suit.Diamonds, CardType.Jack),
                new Card(Suit.Spades, CardType.King),
                new Card(Suit.Diamonds, CardType.Eight)
            };

            hand.Cards[4].IsCutCard = true;

            hand.BuildSets();

            int total = hand.GetFlushes();

            Assert.AreEqual(total, 0);
        }

        [Test]
        public void FlushCribWithCut_ScoresCorrectly()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Diamonds, CardType.Ace),
                new Card(Suit.Diamonds, CardType.Two),
                new Card(Suit.Diamonds, CardType.Jack),
                new Card(Suit.Diamonds, CardType.King),
                new Card(Suit.Diamonds, CardType.Eight)
            };

            hand.Cards[4].IsCutCard = true;
            hand.IsCrib = true;

            hand.BuildSets();

            int total = hand.GetFlushes();

            Assert.AreEqual(total, 5);
        }

        [Test]
        public void FlushCribWithoutCut_ScoresCorrectly()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Diamonds, CardType.Ace),
                new Card(Suit.Diamonds, CardType.Two),
                new Card(Suit.Diamonds, CardType.Jack),
                new Card(Suit.Diamonds, CardType.King),
                new Card(Suit.Clubs, CardType.Eight)
            };

            hand.Cards[4].IsCutCard = true;
            hand.IsCrib = true;

            hand.BuildSets();

            int total = hand.GetFlushes();

            Assert.AreEqual(total, 0);
        }

        [Test]
        public void Nobs_ScoresCorrectly()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Diamonds, CardType.Ace),
                new Card(Suit.Diamonds, CardType.Two),
                new Card(Suit.Diamonds, CardType.Jack),
                new Card(Suit.Diamonds, CardType.King),
                new Card(Suit.Clubs, CardType.Eight)
            };

            hand.Cards[3].IsCutCard = true;

            int score = hand.GetNobs(CardType.Jack);

            Assert.AreEqual(score, 1);
        }

        [Test]
        public void NoNobs_ScoresCorrectly()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Diamonds, CardType.Ace),
                new Card(Suit.Diamonds, CardType.Two),
                new Card(Suit.Diamonds, CardType.Jack),
                new Card(Suit.Diamonds, CardType.King),
                new Card(Suit.Clubs, CardType.Eight)
            };

            hand.Cards[4].IsCutCard = true;

            int score = hand.GetNobs(CardType.Jack);

            Assert.AreEqual(score, 0);
        }

        [Test]
        public void FullScore_DoubleRun()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Diamonds, CardType.Four),
                new Card(Suit.Diamonds, CardType.Five),
                new Card(Suit.Spades, CardType.Five),
                new Card(Suit.Diamonds, CardType.Six),
            };

            hand.GetScore(new Card(Suit.Clubs, CardType.Eight), CardType.Jack);

            Assert.AreEqual(hand.Score, 12);
        }

        [Test]
        public void FullScore_FourFives()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Diamonds, CardType.Five),
                new Card(Suit.Clubs, CardType.Five),
                new Card(Suit.Spades, CardType.Five),
                new Card(Suit.Hearts, CardType.Eight),
            };

            hand.GetScore(new Card(Suit.Hearts, CardType.Five), CardType.Jack);

            Assert.AreEqual(hand.Score, 20);
        }

        [Test]
        public void FullScore_TwentyNine()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Diamonds, CardType.Five),
                new Card(Suit.Clubs, CardType.Five),
                new Card(Suit.Spades, CardType.Five),
                new Card(Suit.Hearts, CardType.Jack),
            };

            hand.GetScore(new Card(Suit.Hearts, CardType.Five), CardType.Jack);

            Assert.AreEqual(hand.Score, 29);
        }

        [Test]
        public void FullScore_TripsWithRun()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Diamonds, CardType.Nine),
                new Card(Suit.Clubs, CardType.Nine),
                new Card(Suit.Spades, CardType.Nine),
                new Card(Suit.Hearts, CardType.Jack),
            };

            hand.GetScore(new Card(Suit.Diamonds, CardType.Ten), CardType.Jack);

            Assert.AreEqual(hand.Score, 15);
        }

        [Test]
        public void FullScore_zero()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Diamonds, CardType.Nine),
                new Card(Suit.Clubs, CardType.Seven),
                new Card(Suit.Spades, CardType.Two),
                new Card(Suit.Hearts, CardType.Jack),
            };

            hand.GetScore(new Card(Suit.Diamonds, CardType.Ace), CardType.Jack);

            Assert.AreEqual(hand.Score, 0);
        }

        [Test]
        public void FullScore_FlushAndRun()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.Ace),
                new Card(Suit.Clubs, CardType.Two),
                new Card(Suit.Clubs, CardType.Three),
                new Card(Suit.Clubs, CardType.Four),
            };

            hand.GetScore(new Card(Suit.Clubs, CardType.Five), CardType.Jack);

            Assert.AreEqual(hand.Score, 12);
        }

        [Test]
        public void FullScore_RunFourSameSuitNotCutInCrib()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.Ace),
                new Card(Suit.Clubs, CardType.Two),
                new Card(Suit.Clubs, CardType.Three),
                new Card(Suit.Clubs, CardType.Four),
            };

            hand.IsCrib = true;

            hand.GetScore(new Card(Suit.Spades, CardType.Five), CardType.Jack);

            Assert.AreEqual(hand.Score, 7);
        }

        [Test]
        public void FullScore_DoubleRunSevenEightNine()
        {
            hand.Cards = new List<Card>() {
                new Card(Suit.Clubs, CardType.Seven),
                new Card(Suit.Clubs, CardType.Eight),
                new Card(Suit.Hearts, CardType.Eight),
                new Card(Suit.Hearts, CardType.Nine),
            };

            hand.IsCrib = true;

            hand.GetScore(new Card(Suit.Spades, CardType.Five), CardType.Jack);

            Assert.AreEqual(hand.Score, 12);
        }
    }
}