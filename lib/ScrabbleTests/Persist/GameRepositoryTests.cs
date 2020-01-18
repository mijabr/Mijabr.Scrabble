using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Scrabble.Persist;
using Scrabble.Play;
using Scrabble.Value;
using Shouldly;
using System;
using System.Collections.Generic;

namespace Scrabble.Tests
{
    [TestClass]
    public class GameRepositoryTests
    {
        IDateTimeOffset dateTimeOffset;
        GameFactory gameFactory;
        GameRepository gameRepo;
        List<ShortGame> shortList;

        [TestInitialize]
        public void SetUp()
        {
            dateTimeOffset = Substitute.For<IDateTimeOffset>();
            gameFactory = new GameFactory(dateTimeOffset);
            gameRepo = new GameRepository();
        }

        Game AddNewGame(int score1 = 0, int score2 = 0)
        {
            var g = gameFactory.NewGame();
            g.Players[0].Name = "P1";
            g.Players[0].Score = score1;
            g.Players[1].Name = "P2";
            g.Players[1].Score = score2;
            gameRepo.Set(g);
            return g;
        }

        void WhenGetShortList()
        {
            shortList = gameRepo.GetShortList();
        }

        void AssertShortGamePlayerNamesAndScores(int game, string name1, int score1, string name2, int score2)
        {
            shortList[game].Id.ShouldNotBe(Guid.Empty);
            shortList[game].Player[0].Name.ShouldBe(name1);
            shortList[game].Player[0].Score.ShouldBe(score1);
            shortList[game].Player[1].Name.ShouldBe(name2);
            shortList[game].Player[1].Score.ShouldBe(score2);
        }

        [TestMethod]
        public void GivenGamesInTheRepo_ThenCanRetrieveGameById()
        {
            AddNewGame();
            var id = AddNewGame().Id;
            AddNewGame();
            AddNewGame();
            var game = gameRepo.GetById(id);
            game.Id.ShouldBe(id);
        }

        [TestMethod]
        public void GivenGamesInTheRepo_ThenCanGetGamesShortList()
        {
            AddNewGame(10, 20);
            AddNewGame(11, 22);
            AddNewGame(25, 18);
            WhenGetShortList();
            AssertShortGamePlayerNamesAndScores(0, "P1", 10, "P2", 20);
            AssertShortGamePlayerNamesAndScores(1, "P1", 11, "P2", 22);
            AssertShortGamePlayerNamesAndScores(2, "P1", 25, "P2", 18);
        }

        [TestMethod]
        public void GivenGamesInTheRepo_ThenCanGetStartTime()
        {
            var now = new DateTimeOffset(2018, 3, 14, 22, 5, 1, new TimeSpan(10, 0, 0));
            dateTimeOffset.Now().Returns(now);
            var game = AddNewGame();
            WhenGetShortList();
            shortList[0].StartTime.ShouldBe(now);
        }

        [TestMethod]
        public void GivenGamesInTheRepo_ThenCanGetLastActiveTime()
        {
            var now = new DateTimeOffset(2018, 3, 14, 22, 5, 1, new TimeSpan(10, 0, 0));
            dateTimeOffset.Now().Returns(now);
            var game = AddNewGame();
            WhenGetShortList();
            shortList[0].LastActiveTime.ShouldBe(now);
        }
    }
}
