using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Scrabble.Play;
using Scrabble.Value;
using Shouldly;
using System;
using System.Linq;

namespace Scrabble.Tests
{
    [TestClass]
    public class GameTests
    {
        GameFactory gameFactory;
        Game game;
        DateTimeOffset timeNow = DateTimeOffset.Now;

        [TestInitialize]
        public void SetUp()
        {
            IDateTimeOffset dateTimeOffset = Substitute.For<IDateTimeOffset>();
            dateTimeOffset.Now().Returns(timeNow);
            gameFactory = new GameFactory(dateTimeOffset);
            game = gameFactory.NewGame();
        }

        [TestMethod]
        public void GivenANewGame_ThenItShouldHaveTheCorrectTileCounts()
        {
            game.BagTiles.Where(t => t.Letter == ' ').Count().ShouldBe(2, "should be 2 blanks");
            game.BagTiles.Where(t => t.Letter == 'E').Count().ShouldBe(12, "should be 12 E's");
            game.BagTiles.Where(t => t.Letter == 'A').Count().ShouldBe(9, "should be 9 A's");
            game.BagTiles.Where(t => t.Letter == 'I').Count().ShouldBe(9, "should be 9 I's");
            game.BagTiles.Where(t => t.Letter == 'O').Count().ShouldBe(8, "should be 8 O's");
            game.BagTiles.Where(t => t.Letter == 'N').Count().ShouldBe(6, "should be 6 N's");
            game.BagTiles.Where(t => t.Letter == 'R').Count().ShouldBe(6, "should be 6 R's");
            game.BagTiles.Where(t => t.Letter == 'T').Count().ShouldBe(6, "should be 6 T's");
            game.BagTiles.Where(t => t.Letter == 'L').Count().ShouldBe(4, "should be 4 L's");
            game.BagTiles.Where(t => t.Letter == 'S').Count().ShouldBe(4, "should be 4 S's");
            game.BagTiles.Where(t => t.Letter == 'U').Count().ShouldBe(4, "should be 4 U's");
            game.BagTiles.Where(t => t.Letter == 'D').Count().ShouldBe(4, "should be 4 D's");
            game.BagTiles.Where(t => t.Letter == 'G').Count().ShouldBe(3, "should be 3 G's");
            game.BagTiles.Where(t => t.Letter == 'B').Count().ShouldBe(2, "should be 2 B's");
            game.BagTiles.Where(t => t.Letter == 'C').Count().ShouldBe(2, "should be 2 C's");
            game.BagTiles.Where(t => t.Letter == 'M').Count().ShouldBe(2, "should be 2 M's");
            game.BagTiles.Where(t => t.Letter == 'P').Count().ShouldBe(2, "should be 2 P's");
            game.BagTiles.Where(t => t.Letter == 'F').Count().ShouldBe(2, "should be 2 F's");
            game.BagTiles.Where(t => t.Letter == 'H').Count().ShouldBe(2, "should be 2 H's");
            game.BagTiles.Where(t => t.Letter == 'V').Count().ShouldBe(2, "should be 2 V's");
            game.BagTiles.Where(t => t.Letter == 'W').Count().ShouldBe(2, "should be 2 W's");
            game.BagTiles.Where(t => t.Letter == 'Y').Count().ShouldBe(2, "should be 2 Y's");
            game.BagTiles.Where(t => t.Letter == 'K').Count().ShouldBe(1, "should be 1 K's");
            game.BagTiles.Where(t => t.Letter == 'J').Count().ShouldBe(1, "should be 1 J's");
            game.BagTiles.Where(t => t.Letter == 'X').Count().ShouldBe(1, "should be 1 X's");
            game.BagTiles.Where(t => t.Letter == 'Q').Count().ShouldBe(1, "should be 1 Q's");
            game.BagTiles.Where(t => t.Letter == 'Z').Count().ShouldBe(1, "should be 1 Z's");
        }

        [TestMethod]
        public void GivenANewGame_ThenEachPlayerShouldHaveNoTiles()
        {
            game.CurrentPlayer().Tiles.Count().ShouldBe(0);
        }

        [TestMethod]
        public void BlankTilesShouldHaveIsBlankProperty()
        {
            var blanks = game.BagTiles.Where(t => t.Letter == ' ');
            blanks.First().IsBlank.ShouldBeTrue();
        }

        [TestMethod]
        public void GivenANewGame_ThenItShouldHaveTwoPlayers()
        {
            game.Players.Count().ShouldBe(2);
            game.Players[0].Name.ShouldBe("Player");
            game.Players[1].Name.ShouldBe("Scrabble Bot");
        }

        [TestMethod]
        public void GivenANewGame_ThenItShouldHaveAnId()
        {
            game.Id.ShouldNotBe(Guid.Empty);
        }

        [TestMethod]
        public void GivenANewGame_ThenItShouldHaveAStartTimeOfNow()
        {
            game.StartTime.ShouldBe(timeNow);
        }

        [TestMethod]
        public void GivenANewGame_ThenLastActiveTimeShouldBeNow()
        {
            game.LastActiveTime.ShouldBe(timeNow);
        }
    }
}
