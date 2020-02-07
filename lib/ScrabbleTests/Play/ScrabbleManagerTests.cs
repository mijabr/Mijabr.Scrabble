using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Scrabble.Ai;
using Scrabble.Draw;
using Scrabble.Go;
using Scrabble.Persist;
using Scrabble.Play;
using Scrabble.Value;

namespace Scrabble.Tests
{
    [TestClass]
    public class ScrabbleManagerTests
    {
        IGameFactory gameFactory;
        Board board;
        ITileDrawer drawer;
        IGoHandler goHandler;
        IAiGoHandler aiGoHandler;
        IGameRepository gameList;
        ScrabbleManager manager;

        [TestInitialize]
        public void CanCreateScrabbleManager()
        {
            gameFactory = Substitute.For<IGameFactory>();
            board = new Board();
            drawer = Substitute.For<ITileDrawer>();
            goHandler = Substitute.For<IGoHandler>();
            aiGoHandler = Substitute.For<IAiGoHandler>();
            gameList = Substitute.For<IGameRepository>();
            manager = new ScrabbleManager(gameFactory, board, drawer, goHandler, aiGoHandler, gameList);
        }

        [TestMethod]
        public void CanGetBoardSquaresFromBoard()
        {
            manager.GetSquares();
        }

        [TestMethod]
        public void WhenCreatingANewGame_ThenDrawTilesForAllPlayers()
        {
            var game = manager.NewGame("Player");
            drawer.Received().DrawTilesForAllPlayers(game);
        }

        [TestMethod]
        public void WhenCreatingANewGame_ThenGameShouldBeSavedToGameList()
        {
            var game = manager.NewGame("Player");
            gameList.Received().Set(game);
        }

        [TestMethod]
        public void GivenAValidGo_WhenGoIsSubmitted_ThenGameIsSavedToList()
        {
            var game = new Game();
            goHandler.Go(game).Returns(new GoResult() { IsValid = true, Game = game });
            manager.SubmitGo(game);
            gameList.Received().Set(game);
        }

        [TestMethod]
        public void GivenAnInvalidGo_WhenGoIsSubmitted_ThenGameIsNotSavedToList()
        {
            var game = new Game();
            goHandler.Go(game).Returns(new GoResult() { IsValid = false, Game = game });
            manager.SubmitGo(game);
            gameList.DidNotReceive().Set(game);
        }

        [TestMethod]
        public void CanRequestAiGoForGame()
        {
            var game = new Game();
            manager.AiGo(game);
            aiGoHandler.Received().Go(game);
        }

        [TestMethod]
        public void CanRequestGameShortList()
        {
            manager.ShortList();
            gameList.Received().GetShortList();
        }
    }
}
