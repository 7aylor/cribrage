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

        public Card[][] setOfTwos = new Card[10][];
        public Card[][] setOfThrees = new Card[10][];
        public Card[][] setOfFours = new Card[5][];

        public List<Card[]> fifteens = new List<Card[]>();
        public List<Card[]> pairs = new List<Card[]>();
        public List<Card[]> runs = new List<Card[]>();

        public Hand(bool isCrib = false)
        {
            Cards = new List<Card>();
            Score = 0;
            IsCrib = isCrib;
        }
        
        public void GetScore(Card cut)
        {
            cut.IsCutCard = true;
            Cards.Add(cut);
            BuildSets();
            Score += GetFifteens();
            Score += GetPairs();
        }

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

        //public int GetFlushes()
        //{
            //if(!IsCrib)
            //{
            //    Suit suit = Cards[0].IsCutCard ? Cards[1].Suit : Cards[0].Suit;
            //    for(int i = 0; i < Cards.Count; i++)
            //    {
            //        if (Cards[i].Suit != suit)
            //            break;
            //    }
            //}

            //return 0;
        //}

    }
}
