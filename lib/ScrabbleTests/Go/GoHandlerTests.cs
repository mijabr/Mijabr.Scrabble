using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Scrabble.Draw;
using Scrabble.Go;
using Scrabble.Play;
using Scrabble.Value;
using Shouldly;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Tests
{
    [TestClass]
    public class GoHandlerTests
    {
        Game game;
        IGoValidator goValidator;
        ITileDrawer drawer;
        List<GoWord> goWords;
        IGoWordFinder goWordFinder;
        IGoWordValidator goWordValidator;
        IGoScorer goScorer;
        IGoMessageMaker goMessageMaker;
        GoHandler goHandler;

        [TestInitialize]
        public void Setup()
        {
            game = new GameFactory(Substitute.For<IDateTimeOffset>()).NewGame();
            goValidator = Substitute.For<IGoValidator>();
            drawer = Substitute.For<ITileDrawer>();
            goWordFinder = Substitute.For<IGoWordFinder>();
            goWordValidator = Substitute.For<IGoWordValidator>();
            goScorer = Substitute.For<IGoScorer>();
            goMessageMaker = Substitute.For<IGoMessageMaker>();
            goHandler = new GoHandler(goValidator, drawer, goWordFinder, goWordValidator, goScorer, goMessageMaker);
            var goWordValidatorResult = new GoValidationResult() { IsValid = true };
            goWordValidator.ValidateWords(Arg.Any<IEnumerable<GoWord>>()).Returns(goWordValidatorResult);
        }

        void GivenAnInvalidGo()
        {
            var validationResult = new GoValidationResult() { IsValid = false, Message = "go is invalid" };
            goValidator.ValidateGo(game).Returns(validationResult);
        }

        void GivenAValidGo()
        {
            var validationResult = new GoValidationResult() { IsValid = true, Message = "go is valid" };
            goValidator.ValidateGo(game).Returns(validationResult);
        }

        void GivenAValidGoWithAPlayerTilePlacedOnBoard()
        {
            game.CurrentPlayer().Tiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            var validationResult = new GoValidationResult() { IsValid = true };
            goValidator.ValidateGo(game).Returns(validationResult);
        }

        void GivenAValidGoWhichHasCreatedSomeWords()
        {
            var validationResult = new GoValidationResult() { IsValid = true };
            goValidator.ValidateGo(game).Returns(validationResult);
            goWords = new List<GoWord>()
            {
                new GoWord() { Word = "GOOD" },
                new GoWord() { Word = "BETTER" }
            };
            goWordFinder.FindWords().Returns(goWords);
        }

        void GivenAValidGoWhichHasCreatedSomeInvalidWords()
        {
            var validationResult = new GoValidationResult() { IsValid = true };
            goValidator.ValidateGo(game).Returns(validationResult);
            goWords = new List<GoWord>()
            {
                new GoWord() { Word = "BETTEREST" }
            };
            goWordFinder.FindWords().Returns(goWords);
            var goWordValidatorResult = new GoValidationResult() { IsValid = false, Message = "BETTEREST is not a word" };
            goWordValidator.ValidateWords(goWords).Returns(goWordValidatorResult);
        }

        void GivenAGoScored12Points()
        {
            var validationResult = new GoValidationResult() { IsValid = true };
            goValidator.ValidateGo(game).Returns(validationResult);
            goScorer.ScoreGo(Arg.Any<IEnumerable<GoWord>>()).Returns(12);
        }

        void GivenPlayerHasNoMoreTiles()
        {
            var validationResult = new GoValidationResult() { IsValid = true };
            goValidator.ValidateGo(game).Returns(validationResult);
        }

        void GivenAllPlayersHaveTilesLeft()
        {
            var validationResult = new GoValidationResult() { IsValid = true };
            goValidator.ValidateGo(game).Returns(validationResult);
            game.Players.ForEach(player =>
            {
                player.Tiles.Add(new Tile());
            });
        }

        void GivenAnotherPlayerHasNoTiles()
        {
            var validationResult = new GoValidationResult() { IsValid = true };
            goValidator.ValidateGo(game).Returns(validationResult);
            game.CurrentPlayer().Tiles.Add(new Tile());
        }

        [TestMethod]
        public void GivenAnInvalidGo_ThenMessageIsReturned()
        {
            GivenAnInvalidGo();
            var result = goHandler.Go(game);
            result.IsValid.ShouldBeFalse();
            result.Message.ShouldBe("go is invalid");
        }

        [TestMethod]
        public void GivenAValidGo_ThenBoardShouldBeUpdatedWithPlayersTiles()
        {
            GivenAValidGoWithAPlayerTilePlacedOnBoard();
            var result = goHandler.Go(game);
            result.Game.BoardTiles.First(t => t.BoardPositionX == 7 && t.BoardPositionY == 7).Letter.ShouldBe('A');
            result.Game.CurrentPlayer().Tiles.FirstOrDefault(t => t.Location == "board").Letter.ShouldBe((char)0);
            result.Game.BagTiles.Count().ShouldBe(100);
        }

        [TestMethod]
        public void GivenAValidGo_ThenPlayerShouldDrawMoreTiles()
        {
            GivenAValidGoWithAPlayerTilePlacedOnBoard();
            var result = goHandler.Go(game);
            drawer.Received().DrawTilesForPlayer(game);
        }

        [TestMethod]
        public void GivenAValidGo_ThenMessageComeFromGoMessageMaker()
        {
            GivenAValidGoWhichHasCreatedSomeWords();
            var result = goHandler.Go(game);
            goMessageMaker.GetGoMessage("NAME", Arg.Any<IEnumerable<GoWord>>(), Arg.Any<int>());
        }

        [TestMethod]
        public void GivenAValidGo_ThenHandlerShouldCheckForInvalidGoWords()
        {
            GivenAValidGoWhichHasCreatedSomeWords();
            var result = goHandler.Go(game);
            goWordValidator.Received(1).ValidateWords(Arg.Any<IEnumerable<GoWord>>());
        }

        [TestMethod]
        public void GivenAGoWithInvalidWords_ThenGoShouldBeInvalid()
        {
            GivenAValidGoWhichHasCreatedSomeInvalidWords();
            var result = goHandler.Go(game);
            result.IsValid.ShouldBeFalse();
        }

        [TestMethod]
        public void GivenAGoWithInvalidWords_ThenGoWordValidatorMessageShouldBeReturned()
        {
            GivenAValidGoWhichHasCreatedSomeInvalidWords();
            var result = goHandler.Go(game);
            result.Message.ShouldBe("BETTEREST is not a word");
        }

        [TestMethod]
        public void GivenAValidGo_ThenItShouldBeNextPlayersTurn()
        {
            GivenAValidGo();
            var result = goHandler.Go(game);
            result.Game.PlayerTurn.ShouldBe(1);
        }

        [TestMethod]
        public void GivenTheLastPlayerHasATurn_ThenItShouldBeTheFirstPlayersTurn()
        {
            game.PlayerTurn = 1;
            GivenAValidGo();
            var result = goHandler.Go(game);
            result.Game.PlayerTurn.ShouldBe(0);
        }

        [TestMethod]
        public void GivenPlayerAlreadyHasAScore_ThenGoScoreShouldBeAdded()
        {
            game.Players[0].Score = 20;
            GivenAGoScored12Points();
            var result = goHandler.Go(game);
            game.Players[0].Score.ShouldBe(32);
        }

        [TestMethod]
        public void GivenAllPlayersHaveTilesLeft_ThenTheGameShouldContinue()
        {
            GivenAllPlayersHaveTilesLeft();
            var result = goHandler.Go(game);
            game.IsFinished.ShouldBeFalse();
        }

        [TestMethod]
        public void GivenPlayerHasNoMoreTiles_ThenTheGameShouldEnd()
        {
            GivenPlayerHasNoMoreTiles();
            var result = goHandler.Go(game);
            game.IsFinished.ShouldBeTrue();
        }

        [TestMethod]
        public void GivenAnotherPlayerHasNoTiles_ThenTheGameShouldEnd()
        {
            GivenAnotherPlayerHasNoTiles();
            var result = goHandler.Go(game);
            game.IsFinished.ShouldBeTrue();
        }
    }
}
