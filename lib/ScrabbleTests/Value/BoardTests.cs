using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrabble.Value;
using Shouldly;
using System.Linq;

namespace Scrabble.Tests
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void CanCreateBoard()
        {
            var board = new Board();

            string[,] squares = new string[15, 15]
            {
                { "TW", "NS", "NS", "DL", "NS", "NS", "NS", "TW", "NS", "NS", "NS", "DL", "NS", "NS", "TW" },
                { "NS", "DW", "NS", "NS", "NS", "TL", "NS", "NS", "NS", "TL", "NS", "NS", "NS", "DW", "NS" },
                { "NS", "NS", "DW", "NS", "NS", "NS", "DL", "NS", "DL", "NS", "NS", "NS", "DW", "NS", "NS" },
                { "DL", "NS", "NS", "DW", "NS", "NS", "NS", "DL", "NS", "NS", "NS", "DW", "NS", "NS", "DL" },
                { "NS", "NS", "NS", "NS", "DW", "NS", "NS", "NS", "NS", "NS", "DW", "NS", "NS", "NS", "NS" },
                { "NS", "TL", "NS", "NS", "NS", "TL", "NS", "NS", "NS", "TL", "NS", "NS", "NS", "TL", "NS" },
                { "NS", "NS", "DL", "NS", "NS", "NS", "DL", "NS", "DL", "NS", "NS", "NS", "DL", "NS", "NS" },
                { "TW", "NS", "NS", "DL", "NS", "NS", "NS", "ST", "NS", "NS", "NS", "DL", "NS", "NS", "TW" },
                { "NS", "NS", "DL", "NS", "NS", "NS", "DL", "NS", "DL", "NS", "NS", "NS", "DL", "NS", "NS" },
                { "NS", "TL", "NS", "NS", "NS", "TL", "NS", "NS", "NS", "TL", "NS", "NS", "NS", "TL", "NS" },
                { "NS", "NS", "NS", "NS", "DW", "NS", "NS", "NS", "NS", "NS", "DW", "NS", "NS", "NS", "NS" },
                { "DL", "NS", "NS", "DW", "NS", "NS", "NS", "DL", "NS", "NS", "NS", "DW", "NS", "NS", "DL" },
                { "NS", "NS", "DW", "NS", "NS", "NS", "DL", "NS", "DL", "NS", "NS", "NS", "DW", "NS", "NS" },
                { "NS", "DW", "NS", "NS", "NS", "TL", "NS", "NS", "NS", "TL", "NS", "NS", "NS", "DW", "NS" },
                { "TW", "NS", "NS", "DL", "NS", "NS", "NS", "TW", "NS", "NS", "NS", "DL", "NS", "NS", "TW" }
            };

            for (int y = 0; y < 15; y++)
            {
                for (int x = 0; x < 15; x++)
                {
                    var square = board.Squares.FirstOrDefault(s => s.X == x && s.Y == y);
                    square.Name.ShouldBe(squares[x, y], $"square {x},{y} should be named {squares[x, y]}");
                }
            }
        }

        [TestMethod]
        public void CanGetBoardSquareByRowAndColumn()
        {
            var board = new Board();
            board.GetSquare(7, 7).Name.ShouldBe("ST");
        }
    }
}
