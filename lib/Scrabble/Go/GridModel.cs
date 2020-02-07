using Scrabble.Value;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Go
{
    public class GridModel : IGridModel
    {
        private readonly Board board;

        public GridModel(Board board)
        {
            this.board = board;
        }

        public void Build(List<Tile> playerTiles, List<Tile> boardTiles)
        {
            Clear();
            AddTilesToModel(boardTiles, GridModelTileOrigin.FromBoard);
            AddTilesToModel(playerTiles, GridModelTileOrigin.FromPlayer);
            IsSingleTileGo = playerTiles.Count() == 1;
            CollectPlayerGoInformation(playerTiles);
        }

        public GridModelTile[,] Grid { get; private set; }
        public bool IsPlayerTileOnOccupiedSpace { get; private set; }
        public bool IsSingleTileGo { get; private set; }
        public int MinX { get; private set; }
        public int MinY { get; private set; }
        public int MaxX { get; private set; }
        public int MaxY { get; private set; }
        public int GoStartX { get; private set; }
        public int GoStartY { get; private set; }
        public bool IsHorizontalGo
        {
            get
            {
                if (!IsSingleTileGo) return (MinY == MaxY);

                if (IsBoardTileLeftOrRight())
                {
                    return true;
                }

                if (IsBoardTileAboveOrBelow())
                {
                    return false;
                }

                return (MinY == MaxY);
            }
        }

        public bool IsVerticalGo
        {
            get
            {
                if (!IsSingleTileGo) return (MinX == MaxX);

                if (IsHorizontalGo)
                {
                    return false;
                }

                if (IsBoardTileAboveOrBelow())
                {
                    return true;
                }

                if (IsBoardTileLeftOrRight())
                {
                    return false;
                }

                return (MinX == MaxX);
            }
        }

        public GoLetter GetGoLetter(int x, int y)
        {
            var goSquare = GetBoardSquare(x, y);
            return new GoLetter()
            {
                TileValue = this.Grid[x, y].TileValue,
                LetterBonus = goSquare.LetterBonus,
                WordBonus = goSquare.WordBonus
            };
        }

        public BoardSquare GetBoardSquare(int x, int y)
        {
            if (Grid[x, y].IsEmpty() || Grid[x, y].Origin == GridModelTileOrigin.FromPlayer)
            {
                return board.GetSquare(x, y);
            }

            return BoardSquare.NormalSquare();
        }

        private bool IsBoardTileAboveOrBelow()
        {
            return (MinY > 0 && !Grid[MinX, MinY - 1].IsEmpty())
                || (MinY < 14 && !Grid[MinX, MinY + 1].IsEmpty());
        }

        private bool IsBoardTileLeftOrRight()
        {
            return (MinX > 0 && !Grid[MinX - 1, MinY].IsEmpty())
                || (MinX < 14 && !Grid[MinX + 1, MinY].IsEmpty());
        }

        private void Clear()
        {
            Grid = new GridModelTile[15, 15];
            IsPlayerTileOnOccupiedSpace = false;
            IsSingleTileGo = false;
        }

        private void AddTilesToModel(IEnumerable<Tile> tiles, GridModelTileOrigin origin)
        {
            foreach (var tile in tiles)
            {
                AddTileToModel(origin, tile);
            }
        }

        private void AddTileToModel(GridModelTileOrigin origin, Tile tile)
        {
            if (origin == GridModelTileOrigin.FromPlayer)
            {
                CheckIfSpaceIsAlreadyOccupied(tile);
            }

            Grid[tile.BoardPositionX, tile.BoardPositionY] = new GridModelTile()
            {
                Origin = origin,
                Letter = tile.Letter,
                TileValue = tile.Value
            };
        }

        private void CheckIfSpaceIsAlreadyOccupied(Tile tile)
        {
            if (Grid[tile.BoardPositionX, tile.BoardPositionY].Letter != 0)
            {
                IsPlayerTileOnOccupiedSpace = true;
            }
        }

        private void CollectPlayerGoInformation(List<Tile> playerTiles)
        {
            if (!playerTiles.Any()) return;

            GoStartX = MinX = playerTiles.Min(t => t.BoardPositionX);
            GoStartY = MinY = playerTiles.Min(t => t.BoardPositionY);
            MaxX = playerTiles.Max(t => t.BoardPositionX);
            MaxY = playerTiles.Max(t => t.BoardPositionY);
            AdjustStartingPositionForHorizontalGos();
            AdjustStartingPositionForVerticalGos();
        }

        private void AdjustStartingPositionForVerticalGos()
        {
            if (!IsVerticalGo) return;

            while (GoStartY > 0 && !Grid[GoStartX, GoStartY - 1].IsEmpty())
            {
                GoStartY--;
            }
        }

        private void AdjustStartingPositionForHorizontalGos()
        {
            if (!IsHorizontalGo) return;

            while (GoStartX > 0 && !Grid[GoStartX - 1, GoStartY].IsEmpty())
            {
                GoStartX--;
            }
        }
    }
}
