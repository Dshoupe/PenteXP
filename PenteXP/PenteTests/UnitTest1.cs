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
            p.Captures = 3;
            Assert.AreEqual(6, p.Captures);
            Player.id = 1;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Captures_CrashesWhenPassedtNegatives()
        {
            
        }

        [TestMethod]
        public void Captures_ActivatesWinBoolean()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Captures_CrashesWhenMorePiecesThanPossibleCaptured()
        {

        }
        



#endregion
    }
}