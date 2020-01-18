using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrabble.Go;
using Scrabble.Value;
using Shouldly;
using System.Collections.Generic;

namespace Scrabble.Tests
{
    [TestClass]
    public class GoScorerTests
    {
        GoScorer goScorer;
        List<GoWord> goWords;

        [TestInitialize]
        public void Setup()
        {
            goScorer = new GoScorer();
            goWords = new List<GoWord>();
        }

        public void GivenAWord()
        {
            goWords.Add(new GoWord()
            {
                GoLetters = new List<GoLetter>()
                {
                     new GoLetter() { LetterBonus = 1, WordBonus = 1, TileValue = 1 },
                     new GoLetter() { LetterBonus = 1, WordBonus = 1, TileValue = 1 },
                     new GoLetter() { LetterBonus = 1, WordBonus = 1, TileValue = 1 }
                }
            });
        }

        void GivenALetterBonusWord()
        {
            goWords.Add(new GoWord()
            {
                GoLetters = new List<GoLetter>()
                {
                     new GoLetter() { LetterBonus = 2, WordBonus = 1, TileValue = 1 },
                     new GoLetter() { LetterBonus = 1, WordBonus = 1, TileValue = 2 },
                     new GoLetter() { LetterBonus = 1, WordBonus = 1, TileValue = 3 }
                }
            });
        }

        void GivenAWordThatUsesAWordBonus()
        {
            goWords.Add(new GoWord()
            {
                GoLetters = new List<GoLetter>()
                {
                     new GoLetter() { LetterBonus = 1, WordBonus = 1, TileValue = 1 },
                     new GoLetter() { LetterBonus = 1, WordBonus = 3, TileValue = 2 },
                     new GoLetter() { LetterBonus = 1, WordBonus = 1, TileValue = 3 }
                }
            });
        }

        [TestMethod]
        public void GivenNoTiles_ThenScorerReturnsZeroScore()
        {
            goScorer.ScoreGo(goWords).ShouldBe(0);
        }

        [TestMethod]
        public void GivenASingleWord_ThenScorerCountsMainWord()
        {
            GivenAWord();

            goScorer.ScoreGo(goWords).ShouldBe(3);
        }

        [TestMethod]
        public void GivenTwoWords_ThenScorerCountsAllWords()
        {
            GivenAWord();
            GivenAWord();

            goScorer.ScoreGo(goWords).ShouldBe(6);
        }

        [TestMethod]
        public void GivenAWordWithALetterBonus_ThenScorerCountsLetterBonus()
        {
            GivenALetterBonusWord();

            goScorer.ScoreGo(goWords).ShouldBe(7);
        }

        [TestMethod]
        public void GivenAWordWithAWordBonus_ThenScorerCountsWordBonus()
        {
            GivenAWordThatUsesAWordBonus();

            goScorer.ScoreGo(goWords).ShouldBe(18);
        }
    }
}
