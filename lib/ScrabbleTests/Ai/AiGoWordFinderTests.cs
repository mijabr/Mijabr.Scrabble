using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Scrabble.Ai;
using Scrabble.Value;
using Shouldly;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Tests
{
    [TestClass]
    public class AiGoWordFinderTests
    {
        AiGridModelTile[,] grid;
        IAiGridModel gridModel;
        Board board;
        AiCandidate candidate;
        AiGoWordFinder wordFinder;
        IEnumerable<GoWord> goWords;

        [TestInitialize]
        public void SetUp()
        {
            gridModel = Substitute.For<IAiGridModel>();
            grid = new AiGridModelTile[15, 15];
            gridModel.Grid.Returns(grid);
            board = Substitute.ForPartsOf<Board>();
            candidate = new AiCandidate();
            wordFinder = new AiGoWordFinder(gridModel, board);
            goWords = null;
        }

        void GivenEmptyCandidate()
        {
            candidate = new AiCandidate();
        }

        void GivenSingleLetterCandidate()
        {
            candidate = new AiCandidate()
            {
                Orientation = 0,
                SearchPattern = "?",
                StartX = 7,
                StartY = 7,
                TilesUsed = 1
            };
        }

        void GivenTwoLetterCandidateUsingStartingBoardTile()
        {
            candidate = new AiCandidate()
            {
                Orientation = 0,
                SearchPattern = "a?",
                StartX = 7,
                StartY = 7,
                TilesUsed = 2
            };

            grid[7, 7].Letter = 'a';
        }

        void GivenOutsideAppleCandidate()
        {
            candidate = new AiCandidate
            {
                SearchPattern = "?ppl?",
                Orientation = 0,
                StartX = 7,
                StartY = 7
            };
        }

        void GivenInsideAppleCandidate()
        {
            candidate = new AiCandidate
            {
                SearchPattern = "a???e",
                Orientation = 0,
                StartX = 7,
                StartY = 7
            };
        }

        void GivenABoardLetterThatMakesASideWord()
        {
            grid[7, 8].Letter = 't';
        }

        void GivenBoardLettersThatMakeSideWordsArtAndApe()
        {
            grid[7, 8].Letter = 'r';
            grid[7, 9].Letter = 't';

            grid[11, 5].Letter = 'a';
            grid[11, 6].Letter = 'p';
        }

        void WhenFindWords(string mainWord)
        {
            goWords = wordFinder.FindWords(mainWord, candidate);
        }

        [TestMethod]
        public void GivenNullMainWord_AndNoCandidate_ThenZeroGoWordsReturned()
        {
            GivenEmptyCandidate();
            WhenFindWords(null);
            goWords.Count().ShouldBe(0);
        }

        [TestMethod]
        public void GivenEmptyMainWord_AndNoCandidate_ThenZeroGoWordsReturned()
        {
            GivenEmptyCandidate();
            WhenFindWords(string.Empty);
            goWords.Count().ShouldBe(0);
        }

        [TestMethod]
        public void GivenMainWord_AndSingleLetterCandidate_ThenMainWordIsReturned()
        {
            GivenSingleLetterCandidate();
            WhenFindWords("a");
            goWords.Count().ShouldBe(1);
            goWords.First().Word.ShouldBe("a");
        }

        [TestMethod]
        public void GivenMainWord_AndSideWords_ThenAllAreReturned()
        {
            GivenSingleLetterCandidate();
            GivenABoardLetterThatMakesASideWord();
            WhenFindWords("a");
            goWords.Count().ShouldBe(2);
            goWords.ShouldContain(g => g.Word == "at");
            goWords.ShouldContain(g => g.Word == "a");
        }

        [TestMethod]
        public void GivenMainWord_AndMultipleSideWords_ThenAllAreReturned()
        {
            GivenOutsideAppleCandidate();
            GivenBoardLettersThatMakeSideWordsArtAndApe();
            WhenFindWords("apple");
            goWords.Count().ShouldBe(3);
            goWords.ShouldContain(g => g.Word == "apple");
            goWords.ShouldContain(g => g.Word == "art");
            goWords.ShouldContain(g => g.Word == "ape");
        }

        [TestMethod]
        public void GivenMainWord_AndSideWordsNotAttachedToPlayerLetters_ThenOnlyMainWordIsReturned()
        {
            GivenInsideAppleCandidate();
            GivenBoardLettersThatMakeSideWordsArtAndApe();
            WhenFindWords("apple");
            goWords.Count().ShouldBe(1);
            goWords.ShouldContain(g => g.Word == "apple");
        }

        [TestMethod]
        public void GivenMainWord_ThenGoLettersShouldBeSet()
        {
            GivenSingleLetterCandidate();
            WhenFindWords("a");
            goWords.First().GoLetters.First().LetterBonus.ShouldBe(1);
        }

        [TestMethod]
        public void GivenMainWordUsingPlayerLetters_ThenBonusesShouldComeFromTheBoard()
        {
            board.GetSquare(8, 7).Returns(BoardSquare.TrippleWordSquare());
            GivenTwoLetterCandidateUsingStartingBoardTile();
            WhenFindWords("at");
            var goLetters = goWords.First().GoLetters;
            goLetters.Count().ShouldBe(2);
            var goLetterT = goLetters.Last();
            goLetterT.WordBonus.ShouldBe(3);
        }

        [TestMethod]
        public void GivenMainWordUsingBoardLetters_ThenBonusesAreAllreadyUsed()
        {
            board.GetSquare(7, 7).Returns(BoardSquare.TrippleWordSquare());
            GivenTwoLetterCandidateUsingStartingBoardTile();
            WhenFindWords("at");
            var goLetters = goWords.First().GoLetters;
            goLetters.Count().ShouldBe(2);
            var goLetterA = goLetters.First();
            goLetterA.WordBonus.ShouldBe(1);
        }

        [TestMethod]
        public void GivenMainWord_AndSideWords_ThenBonusesAreAllreadyUsedForSideWords()
        {
            GivenTwoLetterCandidateUsingStartingBoardTile();
            grid[8, 6].Letter = 'i';
            board.GetSquare(8, 6).Returns(BoardSquare.TrippleWordSquare());
            WhenFindWords("at");
            var sideGoLetters = goWords.First(w => w.Word == "it").GoLetters;
            sideGoLetters.Count().ShouldBe(2);
            var goLetterI = sideGoLetters.First();
            goLetterI.WordBonus.ShouldBe(1);
        }

        [TestMethod]
        public void GivenMainWord_ThenGoWordShouldHavePlayerTileValue()
        {
            gridModel.PlayerTiles.Returns(new List<Tile> { new Tile() { Letter = 'a', Value = 3 } });
            GivenSingleLetterCandidate();
            WhenFindWords("a");
            var goLetters = goWords.First().GoLetters;
            goLetters.Count().ShouldBe(1);
            var goLetter = goLetters.Last();
            goLetter.TileValue.ShouldBe(3);
        }

        [TestMethod]
        public void GivenMainWordUsingBoardLetters_ThenGoWordShouldHaveBoardTileValue()
        {
            GivenTwoLetterCandidateUsingStartingBoardTile();
            grid[7, 7].TileValue = 3;
            WhenFindWords("at");
            var goLetters = goWords.First().GoLetters;
            goLetters.Count().ShouldBe(2);
            var goLetter = goLetters.First();
            goLetter.TileValue.ShouldBe(3);
        }
    }
}
