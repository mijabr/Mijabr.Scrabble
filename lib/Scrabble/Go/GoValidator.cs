using Scrabble.Value;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Go
{
    public class GoValidator : IGoValidator
    {
        IGridModel gridModelable;
        public GoValidator(IGridModel gridModelable)
        {
            this.gridModelable = gridModelable;
        }

        public GoValidationResult ValidateGo(Validatable validatable)
        {
            this.validatable = validatable;
            result = new GoValidationResult() { IsValid = true };

            ApplyValidationChecks();

            return result;
        }

        Validatable validatable;
        GoValidationResult result;
        IEnumerable<Tile> usedPlayerTiles;

        void ApplyValidationChecks()
        {
            var validationChecks = GetValidationChecks();
            foreach (var validationCheck in validationChecks)
            {
                validationCheck();

                if (result.Message != null)
                {
                    result.IsValid = false;
                    break;
                }
            }
        }

        Action[] GetValidationChecks()
        {
            return new Action[]
            {
                CheckIfPlayerHasAnyTiles,
                CheckIfPlayerHasPlacedAnyTiles,
                CheckIfCentreSquareIsUsed,
                CheckIfTilesPlacedInASingleRowOrColumn,
                CheckIfTilesPlacedIntoEmptyBoardSpaces,
                CheckIfTilesPlacedHorizontallyHaveAnyGaps,
                CheckIfTilesPlacedVerticallyHaveAnyGaps,
                CheckIfTilesPlacedAdjacentToExistingTiles,
                CheckIfUsedBlankTilesHaveASubstituteLetter
            };
        }

        void CheckIfPlayerHasAnyTiles()
        {
            if (validatable.CurrentPlayer().Tiles.Count == 0)
            {
                result.Message = "Player has no tiles";
            }
        }

        void CheckIfPlayerHasPlacedAnyTiles()
        {
            usedPlayerTiles = validatable.CurrentPlayer().Tiles.Where(t => t.Location == "board");
            if (usedPlayerTiles.Count() == 0)
            {
                result.Message = "Player must use at least one tile";
            }
        }

        void CheckIfCentreSquareIsUsed()
        {
            var centreTile = validatable.BoardTiles.FirstOrDefault(t => t.BoardPositionX == 7 && t.BoardPositionY == 7);
            if (centreTile.IsDefault())
            {
                var playerCentreTile = usedPlayerTiles.FirstOrDefault(t => t.BoardPositionX == 7 && t.BoardPositionY == 7);
                if (playerCentreTile.IsDefault())
                {
                    result.Message = "The centre starting square must be used on the first turn";
                }
            }
        }

        void CheckIfTilesPlacedInASingleRowOrColumn()
        {
            BuildGridModel();

            if (gridModelable.MinY != gridModelable.MaxY && gridModelable.MinX != gridModelable.MaxX)
            {
                result.Message = "Tiles must be placed in a single row or column";
            }
        }

        void BuildGridModel()
        {
            gridModelable.Build(usedPlayerTiles.ToList(), validatable.BoardTiles);
        }

        void CheckIfTilesPlacedIntoEmptyBoardSpaces()
        {
            if (gridModelable.IsPlayerTileOnOccupiedSpace)
            {
                result.Message = "Tiles be placed on empty board spaces";
            }
        }

        void CheckIfTilesPlacedHorizontallyHaveAnyGaps()
        {
            if (gridModelable.MinY == gridModelable.MaxY)
            {
                int x = gridModelable.MinX;
                int y = gridModelable.MinY;
                var playerTileCount = 0;
                while (x < 15 && playerTileCount < usedPlayerTiles.Count())
                {
                    if (gridModelable.Grid[x, y].Origin == GridModelTileOrigin.FromPlayer)
                    {
                        playerTileCount++;
                    }

                    if (gridModelable.Grid[x, y].IsEmpty())
                    {
                        result.Message = "Tiles must be placed without any gaps";
                    }

                    x++;
                }
            }
        }

        void CheckIfTilesPlacedVerticallyHaveAnyGaps()
        {
            if (gridModelable.MinX == gridModelable.MaxX)
            {
                int x = gridModelable.MinX;
                int y = gridModelable.MinY;
                var playerTileCount = 0;
                while (y < 15 && playerTileCount < usedPlayerTiles.Count())
                {
                    if (gridModelable.Grid[x, y].Origin == GridModelTileOrigin.FromPlayer)
                    {
                        playerTileCount++;
                    }

                    if (gridModelable.Grid[x, y].IsEmpty())
                    {
                        result.Message = "Tiles must be placed without any gaps";
                    }

                    y++;
                }
            }
        }

        void CheckIfTilesPlacedAdjacentToExistingTiles()
        {
            foreach (var tile in usedPlayerTiles)
            {
                if ((tile.BoardPositionX == 7 && tile.BoardPositionY == 7)
                || (tile.BoardPositionX > 0 && gridModelable.Grid[tile.BoardPositionX - 1, tile.BoardPositionY].Origin == GridModelTileOrigin.FromBoard)
                || (tile.BoardPositionX < 14 && gridModelable.Grid[tile.BoardPositionX + 1, tile.BoardPositionY].Origin == GridModelTileOrigin.FromBoard)
                || (tile.BoardPositionY > 0 && gridModelable.Grid[tile.BoardPositionX, tile.BoardPositionY - 1].Origin == GridModelTileOrigin.FromBoard)
                || (tile.BoardPositionY < 14 && gridModelable.Grid[tile.BoardPositionX, tile.BoardPositionY + 1].Origin == GridModelTileOrigin.FromBoard))
                {
                    return;
                }
            }

            result.Message = "Tiles must be placed adjacent to existing board tiles";
        }

        void CheckIfUsedBlankTilesHaveASubstituteLetter()
        {
            foreach (var tile in usedPlayerTiles)
            {
                if (tile.IsBlank && tile.Letter == ' ')
                {
                    result.Message = "Blank tiles must have a letter assigned";
                }
            }
        }
    }
}
