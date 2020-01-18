using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Scrabble.Ai;
using Scrabble.Go;
using Scrabble.Play;
using Scrabble.Value;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using words;

namespace Scrabble.Tests
{
    [TestClass]
    public class AiGoHandlerTests
    {
        AiGoHandler aiGoHandler;
        Game game;
        IAiGridModel aiGridModel;
        List<AiCandidate> candidates;
        List<Tile> playerTiles;
        WordFindable wordFinder;
        IAiGoWordFinder goWordFinder;
        WordValidatable wordValidator;
        IGoScorer goScorer;
        IAiGoPlacer goPlacer;
        GoResult goResult;

        [TestInitialize]
        public void SetUp()
        {
            game = new GameFactory(Substitute.For<IDateTimeOffset>()).NewGame();
            game.Players[0].Tiles.Add(new Tile() { Letter = 'A' });

            aiGridModel = Substitute.For<IAiGridModel>();
            candidates = new List<AiCandidate>();
            playerTiles = new List<Tile>();
            aiGridModel.Candidates.Returns(candidates);
            aiGridModel.PlayerTiles.Returns(playerTiles);

            wordFinder = Substitute.For<WordFindable>();
            goWordFinder = Substitute.For<IAiGoWordFinder>();
            wordValidator = Substitute.For<WordValidatable>();
            goScorer = Substitute.For<IGoScorer>();
            goPlacer = Substitute.For<IAiGoPlacer>();
            aiGoHandler = new AiGoHandler(aiGridModel, wordFinder, goWordFinder, wordValidator, goScorer, goPlacer);

            goResult = null;
        }

        void GivenCandidateWithPattern(string pattern)
        {
            candidates.Add(new AiCandidate()
            {
                SearchPattern = pattern
            });
        }

        void GivenPlayerTilesPPL()
        {
            playerTiles.Add(new Tile() { Letter = 'P' });
            playerTiles.Add(new Tile() { Letter = 'P' });
            playerTiles.Add(new Tile() { Letter = 'L' });
        }

        void GivenMainWords(params string[] mainWords)
        {
            wordFinder.FindWords(Arg.Any<string>()).Returns(mainWords);
        }

        List<GoWord> GivenGoWordsForMainWord(string mainWord, params string[] goWordsList)
        {
            var goWordsReturn = new List<GoWord>();
            foreach (var word in goWordsList)
            {
                goWordsReturn.Add(new GoWord() { Word = word });
            }

            goWordFinder.FindWords(mainWord, Arg.Any<AiCandidate>()).Returns(goWordsReturn);

            return goWordsReturn;
        }

        void GivenGoWordsForMainWordWithScore(string mainWord, int score, params string[] goWordsList)
        {
            var goWords = GivenGoWordsForMainWord(mainWord, goWordsList);
            goScorer.ScoreGo(goWords).Returns(score);
        }

        void GivenGoWordWithGoLetters()
        {
            var goWords = new List<string> { "a", "b" };
            var goLetters = new List<GoLetter>()
            {
                new GoLetter()
                {
                     TileValue = 3,
                     LetterBonus = 1,
                     WordBonus = 2
                }
            };
            var goWordsReturn = new List<GoWord>();
            foreach (var word in goWords)
            {
                goWordsReturn.Add(new GoWord() { Word = word, GoLetters = goLetters });
            }

            goWordFinder.FindWords(Arg.Any<string>(), Arg.Any<AiCandidate>()).Returns(goWordsReturn);
        }

        void GivenWordsAreInTheDictionary(params string[] validWords)
        {
            foreach (var validWord in validWords)
            {
                wordValidator.IsWord(validWord).Returns(true);
            }
        }

        void GivenWordsAllInTheDictionary()
        {
            wordValidator.IsWord(Arg.Any<string>()).Returns(true);
        }

        void WhenHandleGo()
        {
            goResult = aiGoHandler.Go(game);
        }

        [TestMethod]
        public void GivenNoTiles_ThenCannotMakeAnyMove()
        {
            game.Players[0].Tiles.Clear();
            WhenHandleGo();
            goResult.Message.ShouldBe("Player does not have any tiles.");
        }

        [TestMethod]
        public void GivenBoardTiles_ThenShouldCallBuildOnAiGridModel()
        {
            WhenHandleGo();
            aiGridModel.Received().Build(Arg.Any<List<Tile>>(), Arg.Any<List<Tile>>());
        }

