using Scrabble.Value;
using System.Collections;
using System.Collections.Generic;

namespace Scrabble.Ai
{
    public class AiGridModel : IAiGridModel, IEnumerable<AiCandidate>, IEnumerator<AiCandidate>
    {
        public void Build(List<Tile> playerTiles, List<Tile> boardTiles)
        {
            Clear();
            PlayerTiles = playerTiles;
            Grid[7, 7].IsNextToTile = true;
            AddTilesToModel(boardTiles);
            Reset();
        }

        public AiGridModelTile[,] Grid { get; private set; }

        public IEnumerable<AiCandidate> Candidates
        {
            get
            {
                Reset();
                return this;
            }
        }

        public AiCandidate Current => currentCandidate;

        object IEnumerator.Current => currentCandidate;

        public bool MoveNext()
        {
            return NextCandidate();
        }

        public void Reset()
        {
            currentCandidate.SearchPattern = "";
            currentCandidate.StartX = 0;
            currentCandidate.StartY = 0;
            currentCandidate.Orientation = 0;
            currentCandidate.TilesUsed = PlayerTiles.Count + 1;
        }

        public void Dispose()
        {
        }

        public IEnumerator<AiCandidate> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public List<Tile> PlayerTiles { get; private set; }

        private AiCandidate currentCandidate;

        private void Clear()
        {
            Grid = new AiGridModelTile[15, 15];
        }

        private void AddTilesToModel(IEnumerable<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                AddTileToModel(tile);
            }
        }

        private void AddTileToModel(Tile tile)
        {
            Grid[tile.BoardPositionX, tile.BoardPositionY].Letter = tile.Letter;
            Grid[tile.BoardPositionX, tile.BoardPositionY].TileValue = tile.Value;
            SetNextToTile(tile.BoardPositionX - 1, tile.BoardPositionY);
            SetNextToTile(tile.BoardPositionX + 1, tile.BoardPositionY);
            SetNextToTile(tile.BoardPositionX, tile.BoardPositionY - 1);
            SetNextToTile(tile.BoardPositionX, tile.BoardPositionY + 1);
        }

        private void SetNextToTile(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < 15 && y < 15)
            {
                Grid[x, y].IsNextToTile = true;
            }
        }

        private bool NextCandidate()
        {
            while (currentCandidate.Next(PlayerTiles.Count))
            {
                if (IsValidCandidate())
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsValidCandidate()
        {
            return currentCandidate.Orientation == 0 
                ? IsValidHorizontalCandidate() 
                : IsValidVerticalCandidate();
        }

        private bool IsValidHorizontalCandidate()
        {
            if (currentCandidate.TilesUsed == 1 && (IsRightOfStartingSpotEmpty() || (IsStartAtBoardRight() && IsStartingSpotEmpty())) && (IsLeftOfStartingSpotEmpty() || (IsStartAtBoardLeft() && IsStartingSpotEmpty()) ))
            {
                if (!IsSpecialStartingGo())
                {
                    return false;
                }
            }
            else if (!IsLeftOfStartingSpotEmpty() && !IsStartAtBoardLeft())
            {
                return false;
            }

            var tilesLeftToPlace = currentCandidate.TilesUsed;
            var isJoined = false;
            currentCandidate.SearchPattern = string.Empty;
            var placeX = currentCandidate.StartX;

            while (placeX < 15 && (tilesLeftToPlace > 0 || Grid[placeX, currentCandidate.StartY].Letter != 0))
            {
                if (Grid[placeX, currentCandidate.StartY].Letter == 0)
                {
                    tilesLeftToPlace--;
                    currentCandidate.SearchPattern += "?";
                }
                else
                {
                    currentCandidate.SearchPattern += Grid[placeX, currentCandidate.StartY].Letter;
                }

                if (Grid[placeX, currentCandidate.StartY].IsNextToTile)
                {
                    isJoined = true;
                }

                placeX++;
            }

            return (tilesLeftToPlace == 0 && isJoined);
        }

        private bool IsValidVerticalCandidate()
        {
            if (!IsAboveOfStartingSpotEmpty() && (!IsStartAtBoardTop() || IsStartingSpotEmpty()))
            {
                return false;
            }

            if (currentCandidate.TilesUsed == 1 && IsStartingSpotEmpty() && (IsBelowOfStartingSpotEmpty() || IsStartAtBoardBottom()) && IsAboveOfStartingSpotEmpty())
            {
                return false;
            }

            var tilesLeftToPlace = currentCandidate.TilesUsed;
            var isJoined = false;
            currentCandidate.SearchPattern = string.Empty;
            var placeY = currentCandidate.StartY;

            while (placeY < 15 && (tilesLeftToPlace > 0 || Grid[currentCandidate.StartX, placeY].Letter != 0))
            {
                if (Grid[currentCandidate.StartX, placeY].Letter == 0)
                {
                    tilesLeftToPlace--;
                    currentCandidate.SearchPattern += "?";
                }
                else
                {
                    currentCandidate.SearchPattern += Grid[currentCandidate.StartX, placeY].Letter;
                }

                if (Grid[currentCandidate.StartX, placeY].IsNextToTile)
                {
                    isJoined = true;
                }

                placeY++;
            }

            return (tilesLeftToPlace == 0 && isJoined);
        }

        private bool IsStartingSpotEmpty()
        {
            return Grid[currentCandidate.StartX, currentCandidate.StartY].Letter == 0;
        }

        private bool IsLeftOfStartingSpotEmpty()
        {
            return currentCandidate.StartX > 0 && Grid[currentCandidate.StartX - 1, currentCandidate.StartY].Letter == 0;
        }

        private bool IsRightOfStartingSpotEmpty()
        {
            return currentCandidate.StartX < 14 && Grid[currentCandidate.StartX + 1, currentCandidate.StartY].Letter == 0;
        }

        private bool IsAboveOfStartingSpotEmpty()
        {
            return currentCandidate.StartY > 0 && Grid[currentCandidate.StartX, currentCandidate.StartY - 1].Letter == 0;
        }

        private bool IsBelowOfStartingSpotEmpty()
        {
            return currentCandidate.StartY < 14 && Grid[currentCandidate.StartX, currentCandidate.StartY + 1].Letter == 0;
        }

        private bool IsStartAtBoardCentre()
        {
            return currentCandidate.StartX == 7 && currentCandidate.StartY == 7;
        }

        private bool IsStartAtBoardLeft()
        {
            return currentCandidate.StartX == 0;
        }

        private bool IsStartAtBoardRight()
        {
            return currentCandidate.StartX == 14;
        }

        private bool IsStartAtBoardTop()
        {
            return currentCandidate.StartY == 0;
        }

        private bool IsStartAtBoardBottom()
        {
            return currentCandidate.StartY == 14;
        }

        private bool IsSpecialStartingGo()
        {
            return IsStartAtBoardCentre() && IsAboveOfStartingSpotEmpty() && IsBelowOfStartingSpotEmpty();
        }
    }
}
