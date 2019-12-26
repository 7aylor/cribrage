using System;
using System.Collections.Generic;
using System.Text;
using cribrage;
using NUnit.Framework;

namespace CribbageTest
{
    [TestFixture]
    class GameManagerTests
    {
        [Test]
        public void GoToNextPhase_Works()
        {
            //gm inits state as deal, next state should be discard
            GameManager gm = new GameManager();
            gm.GoToNextPhase();

            Assert.AreEqual(gm.State, GameState.Discard);
        }
    }
}
