using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PenteXP.Models;

namespace PenteTests
{
    [TestClass]
    public class UnitTest1
    {
        #region Player Tests

        //Name
        [TestMethod]
        public void Name_AcceptsValidInput()
        {
            HumanPlayer p = new HumanPlayer("Test Player");

            Assert.AreEqual("Test Player", p.Name);
            Player.id = 1;
        }

        [TestMethod]
        public void Name_AcceptsValidInputPlayer2()
        {
            HumanPlayer p = new HumanPlayer("Test Player");
            HumanPlayer p2 = new HumanPlayer("Test Player2");
            Assert.AreEqual("Test Player2", p2.Name);
            Player.id = 1;
        }

        [TestMethod]
        public void Name_DefaultGivenWhiteSpace()
        {
            HumanPlayer p = new HumanPlayer("      ");
            Assert.AreEqual("Player 1", p.Name);
            Player.id = 1;
        }

        [TestMethod]
        public void Name_DefaultGivenWhiteSpacePlayer2()
        {
            HumanPlayer p = new HumanPlayer("      ");
            HumanPlayer p2 = new HumanPlayer("      ");
            Assert.AreEqual("Player 2", p2.Name);
            Player.id = 1;
        }

        [TestMethod]
        public void Name_DefaultsWhenGivenEmptyString()
        {
            HumanPlayer p = new HumanPlayer("");
            Assert.AreEqual("Player 1", p.Name);
            Player.id = 1;
        }

        [TestMethod]
        public void Name_DefaultsWhenGivenEmptyStringPlayer2()
        {
            HumanPlayer p = new HumanPlayer("");
            HumanPlayer p2 = new HumanPlayer("");
            Assert.AreEqual("Player 2", p2.Name);
            Player.id = 1;
        }


        [TestMethod]
        public void Name_DefaultsWhenGivenNull()
        {
            HumanPlayer p = new HumanPlayer(null);
            Assert.AreEqual("Player 1", p.Name);
            Player.id = 1;
        }

        [TestMethod]
        public void Name_DefaultsWhenGivenNullPlayer2()
        {
            HumanPlayer p = new HumanPlayer(null);
            HumanPlayer p2 = new HumanPlayer(null);
            Assert.AreEqual("Player 2", p2.Name);
            Player.id = 1;
        }

        //Captures
        [TestMethod]
        public void Captures_IncrementsProperly()
        {
            HumanPlayer p = new HumanPlayer("Test Player");
            p.Captures = 3;
            Assert.AreEqual(3, p.Captures);
            Player.id = 1;
        }

        [TestMethod]
        public void Captures_IncrementsProperlyAfterFirst()
        {
            HumanPlayer p = new HumanPlayer("Test Player");
            p.Captures = 3;
            p.Captures += 3;
            Assert.AreEqual(6, p.Captures);
            Player.id = 1;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Captures_CrashesWhenPassedtNegatives()
        {
            HumanPlayer p = new HumanPlayer("Test Player");
            p.Captures = -1;
        }

        [TestMethod]
        public void Captures_ActivatesWinBoolean()
        {
            HumanPlayer p = new HumanPlayer("Test Player");
            Game g = new Game();
            p.Captures = 5;
            Assert.AreEqual(g.CheckWin(), true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Captures_CrashesWhenMorePiecesThanPossibleCaptured()
        {
            HumanPlayer p = new HumanPlayer("Test Player");
            p.Captures = 100;
        }

        [TestMethod]
        public void GetPlayerColorForPlayer1()
        {
            HumanPlayer p1 = new HumanPlayer("Test Player");
            p1.id = 1;
            Assert.AreEqual(p1.PlayerColor, Colors.Black);
        }

        [TestMethod]
        public void GetPlayerColorForPlayer2()
        {
            HumanPlayer p2 = new HumanPlayer("Test Player");
            p2.id = 2;
            Assert.AreEqual(p2.PlayerColor, Colors.White);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetInvalidColor()
        {
            HumanPlayer p = new HumanPlayer("Test Player");
            p.id = 1;
            p.PlayerColor = COlors.Orange;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetColorNull()
        {
            HumanPlayer p = new HumanPlayer("Test Player");
            p.id = 1;
            p.PlayerColor = null;
        }

        #endregion

        #region Game tests

        [TestMethod]
        public void CheckWinNull()
        {
            Game g = new Game();
            Assert.AreNotEqual(g.CheckWin(), null);
        }

        #endregion
    }
}