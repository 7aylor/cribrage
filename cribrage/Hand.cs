using System;
using System.Collections.Generic;
using System.Text;

namespace cribrage
{
    class Hand
    {
        public bool Discarded { get; set; }
        public List<Card> Cards { get; set; }
        public int Score { get; set; }
        
        public void SetScore(Card cut)
        {
            
        }

        private int GetNumFifteens()
        {
            return 0;
        }
    }
}
