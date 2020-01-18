using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Scrabble.Go;
using Scrabble.Value;
using Shouldly;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Tests
{
    [TestClass]
    public class GoWordFinderTests
    {
        IGridModel gridModel;
        GridModelTile[,] grid;
        GoWordFinder wordFinder;
        IEnumerable<GoWord> foundWords;

        [TestInitialize]
        public void Setup()
        {
            gridModel = Substitute.For<IGridModel>();
            grid = new GridModelTile[15, 15];
            gridModel.Grid.Returns(grid);
            wordFinder = new GoWordFinder(gridModel);
        }

        void GivenPlayerHorizontalGo(string letters, int startx, int starty, int gostartx = -1)
        {
            gridModel.IsHorizontalGo.Returns(true);
            gridModel.GoStartX.Returns(gostartx == -1 ? startx : gostartx);
            gridModel.GoStartY.Returns(starty);

            int x = startx;
            foreach (char letter in letters)
            {
                while (!grid[x, starty].IsEmpty())
                {
                    x++;
                }

                grid[x, starty].Letter = letter;
                grid[x, starty].Origin = GridModelTileOrigin.FromPlayer;
                x++;
            }
        }

        void GivenPlayerVerticalGo(string letters, int startx, int starty)
        {
            gridModel.IsVerticalGo.Returns(true);
            gridModel.GoStartX.Returns(startx);
            gridModel.GoStartY.Returns(starty);

            int y = starty;
            foreach (char letter in letters)
            {
                grid[startx, y].Letter = letter;
                grid[startx, y].Origin = GridModelTileOrigin.FromPlayer;
                y++;
            }
        }

        void GivenBoardTile(char letter, int x, int y)
        {
            gridModel.Grid[x, y].Letter = letter;
            gridModel.Grid[x, y].Origin = GridModelTileOrigin.FromBoard;
        }

        void GivenBonusTile(BoardSquare square, int x, int y)
        {
            gridModel.GetBoardSquare(x, y).Returns(square);
            gridModel.GetGoLetter(x, y).Returns(new GoLetter()
            {
                LetterBonus = square.LetterBonus,
                WordBonus = square.WordBonus
            });
        }

        void WhenFindWords()
        {
            foundWords = wordFinder.FindWords();
        }
        
        void AssertWordsFound(params string[] words)
        {
            foundWords.Count().ShouldBe(words.Length);
            int n = 0;
            foreach (var foundWord in foundWords)
            {
                foundWord.Word.ShouldBe(words[n++]);
            }
        }

        [TestMethod]
        public void GivenNoTilesArePlaced_ThenNoWordsAreFound()
        {
            wordFinder.FindWords().Count().ShouldBe(0);
        }

        [TestMethod]
        public void GivenOneTileIsPlaced_ThenOneWordIsFound()
        {
            GivenPlayerHorizontalGo("A", 7, 7);
            WhenFindWords();
            AssertWordsFound("A");
        }

        [TestMethod]
        public void GivenHorizontalGo_ThenWordIsFound()
        {
            GivenPlayerHorizontalGo("CAT", 7, 7);
            WhenFindWords();
            AssertWordsFound("CAT");
        }

        [TestMethod]
        public void GivenVerticalGo_ThenWordIsFound()
        {
            GivenPlayerVerticalGo("CAT", 7, 7);
            WhenFindWords();
            AssertWordsFound("CAT");
        }

        [TestMethod]
        public void GivenHorizontalGoWithSideWordAbove_ThenBothWordsAreFound()
        {
            GivenBoardTile('A', 9, 6);
            GivenPlayerHorizontalGo("CAT", 7, 7);
            WhenFindWords();
            AssertWordsFound("CAT", "AT");
        }

        [TestMethod]
        public void GivenHorizontalGoWithSideWordBelow_ThenBothWordsAreFound()
        {
            GivenBoardTile('O', 9, 8);
            GivenPlayerHorizontalGo("CAT", 7, 7);
            WhenFindWords();
            AssertWordsFound("CAT", "TO");
        }

        [TestMethod]
        public void GivenVerticalGoWithSideWordLeft_ThenBothWordsAreFound()
        {
            GivenBoardTile('A', 6, 9);
            GivenPlayerVerticalGo("CAT", 7, 7);
            WhenFindWords();
            AssertWordsFound("CAT", "AT");
        }

        [TestMethod]
        public void GivenHorizontalGoWithSideWordRight_ThenBothWordsAreFound()
        {
            GivenBoardTile('O', 8, 9);
            GivenPlayerVerticalGo("CAT", 7, 7);
            WhenFindWords();
            AssertWordsFound("CAT", "TO");
        }

        [TestMethod]
        public void GivenGoPlacesATileCompletelySurrounded_ThenBothWordsAreFound()
        {
            GivenBoardTile('T', 7, 6);
            GivenBoardTile('P', 7, 8);
            GivenBoardTile('P', 6, 7);
            GivenBoardTile('P', 8, 7);
            GivenPlayerHorizontalGo("O", 7, 7, 6);
            WhenFindWords();
            AssertWordsFound("POP", "TOP");
        }

        [TestMethod]
        public void GivenGoIntersectsABoardWord_ThenTheIntersectedWordIsNotFound()
        {
            GivenBoardTile('T', 7, 6);
            GivenBoardTile('A', 7, 7);
            GivenBoardTile('L', 7, 8);
            GivenBoardTile('K', 7, 9);
            GivenPlayerHorizontalGo("WAER", 5, 6);
            WhenFindWords();
            AssertWordsFound("WATER");
        }

        [TestMethod]
        public void GivenASideWordThatBeginsOnTheLeftSideOfTheBoard_ThenItShouldBeFound()
        {
            GivenBoardTile('T', 0, 11);
            GivenBoardTile('I', 1, 11);
            GivenBoardTile('E', 2, 11);
            GivenPlayerVerticalGo("DAY", 3, 11);

            WhenFindWords();
            AssertWordsFound("DAY", "TIED");
        }

        [TestMethod]
        public void GivenAGoUsesABonusSquare_ThenGoWordContainsTheBonusSquare()
        {
            GivenBonusTile(BoardSquare.DoubleLetterSquare(), 3, 7);
            GivenPlayerHorizontalGo("BONUS", 3, 7);
            WhenFindWords();
            foundWords.First().GoLetters.First().LetterBonus.ShouldBe(2);
        }

        [TestMethod]
        public void GivenAGoUsesABonusSquare_ThenSideWordContainsTheBonusSquare()
        {
            GivenBonusTile(BoardSquare.DoubleWordSquare(), 4, 7);
            GivenBoardTile('N', 4, 8);
            GivenPlayerHorizontalGo("BONUS", 3, 7);
            WhenFindWords();
            foundWords.First(go => go.Word == "ON").GoLetters.First().WordBonus.ShouldBe(2);
        }
    }
}
