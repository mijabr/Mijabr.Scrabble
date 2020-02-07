using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Scrabble.Play;
using Scrabble.Value;
using Shouldly;
using System;
using System.Linq;

namespace ScrabbleTests.Value
{
    [TestClass]
    public class GameTests
    {
        private GameFactory gameFactory;
        private Game game;
        private DateTimeOffset timeNow;

        [TestInitialize]
        public void SetUp()
        {
            timeNow = DateTimeOffset.Now;
            var dateTimeOffset = Substitute.For<IDateTimeOffset>();
            dateTimeOffset.Now().Returns(timeNow);
            gameFactory = new GameFactory(dateTimeOffset);
            game = gameFactory.NewGame("Player");
        }

        [TestMethod]
        public void GivenANewGame_ThenItShouldHaveTheCorrectTileCounts()
        {
            game.BagTiles.Count(t => t.Letter == ' ').ShouldBe(2, "should be 2 blanks");
            game.BagTiles.Count(t => t.Letter == 'E').ShouldBe(12, "should be 12 E's");
            game.BagTiles.Count(t => t.Letter == 'A').ShouldBe(9, "should be 9 A's");
            game.BagTiles.Count(t => t.Letter == 'I').ShouldBe(9, "should be 9 I's");
            game.BagTiles.Count(t => t.Letter == 'O').ShouldBe(8, "should be 8 O's");
            game.BagTiles.Count(t => t.Letter == 'N').ShouldBe(6, "should be 6 N's");
            game.BagTiles.Count(t => t.Letter == 'R').ShouldBe(6, "should be 6 R's");
            game.BagTiles.Count(t => t.Letter == 'T').ShouldBe(6, "should be 6 T's");
            game.BagTiles.Count(t => t.Letter == 'L').ShouldBe(4, "should be 4 L's");
            game.BagTiles.Count(t => t.Letter == 'S').ShouldBe(4, "should be 4 S's");
            game.BagTiles.Count(t => t.Letter == 'U').ShouldBe(4, "should be 4 U's");
            game.BagTiles.Count(t => t.Letter == 'D').ShouldBe(4, "should be 4 D's");
            game.BagTiles.Count(t => t.Letter == 'G').ShouldBe(3, "should be 3 G's");
            game.BagTiles.Count(t => t.Letter == 'B').ShouldBe(2, "should be 2 B's");
            game.BagTiles.Count(t => t.Letter == 'C').ShouldBe(2, "should be 2 C's");
            game.BagTiles.Count(t => t.Letter == 'M').ShouldBe(2, "should be 2 M's");
            game.BagTiles.Count(t => t.Letter == 'P').ShouldBe(2, "should be 2 P's");
            game.BagTiles.Count(t => t.Letter == 'F').ShouldBe(2, "should be 2 F's");
            game.BagTiles.Count(t => t.Letter == 'H').ShouldBe(2, "should be 2 H's");
            game.BagTiles.Count(t => t.Letter == 'V').ShouldBe(2, "should be 2 V's");
            game.BagTiles.Count(t => t.Letter == 'W').ShouldBe(2, "should be 2 W's");
            game.BagTiles.Count(t => t.Letter == 'Y').ShouldBe(2, "should be 2 Y's");
            game.BagTiles.Count(t => t.Letter == 'K').ShouldBe(1, "should be 1 K's");
            game.BagTiles.Count(t => t.Letter == 'J').ShouldBe(1, "should be 1 J's");
            game.BagTiles.Count(t => t.Letter == 'X').ShouldBe(1, "should be 1 X's");
            game.BagTiles.Count(t => t.Letter == 'Q').ShouldBe(1, "should be 1 Q's");
            game.BagTiles.Count(t => t.Letter == 'Z').ShouldBe(1, "should be 1 Z's");
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
