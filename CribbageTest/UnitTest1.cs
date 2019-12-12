using cribrage;
using NUnit.Framework;

namespace Tests
{
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

            hand.GetScore(deck.Cards[4]);

            foreach(var set in hand.setOfTwos)
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

            hand.GetScore(deck.Cards[4]);

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

            hand.GetScore(deck.Cards[4]);

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
    }
}