        [TestMethod]
        public void GivenTilePlacementCandidates_ThenGoHandlerShouldCallWordFinder()
        {
            GivenCandidateWithPattern("a???e");
            WhenHandleGo();
            wordFinder.Received().FindWords("a???e");
        }

        [TestMethod]
        public void GivenTilePlacementCandidates_AndPlayerTiles_ThenGoHandlerShouldCallWordFinderWithLetterRestriction()
        {
            GivenCandidateWithPattern("a???e");
            GivenPlayerTilesPPL();
            WhenHandleGo();
            wordFinder.Received().FindWords("a???e", "ppl");
        }

        [TestMethod]
        public void GivenAMainWordIsFound_ThenGoWordFinderIsCalledWithMainWord()
        {
            GivenCandidateWithPattern("a???e");
            GivenMainWords("apple");
            WhenHandleGo();
            goWordFinder.Received().FindWords("apple", Arg.Any<AiCandidate>());
        }

        [TestMethod]
        public void GivenMultipleMainWordsAreFound_ThenWordFinderIsCalledWithAllMainWords()
        {
            GivenCandidateWithPattern("a???e");
            GivenMainWords("apple", "apply");
            WhenHandleGo();
            goWordFinder.Received().FindWords("apple", Arg.Any<AiCandidate>());
            goWordFinder.Received().FindWords("apply", Arg.Any<AiCandidate>());
        }

        [TestMethod]
        public void GivenGoWordsAreFound_ThenWordValidatorIsCalledWithAllGoWords()
        {
            GivenCandidateWithPattern("a???e");
            GivenMainWords("apple");
            GivenGoWordsForMainWord("apple", "apple", "ape", "eel");
            GivenWordsAreInTheDictionary("apple", "ape", "eel");
            WhenHandleGo();
            wordValidator.Received().IsWord("apple");
            wordValidator.Received().IsWord("ape");
            wordValidator.Received().IsWord("eel");
        }

        [TestMethod]
        public void GivenAllGoWordsAreValidated_ThenValidGoIsSaved()
        {
            GivenCandidateWithPattern("a???e");
            GivenMainWords("apple");
            GivenGoWordsForMainWord("apple", "apple", "ape", "eel");
            GivenWordsAreInTheDictionary("apple", "ape", "eel");
            WhenHandleGo();
            var go = aiGoHandler.ValidGoes.First(g => g.MainWord == "APPLE");
            go.Candidate.SearchPattern.ShouldBe("a???e");
        }

        [TestMethod]
        public void GivenOneGoWordIsInvalid_ThenGoIsDiscarded()
        {
            GivenCandidateWithPattern("a???e");
            GivenMainWords("apple");
            GivenGoWordsForMainWord("apple", "apple", "ape", "eel");
            GivenWordsAreInTheDictionary("apple", "ape");
            WhenHandleGo();
            aiGoHandler.ValidGoes.Count().ShouldBe(0);
        }

        [TestMethod]
        public void GivenValidWords_ThenSavedGoContainsGoWords()
        {
            GivenCandidateWithPattern("a");
            GivenMainWords("a");
            GivenGoWordWithGoLetters();
            GivenWordsAllInTheDictionary();
            WhenHandleGo();
            aiGoHandler.ValidGoes.First().GoWords.ShouldNotBeNull();
        }

        [TestMethod]
        public void GivenValidWords_ThenGoWordScorerShouldBeCalled()
        {
            GivenCandidateWithPattern("a");
            GivenMainWords("a");
            GivenGoWordWithGoLetters();
            GivenWordsAllInTheDictionary();
            WhenHandleGo();
            goScorer.Received().ScoreGo(Arg.Any<IEnumerable<GoWord>>());
            aiGoHandler.ValidGoes.First();
        }

        [TestMethod]
        public void GivenTwoValidGoes_ThenHighestScoredGoIsUsed()
        {
            GivenCandidateWithPattern("a???e");
            GivenMainWords("apple", "archbishop");
            GivenGoWordsForMainWordWithScore("apple", 22, "apple");
            GivenGoWordsForMainWordWithScore("archbishop", 19, "archbishop");
            GivenWordsAreInTheDictionary("apple", "archbishop");
            WhenHandleGo();
            goPlacer.Received().PlaceGo(Arg.Is<AiValidGo>(g => g.MainWord == "APPLE"), game);
        }
    }
}
