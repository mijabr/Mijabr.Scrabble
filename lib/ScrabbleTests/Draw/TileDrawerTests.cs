using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Scrabble.Draw;
using Scrabble.Play;
using Scrabble.Value;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Tests
{
    [TestClass]
    public class TileDrawerTests
    {
        TileDrawer drawer;
        Game game;
        Dictionary<char, int> tileDrawCounts;

        [TestInitialize]
        public void Setup()
        {
            drawer = new TileDrawer();
            game = new GameFactory(Substitute.For<IDateTimeOffset>()).NewGame();
        }

        void GivenOneBagTilesIsLeft()
        {
            game.BagTiles = new List<Tile>() { new Tile('A') };
        }

        void GivenTenBagTilesAreLeft()
        {
            game.BagTiles = new List<Tile>()
            {
                new Tile('A'), new Tile('E'), new Tile('I'), new Tile('O'), new Tile('U'),
                new Tile('J'), new Tile('S'), new Tile('T'), new Tile('R'), new Tile('M')
            };
        }

        void GivenPlayerHasThreeTilesPositionedAtZeroOneAndFive()
        {
            game.CurrentPlayer().Tiles = new List<Tile>()
            {
                new Tile('X') { TrayPosition = 5 },
                new Tile('Z') { TrayPosition = 0 },
                new Tile('P') { TrayPosition = 1 }
            };
        }

        void AssertBagTileCountAndPlayerTileCount(int bagTileCount, int playerTileCount)
        {
            game.BagTiles.Count().ShouldBe(bagTileCount);
            game.CurrentPlayer().Tiles.Count().ShouldBe(playerTileCount);
        }

        void SetupTileDrawCountDictionary()
        {
            GivenTenBagTilesAreLeft();
            tileDrawCounts = new Dictionary<char, int>();
            foreach (var tile in game.BagTiles)
            {
                tileDrawCounts[tile.Letter] = 0;
            }
        }

        void Draw1000Times()
        {
            for (int i = 0; i < 1000; i++)
            {
                GivenTenBagTilesAreLeft();
                game.CurrentPlayer().Tiles.Clear();
                drawer.DrawTilesForPlayer(game);
                foreach (var tile in game.CurrentPlayer().Tiles)
                {
                    tileDrawCounts[tile.Letter]++;
                }
            }
        }

        void AssertPlayerTilesAreInTileTray()
        {
            foreach (var tile in game.CurrentPlayer().Tiles)
            {
                tile.Location.ShouldBe("tray");
            }
        }

        void AssertPlayerTilesArePositionedAt(params int[] positions)
        {
            var orderedPlayerTiles = game.CurrentPlayer().Tiles.OrderBy(t => t.TrayPosition);
            int n = 0;
            foreach (var tile in orderedPlayerTiles)
            {
                tile.TrayPosition.ShouldBe(positions[n++]);
            }
        }
        void AssertAllPlayersHaveSevenTiles()
        {
            foreach (var player in game.Players)
            {
                player.Tiles.Count(t => t.Location == "tray").ShouldBe(7);
            }
        }

        [TestMethod]
        public void GivenNullBagTiles_WhenDrawTiles_ThenNoTilesAreDrawn()
        {
            game.BagTiles = null;
            drawer.DrawTilesForPlayer(game);
            game.CurrentPlayer().Tiles.Count().ShouldBe(0);
        }

        [TestMethod]
        public void GivenNullPlayerTiles_WhenDrawTiles_ThenNoTilesAreDrawn()
        {
            game.CurrentPlayer().Tiles = null;
            drawer.DrawTilesForPlayer(game);
        }

        [TestMethod]
        public void GivenNoBagTiles_WhenDrawTiles_ThenNoTilesAreDrawn()
        {
            game.BagTiles.Clear();
            drawer.DrawTilesForPlayer(game);
            game.CurrentPlayer().Tiles.Count().ShouldBe(0);
        }

        [TestMethod]
        public void GivenOneBagTile_WhenDrawTiles_ThenOneTileIsDrawn()
        {
            GivenOneBagTilesIsLeft();
            drawer.DrawTilesForPlayer(game);
            AssertBagTileCountAndPlayerTileCount(0, 1);
            game.CurrentPlayer().Tiles.First().Letter.ShouldBe('A');
        }

        [TestMethod]
        public void GivenPlentyOfBagTiles_WhenDrawTiles_ThenOnlySevenAreDrawn()
        {
            GivenTenBagTilesAreLeft();
            drawer.DrawTilesForPlayer(game);
            AssertBagTileCountAndPlayerTileCount(3, 7);
        }

        [TestMethod]
        public void GivenPlayerHasSomeTiles_WhenDrawTiles_ThenPlayerDrawsUpToSeven()
        {
            GivenTenBagTilesAreLeft();
            GivenPlayerHasThreeTilesPositionedAtZeroOneAndFive();
            drawer.DrawTilesForPlayer(game);
            AssertBagTileCountAndPlayerTileCount(6, 7);
        }

        [TestMethod]
        public void PlayerTilesShouldBeDrawnAtRandom()
        {
            SetupTileDrawCountDictionary();
            Draw1000Times();

            foreach (var tileCount in tileDrawCounts)
            {
                Console.WriteLine($"Tile {tileCount.Key} was drawn {tileCount.Value} times");
            }

            foreach (var tileCount in tileDrawCounts)
            {
                tileCount.Value.ShouldBeInRange(550, 850, $"{tileCount.Key} was drawn outside the acceptable deviation");
            }
        }

        [TestMethod]
        public void PlayerTilesShouldBeDrawIntoTheTileTray()
        {
            GivenTenBagTilesAreLeft();
            drawer.DrawTilesForPlayer(game);
            AssertPlayerTilesAreInTileTray();
        }

        [TestMethod]
        public void PlayerTilesShouldBeDrawnToAllEmptyPositions()
        {
            GivenTenBagTilesAreLeft();
            drawer.DrawTilesForPlayer(game);
            AssertPlayerTilesArePositionedAt(0, 1, 2, 3, 4, 5, 6);
        }

        [TestMethod]
        public void PlayerTilesShouldBeDrawnToTheFirstEmptyPosition()
        {
            GivenOneBagTilesIsLeft();
            GivenPlayerHasThreeTilesPositionedAtZeroOneAndFive();
            drawer.DrawTilesForPlayer(game);
            AssertPlayerTilesArePositionedAt(0, 1, 2, 5);
        }

        [TestMethod]
        public void WhenDrawForAllPlayers_ThenAllPlayersShouldGetTiles()
        {
            drawer.DrawTilesForAllPlayers(game);
            AssertAllPlayersHaveSevenTiles();
        }
    }
}
