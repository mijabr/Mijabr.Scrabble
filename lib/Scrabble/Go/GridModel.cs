using Scrabble.Value;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Go
{
    public class GridModel : IGridModel
    {
        Board board;
        public GridModel(Board board)
        {
            this.board = board;
        }

        public void Build(List<Tile> PlayerTiles, List<Tile> BoardTiles)
        {
            Clear();
            AddTilesToModel(BoardTiles, GridModelTileOrigin.FromBoard);
            AddTilesToModel(PlayerTiles, GridModelTileOrigin.FromPlayer);
            IsSingleTileGo = PlayerTiles.Count() == 1;
            CollectPlayerGoInformation(PlayerTiles);
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
                if (IsSingleTileGo)
                {
                    if (IsBoardTileLeftOrRight())
                    {
                        return true;
                    }

                    if (IsBoardTileAboveOrBelow())
                    {
                        return false;
                    }
                }

                return (MinY == MaxY);
            }
        }

        public bool IsVerticalGo
        {
            get
            {
                if (IsSingleTileGo)
                {
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
            else
            {
                return BoardSquare.NormalSquare();
            }
        }

        bool IsBoardTileAboveOrBelow()
        {
            return (MinY > 0 && !Grid[MinX, MinY - 1].IsEmpty())
                || (MinY < 14 && !Grid[MinX, MinY + 1].IsEmpty());
        }

        bool IsBoardTileLeftOrRight()
        {
            return (MinX > 0 && !Grid[MinX - 1, MinY].IsEmpty())
                || (MinX < 14 && !Grid[MinX + 1, MinY].IsEmpty());
        }

        void Clear()
        {
            Grid = new GridModelTile[15, 15];
            IsPlayerTileOnOccupiedSpace = false;
            IsSingleTileGo = false;
        }

        void AddTilesToModel(IEnumerable<Tile> tiles, GridModelTileOrigin origin)
        {
            foreach (var tile in tiles)
            {
                AddTileToModel(origin, tile);
            }
        }

        void AddTileToModel(GridModelTileOrigin origin, Tile tile)
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

        void CheckIfSpaceIsAlreadyOccupied(Tile tile)
        {
            if (Grid[tile.BoardPositionX, tile.BoardPositionY].Letter != 0)
            {
                IsPlayerTileOnOccupiedSpace = true;
            }
        }

        void CollectPlayerGoInformation(List<Tile> PlayerTiles)
        {
            if (PlayerTiles.Count() > 0)
            {
                GoStartX = MinX = PlayerTiles.Min(t => t.BoardPositionX);
                GoStartY = MinY = PlayerTiles.Min(t => t.BoardPositionY);
                MaxX = PlayerTiles.Max(t => t.BoardPositionX);
                MaxY = PlayerTiles.Max(t => t.BoardPositionY);
                AdjustStartingPositionForHoritontalGos();
                AdjustStartingPositionForVerticalGos();
            }
        }

        void AdjustStartingPositionForVerticalGos()
        {
            if (IsVerticalGo)
            {
                while (GoStartY > 0 && !Grid[GoStartX, GoStartY - 1].IsEmpty())
                {
                    GoStartY--;
                }
            }
        }

        void AdjustStartingPositionForHoritontalGos()
        {
            if (IsHorizontalGo)
            {
                while (GoStartX > 0 && !Grid[GoStartX - 1, GoStartY].IsEmpty())
                {
                    GoStartX--;
                }
            }
        }
    }
}
