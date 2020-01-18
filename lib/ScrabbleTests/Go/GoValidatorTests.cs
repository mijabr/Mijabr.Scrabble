using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Scrabble.Go;
using Scrabble.Value;
using Shouldly;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Tests
{
    [TestClass]
    public class GoValidatorTests
    {
        GridModelTile[,] gridModelTiles;
        IGridModel gridModelable;
        GoValidator validator;
        Validatable validatable;
        IPlayer player;
        ICollection<Tile> playerTiles;
        List<Tile> boardTiles;
        GoValidationResult result;

        [TestInitialize]
        public void Setup()
        {
            gridModelTiles = new GridModelTile[15, 15];
            gridModelable = Substitute.For<IGridModel>();
            gridModelable.Grid.Returns(gridModelTiles);
            validator = new GoValidator(gridModelable);
            validatable = Substitute.For<Validatable>();
            playerTiles = new List<Tile>();
            player = Substitute.For<IPlayer>();
            player.Tiles.Returns(playerTiles);
            validatable.CurrentPlayer().Returns(player);
            boardTiles = new List<Tile>();
            validatable.BoardTiles.Returns(boardTiles);
        }

        Tile MakePlayerTile(char letter, string location = "", int x = 0, int y = 0)
        {
            var playerTile = new Tile(letter) { Location = location, BoardPositionX = x, BoardPositionY = y };
            playerTile.IsBlank = letter == ' ';

            return playerTile;
        }

        void GivenPlayerTile(char letter, string location = "", int x = 0, int y = 0)
        {
            playerTiles.Add(MakePlayerTile(letter, location, x, y));
            gridModelTiles[x, y] = new GridModelTile()
            {
                Origin = GridModelTileOrigin.FromPlayer,
                Letter = letter
            };

            gridModelable.MinX.Returns(playerTiles.Min(t => t.BoardPositionX));
            gridModelable.MaxX.Returns(playerTiles.Max(t => t.BoardPositionX));
            gridModelable.MinY.Returns(playerTiles.Min(t => t.BoardPositionY));
            gridModelable.MaxY.Returns(playerTiles.Max(t => t.BoardPositionY));
        }

        void GivenBoardTile(char letter, int x, int y)
        {
            boardTiles.Add(new Tile(letter) { Location = "board", BoardPositionX = x, BoardPositionY = y });
            gridModelTiles[x, y] = new GridModelTile()
            {
                Origin = GridModelTileOrigin.FromBoard,
                Letter = letter
            };
        }

        void GivenPlayerTileIsOnTopOfBoardSpace()
        {
            gridModelable.IsPlayerTileOnOccupiedSpace.Returns(true);
        }

        void WhenValidateGo()
        {
            result = validator.ValidateGo(validatable);
        }

        void AssertGoIsInvalidWithMessage(string message)
        {
            result.IsValid.ShouldBeFalse();
            result.Message.ShouldBe(message);
        }

        void AssertGoIsValid()
        {
            result.IsValid.ShouldBeTrue();
        }

        [TestMethod]
        public void GivenPlayerHasNoTiles_ThenGoIsInvalid()
        {
            WhenValidateGo();
            AssertGoIsInvalidWithMessage("Player has no tiles");
        }

        [TestMethod]
        public void GivenPlayerHasPlacedNoTilesOnTheBoard_ThenGoIsInvalid()
        {
            GivenPlayerTile('A', "tray");
            WhenValidateGo();
            AssertGoIsInvalidWithMessage("Player must use at least one tile");
        }

        [TestMethod]
        public void GivenCentreSquareIsEmpty_ThenGoIsInvalid()
        {
            GivenPlayerTile('A', "board", 6, 7);
            WhenValidateGo();
            AssertGoIsInvalidWithMessage("The centre starting square must be used on the first turn");
        }

        [TestMethod]
        public void GivenTilesArePlaceInMoreThanOnePlane_ThenGoIsInvalid()
        {
            GivenPlayerTile('A', "board", 6, 7);
            GivenPlayerTile('A', "board", 7, 7);
            GivenPlayerTile('A', "board", 7, 6);
            WhenValidateGo();
            AssertGoIsInvalidWithMessage("Tiles must be placed in a single row or column");
        }

        [TestMethod]
        public void GivenPlacedTilesHaveAHorizontalGap_ThenGoIsInvalid()
        {
            GivenPlayerTile('A', "board", 7, 7);
            GivenPlayerTile('A', "board", 9, 7);
            WhenValidateGo();
            AssertGoIsInvalidWithMessage("Tiles must be placed without any gaps");
        }

        [TestMethod]
        public void GivenPlacedTilesHaveAHorizontalGapWhichIsFilledByABoardTile_ThenGoIsValid()
        {
            GivenPlayerTile('C', "board", 7, 7);
            GivenBoardTile('A', 8, 7); 
            GivenPlayerTile('T', "board", 9, 7);
            WhenValidateGo();
            AssertGoIsValid();
        }

        [TestMethod]
        public void GivenPlacedTilesHaveAVerticalGap_ThenGoIsInvalid()
        {
            GivenPlayerTile('A', "board", 7, 7);
            GivenPlayerTile('A', "board", 7, 9);
            WhenValidateGo();
            AssertGoIsInvalidWithMessage("Tiles must be placed without any gaps");
        }

        [TestMethod]
        public void GivenPlacedTilesHaveAVerticalGapWhichIsFilledByABoardTile_ThenGoIsValid()
        {
            GivenPlayerTile('C', "board", 7, 7);
            GivenBoardTile('A', 7, 8);
            GivenPlayerTile('T', "board", 7, 9);
            WhenValidateGo();
            AssertGoIsValid();
        }

        [TestMethod]
        public void GivenPlacedTilesAreOnTopOfOccupiedSpaces_ThenGoIsInvalid()
        {
            GivenPlayerTile('A', "board", 7, 7);
            GivenPlayerTileIsOnTopOfBoardSpace();
            WhenValidateGo();
            AssertGoIsInvalidWithMessage("Tiles be placed on empty board spaces");
        }

        [TestMethod]
        public void GivenPlacedTilesAreNotAdjacentToExistingTiles_ThenGoIsInvalid()
        {
            GivenBoardTile('A', 7, 7);
            GivenPlayerTile('A', "board", 6, 6);
            WhenValidateGo();
            AssertGoIsInvalidWithMessage("Tiles must be placed adjacent to existing board tiles");
        }

        [TestMethod]
        public void GivenTilesPlacedOnStartingSquare_ThenGoIsValid()
        {
            GivenPlayerTile('A', "board", 7, 7);
            WhenValidateGo();
            AssertGoIsValid();
        }

        [TestMethod]
        public void GivenBlankTilesWithoutLetter_ThenGoIsInvalid()
        {
            GivenPlayerTile(' ', "board", 7, 7);
            WhenValidateGo();
            AssertGoIsInvalidWithMessage("Blank tiles must have a letter assigned");
        }
    }
}
