using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrabble.Go;
using Scrabble.Value;
using Shouldly;
using System.Collections.Generic;

namespace Scrabble.Tests
{
    [TestClass]
    public class GridModelTests
    {
        GridModel gridModel;
        List<Tile> playerTiles;
        List<Tile> boardTiles;

        [TestInitialize]
        public void Setup()
        {
            gridModel = new GridModel(new Board());
            playerTiles = new List<Tile>();
            boardTiles = new List<Tile>();
        }

        [TestMethod]
        public void GivenAPlayerTile_ModelShouldContainPlayerTile()
        {
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.Grid[7, 7].Letter.ShouldBe('A');
            gridModel.Grid[7, 7].Origin.ShouldBe(GridModelTileOrigin.FromPlayer);
        }

        [TestMethod]
        public void GivenABoardTile_ModelShouldContainBoardTile()
        {
            boardTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.Grid[7, 7].Letter.ShouldBe('A');
            gridModel.Grid[7, 7].Origin.ShouldBe(GridModelTileOrigin.FromBoard);
        }

        [TestMethod]
        public void GivenAPlayerTileAndBoardTileOccupyTheSameSpace_ThenOccupyFlagShouldBeSet()
        {
            boardTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.IsPlayerTileOnOccupiedSpace.ShouldBeTrue();
        }

        [TestMethod]
        public void GivenPlayerTilesArePlaced_ThenMinimumAndMaximumPlayerTileCoordinatesOfTheGoCanBeFound()
        {
            boardTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 9, BoardPositionY = 7 });
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 8, BoardPositionY = 7 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.MinX.ShouldBe(8);
            gridModel.MinY.ShouldBe(7);
            gridModel.MaxX.ShouldBe(9);
            gridModel.MaxY.ShouldBe(7);
        }

        [TestMethod]
        public void GivenPlayerTilesArePlaced_ThenStartingCoordinatesOfTheGoCanBeFound()
        {
            boardTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 8, BoardPositionY = 8 });
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 8 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.GoStartX.ShouldBe(7);
            gridModel.GoStartY.ShouldBe(8);
        }

        [TestMethod]
        public void GivenPlayerTilesArePlacedAfterBoardTiles_ThenStartingCoordinatesShouldBeOfTheBoardTiles()
        {
            boardTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 9, BoardPositionY = 7 });
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 8, BoardPositionY = 7 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.GoStartX.ShouldBe(7);
            gridModel.GoStartY.ShouldBe(7);
        }

        [TestMethod]
        public void GivenPlayerTilesArePlacedVerticallyAfterBoardTiles_ThenStartingCoordinatesShouldBeOfTheBoardTiles()
        {
            boardTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 8 });
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 9 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.GoStartX.ShouldBe(7);
            gridModel.GoStartY.ShouldBe(7);
        }

        [TestMethod]
        public void GivenASingleTileIsPlaceBelowABoardTile_ThenTheGoShouldBeConsideredVertical()
        {
            boardTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 8 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.IsVerticalGo.ShouldBe(true);
            gridModel.IsHorizontalGo.ShouldBe(false);
        }

        [TestMethod]
        public void GivenASingleTileIsPlaceAboveABoardTile_ThenTheGoShouldBeConsideredVertical()
        {
            boardTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 6 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.IsVerticalGo.ShouldBe(true);
            gridModel.IsHorizontalGo.ShouldBe(false);
        }

        [TestMethod]
        public void GivenASingleTileIsPlaceRightOfABoardTile_ThenTheGoShouldBeConsideredHorizontal()
        {
            boardTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 8, BoardPositionY = 7 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.IsVerticalGo.ShouldBe(false);
            gridModel.IsHorizontalGo.ShouldBe(true);
        }

        [TestMethod]
        public void GivenASingleTileIsPlaceLeftOfABoardTile_ThenTheGoShouldBeConsideredHorizontal()
        {
            boardTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            playerTiles.Add(new Tile('A') { Location = "board", BoardPositionX = 6, BoardPositionY = 7 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.IsVerticalGo.ShouldBe(false);
            gridModel.IsHorizontalGo.ShouldBe(true);
        }

        [TestMethod]
        public void GivenASingleTileIsPlaceRightOfABoardTileAndBelowATile_ThenTheGoShouldBeConsideredHorizontal()
        {
            boardTiles.Add(new Tile('I') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            boardTiles.Add(new Tile('T') { Location = "board", BoardPositionX = 8, BoardPositionY = 7 });
            boardTiles.Add(new Tile('T') { Location = "board", BoardPositionX = 7, BoardPositionY = 8 });
            playerTiles.Add(new Tile('O') { Location = "board", BoardPositionX = 8, BoardPositionY = 8 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.IsHorizontalGo.ShouldBe(true);
            gridModel.IsVerticalGo.ShouldBe(false);
            gridModel.GoStartX.ShouldBe(7);
            gridModel.GoStartY.ShouldBe(8);
        }

        [TestMethod]
        public void GivenEmptyGridModel_ThenCanGetBoardSquare()
        {
            gridModel.Build(playerTiles, boardTiles);
            gridModel.GetBoardSquare(7, 7).Name.ShouldBe("ST");
            gridModel.GetBoardSquare(7, 7).WordBonus.ShouldBe(2);
        }

        [TestMethod]
        public void GivenABoardTileIsCoveringASquare_ThenBonusShouldNotBeAllowedAgain()
        {
            boardTiles.Add(new Tile('I') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.GetBoardSquare(7, 7).Name.ShouldBe("NS");
            gridModel.GetBoardSquare(7, 7).WordBonus.ShouldBe(1);
        }

        [TestMethod]
        public void GivenAPlayerTileIsCoveringASquare_ThenBonusShouldBeUsedOnThisTurn()
        {
            playerTiles.Add(new Tile('I') { Location = "board", BoardPositionX = 7, BoardPositionY = 7 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.GetBoardSquare(7, 7).Name.ShouldBe("ST");
            gridModel.GetBoardSquare(7, 7).WordBonus.ShouldBe(2);
        }

        [TestMethod]
        public void GivenAPlayerTile_ThenGridShouldContainTileValue()
        {
            playerTiles.Add(new Tile('I') { Location = "board", BoardPositionX = 7, BoardPositionY = 7, Value = 2 });
            gridModel.Build(playerTiles, boardTiles);
            gridModel.Grid[7, 7].TileValue.ShouldBe(2);
        }
    }
}
