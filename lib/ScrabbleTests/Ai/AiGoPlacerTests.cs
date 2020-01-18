using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Scrabble.Ai;
using Scrabble.Play;
using Scrabble.Value;
using Shouldly;
using System.Linq;

namespace Scrabble.Tests
{
    [TestClass]
    public class AiGoPlacerTests
    {
        AiValidGo go;
        Game game;
        AiGoPlacer placer;

        [TestInitialize]
        public void SetUp()
        {
            placer = new AiGoPlacer();
            game = new GameFactory(Substitute.For<IDateTimeOffset>()).NewGame();
            game.Players[0].Tiles.Clear();
            go = new AiValidGo();
        }

        public void GivenGoWordStartingAt(string goWord, string goWordPattern, int startx, int starty, int orientation = 0)
        {
            GivenGoCandidate(goWord, goWordPattern, startx, starty, orientation);
            GivenPlayerTiles(goWord, goWordPattern, startx, starty);
        }

        public void GivenGoCandidate(string goWord, string goWordPattern, int startx, int starty, int orientation)
        {
            go.Candidate = new AiCandidate()
            {
                StartX = startx,
                StartY = starty,
                SearchPattern = goWordPattern,
                Orientation = orientation
            };
            go.MainWord = goWord;
        }

        void GivenPlayerTiles(string goWord, string goWordPattern, int startx, int starty)
        {
            int position = 0;
            foreach (var letter in goWord)
            {
                if (goWordPattern[position] == '?')
                {
                    game.CurrentPlayer().Tiles.Add(new Tile() { Letter = letter, Location = "tray" });
                }

                position++;
            }
        }

        void GivenPlayerTilesAreNotInTheTray()
        {
            game.CurrentPlayer().Tiles.Add(new Tile() { Letter = 'A', Location = "board" });
            game.CurrentPlayer().Tiles.Add(new Tile() { Letter = 'B', Location = "board" });
            game.CurrentPlayer().Tiles.Add(new Tile() { Letter = 'C', Location = "board" });
        }

        public void WhenPlaceGo()
        {
            placer.PlaceGo(go, game);
        }

        public void ThenPlayerTileShouldBePlacedAt(int x, int y, char letter)
        {
            game.CurrentPlayer().Tiles.First(
                t => t.Location == "board" && 
                t.BoardPositionX == x && 
                t.BoardPositionY == y && 
                t.Letter == letter);
        }

        [TestMethod]
        public void GivenEmptyGo_ThenNoTilesArePlaced()
        {
            WhenPlaceGo();
        }

        [TestMethod]
        public void GivenEmptyGo_ThenShouldReturnAllTilesToTray()
        {
            GivenPlayerTilesAreNotInTheTray();
            WhenPlaceGo();
            game.CurrentPlayer().Tiles.All(t => t.Location == "tray").ShouldBeTrue();
        }

        [TestMethod]
        public void GivenASingleLetterGo_ThenTilesArePlaced()
        {
            GivenGoWordStartingAt("A", "?", 7, 7);
            WhenPlaceGo();
            ThenPlayerTileShouldBePlacedAt(7, 7, 'A');
        }

        [TestMethod]
        public void GivenATwoLetterGo_ThenTilesArePlaced()
        {
            GivenGoWordStartingAt("AT", "??", 7, 7);
            WhenPlaceGo();
            ThenPlayerTileShouldBePlacedAt(7, 7, 'A');
            ThenPlayerTileShouldBePlacedAt(8, 7, 'T');
        }

        [TestMethod]
        public void GivenAGoWordThatUsesBoardTiles_ThenTilesArePlaced()
        {
            GivenGoWordStartingAt("ATE", "A??", 7, 7);
            WhenPlaceGo();
            ThenPlayerTileShouldBePlacedAt(8, 7, 'T');
            ThenPlayerTileShouldBePlacedAt(9, 7, 'E');
        }

        [TestMethod]
        public void GivenAVerticalGo_ThenTilesArePlaced()
        {
            GivenGoWordStartingAt("ATE", "A??", 7, 7, 1);
            WhenPlaceGo();
            ThenPlayerTileShouldBePlacedAt(7, 8, 'T');
            ThenPlayerTileShouldBePlacedAt(7, 9, 'E');
        }
    }
}
