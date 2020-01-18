using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrabble.Value;
using Shouldly;

namespace Scrabble.Tests
{
    [TestClass]
    public class BoardSquareTests
    {
        BoardSquare square;

        void AssertSquareNameLetterBonusAndWordBonus(string name, int letterBonus, int wordBonus)
        {
            square.Name.ShouldBe(name);
            square.LetterBonus.ShouldBe(letterBonus);
            square.WordBonus.ShouldBe(wordBonus);
        }

        [TestMethod]
        public void CanCreateNormalSquare()
        {
            square = BoardSquare.NormalSquare();
            AssertSquareNameLetterBonusAndWordBonus("NS", 1, 1);
        }

        [TestMethod]
        public void CanCreateDoubleLetterSquare()
        {
            square = BoardSquare.DoubleLetterSquare();
            AssertSquareNameLetterBonusAndWordBonus("DL", 2, 1);
        }

        [TestMethod]
        public void CanCreateTripleLetterSquare()
        {
            square = BoardSquare.TrippleLetterSquare();
            AssertSquareNameLetterBonusAndWordBonus("TL", 3, 1);
        }

        [TestMethod]
        public void CanCreateDoubleWordSquare()
        {
            square = BoardSquare.DoubleWordSquare();
            AssertSquareNameLetterBonusAndWordBonus("DW", 1, 2);
        }

        [TestMethod]
        public void CanCreateTripleWordSquare()
        {
            square = BoardSquare.TrippleWordSquare();
            AssertSquareNameLetterBonusAndWordBonus("TW", 1, 3);
        }

        [TestMethod]
        public void CanCreateStartingSquare()
        {
            square = BoardSquare.StartingSquare();
            AssertSquareNameLetterBonusAndWordBonus("ST", 1, 2);
        }
    }
}
