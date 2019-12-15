using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace cribrage
{
    public class Hand
    {
        public bool Discarded { get; set; }
        public List<Card> Cards { get; set; }
        public int Score { get; set; }
        public bool IsCrib { get; set; }
        public int CardDrawPosX { get; set; }
        public int CardDrawPosY { get; set; }


        //sets of unique card combinations in hand
        public Card[][] setOfTwos = new Card[10][];
        public Card[][] setOfThrees = new Card[10][];
        public Card[][] setOfFours = new Card[5][];

        //sets of cards that make up scores
        //TODO: may need to tweak these to display scoring breakdowns
        public List<Card[]> fifteens = new List<Card[]>();
        public List<Card[]> pairs = new List<Card[]>();
        public List<Card[]> runs = new List<Card[]>();

        public Hand(bool isCrib = false)
        {
            Cards = new List<Card>();
            Score = 0;
            IsCrib = isCrib;
        }

        /// <summary>
        /// Used to help estimate best possible hand before discarding
        /// </summary>
        /// <returns></returns>
        public int GetScoreBeforeCut()
        {
            int score = 0;
            score += GetFifteens();
            score += GetPairs();
            score += GetRuns();
            score += GetFlushes();

            return score;
        }
        
        /// <summary>
        /// Gets the total score in the scoring round after a card has been cut
        /// </summary>
        /// <param name="cut"></param>
        /// <param name="nobsCard"></param>
        public void GetScore(Card cut, CardType nobsCard = CardType.Jack)
        {
            Score = 0;
            cut.IsCutCard = true;
            Cards.Add(cut);
            BuildSets();
            Score += GetFifteens();
            Score += GetPairs();
            Score += GetRuns();
            Score += GetFlushes();
            Score += GetNobs(nobsCard);
            Cards.Remove(cut);
        }

        /// <summary>
        /// Builds sets of unique cards for sets of two, three, and four 
        /// to be used by scoring methods to determine scores
        /// </summary>
        public void BuildSets()
        {
            //build sets of 2
            int index = 0;
            for(int i = 0; i < 5; i++)
            {
                for(int j = 4; j > i; j--)
                {
                    setOfTwos[index] = new Card[] { Cards[i], Cards[j]};
                    index++;
                }
            }

            //build sets of 3
            index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (j == i) break;
                    for(int k = 0; k < 5; k++)
                    {
                        if (k == j || k == i) break;
                        setOfThrees[index] = new Card[] { Cards[i], Cards[j], Cards[k] };
                        index++;
                    }
                }
            }

            //build sets of 4
            index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (j == i) break;
                    for (int k = 0; k < 5; k++)
                    {
                        if (k == j || k == i) break;
                        for (int l = 0; i < 5; l++)
                        {
                            if (l == k || l == j || l == i) break;
                            setOfFours[index] = new Card[] { Cards[i], Cards[j], Cards[k], Cards[l] };
                            index++;
                            Debug.WriteLine(index + ". " + Cards[i].Name + ", " + Cards[j].Name + ", " + Cards[k].Name + ", " + Cards[l].Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get total score from fifteens in hand
        /// </summary>
        /// <returns></returns>
        public int GetFifteens()
        {
            int scoreFromFifteens = 0;

            for(int i = 0; i < setOfTwos.Length; i++)
            {
                if(setOfTwos[i][0].Value + setOfTwos[i][1].Value == 15)
                {
                    scoreFromFifteens += 2;
                    fifteens.Add(setOfTwos[i]);
                }
            }

            for(int i = 0; i < setOfThrees.Length; i++)
            {
                if(setOfThrees[i][0].Value + setOfThrees[i][1].Value + setOfThrees[i][2].Value == 15)
                {
                    scoreFromFifteens += 2;
                    fifteens.Add(setOfThrees[i]);
                }
            }

            for(int i = 0; i < setOfFours.Length; i++)
            {
                if(setOfFours[i][0].Value + setOfFours[i][1].Value + setOfFours[i][2].Value + setOfFours[i][3].Value  == 15)
                {
                    scoreFromFifteens += 2;
                    fifteens.Add(setOfFours[i]);
                }
            }

            if(Cards[0].Value + Cards[1].Value + Cards[2].Value + Cards[3].Value + Cards[4].Value == 15)
            {
                scoreFromFifteens += 2;
                fifteens.Add(Cards.ToArray());
            }

            return scoreFromFifteens;
        }

        /// <summary>
        /// Get total score from pairs in hand
        /// </summary>
        /// <returns></returns>
        public int GetPairs()
        {
            int scoreFromPairs = 0;

            for(int i = 0; i < setOfTwos.Length; i++)
            {
                if(setOfTwos[i][0].Ordinal == setOfTwos[i][1].Ordinal)
                {
                    scoreFromPairs += 2;
                    pairs.Add(setOfTwos[i]);
                }
            }

            return scoreFromPairs;
        }

        /// <summary>
        /// Get total score from runs in hand
        /// </summary>
        /// <returns></returns>
        public int GetRuns()
        {

            //run of five
            Cards.Sort((a,b) => (a.Ordinal).CompareTo(b.Ordinal));
            
            if(Cards[0].Ordinal + 1 == Cards[1].Ordinal &&
               Cards[1].Ordinal + 1 == Cards[2].Ordinal &&
               Cards[2].Ordinal + 1 == Cards[3].Ordinal &&
               Cards[3].Ordinal + 1 == Cards[4].Ordinal)
            {
                runs.Add(Cards.ToArray());
                return 5;
            }

            //check fours
            for(int i = 0; i < setOfFours.Length; i++)
            {
                Array.Sort(setOfFours[i], (a, b) => (a.Ordinal).CompareTo(b.Ordinal));
                if(setOfFours[i][0].Ordinal + 1 == setOfFours[i][1].Ordinal &&
                   setOfFours[i][1].Ordinal + 1 == setOfFours[i][2].Ordinal &&
                   setOfFours[i][2].Ordinal + 1 == setOfFours[i][3].Ordinal)
                {
                    runs.Add(setOfFours[i]);
                    return 4;
                }
            }

            int sumOfThreeRuns = 0;
            //check all combinations of 3 in case of double run
            for (int i = 0; i < setOfThrees.Length; i++)
            {
                Array.Sort(setOfThrees[i], (a, b) => (a.Ordinal).CompareTo(b.Ordinal));
                if (setOfThrees[i][0].Ordinal + 1 == setOfThrees[i][1].Ordinal &&
                   setOfThrees[i][1].Ordinal + 1 == setOfThrees[i][2].Ordinal)
                {
                    runs.Add(setOfThrees[i]);
                    sumOfThreeRuns += 3;
                }
            }

            return sumOfThreeRuns;
        }

        /// <summary>
        /// Get total score from flushes in hand
        /// </summary>
        /// <returns></returns>
        public int GetFlushes()
        {
            List<Card> OriginalHand = new List<Card>();
            Card cutCard = new Card();

            foreach(Card c in Cards)
            {
                if (!c.IsCutCard)
                    OriginalHand.Add(c);
                else
                    cutCard = c;
            }

            if (!IsCrib)
            {
                int score = 0;
                if (OriginalHand[0].Suit == OriginalHand[1].Suit &&
                   OriginalHand[1].Suit == OriginalHand[2].Suit &&
                   OriginalHand[2].Suit == OriginalHand[3].Suit)
                {
                    score = 4;
                    if(cutCard.Suit == OriginalHand[3].Suit)
                    {
                        score += 1;
                    }
                }

                return score;
            }
            else
            {
                if(Cards[0].Suit == Cards[1].Suit &&
                   Cards[1].Suit == Cards[2].Suit &&
                   Cards[2].Suit == Cards[3].Suit &&
                   Cards[3].Suit == Cards[4].Suit)
                {
                    return 5;
                }
            }

            return 0;
        }

        /// <summary>
        /// Get score from nobs in hand
        /// </summary>
        /// <param name="nobsType"></param>
        /// <returns></returns>
        public int GetNobs(CardType nobsType)
        {
            List<Card> nobsCards = new List<Card>();
            Card cutCard = new Card();
            foreach(Card c in Cards)
            {
                if (c.Type == nobsType)
                    nobsCards.Add(c);
                if (c.IsCutCard)
                    cutCard = c;
            }

            foreach(Card nc in nobsCards)
            {
                if(nc.Ordinal > 0 && nc.Type != cutCard.Type)
                {
                    if (nc.Suit == cutCard.Suit)
                        return 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// Empties the hand of any cards
        /// </summary>
        public void Reset()
        {
            Cards.Clear();
        }
    }
}
