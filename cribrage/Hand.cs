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

        public Card[][] setOfTwos = new Card[10][];
        public Card[][] setOfThrees = new Card[10][];
        public Card[][] setOfFours = new Card[5][];

        public Hand()
        {
            Cards = new List<Card>();
        }
        
        public void GetScore(Card cut)
        {
            Cards.Add(cut);
            BuildSets();
        }

        private void BuildSets()
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

            //build set of 5

        }

        private int GetNumFifteens()
        {
            //for(int i = 2; i <= 5; i++)
            //{
            //    for(int j = 0; j < )
            //}

            return 0;
        }
    }
}
