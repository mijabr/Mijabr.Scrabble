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

        AiCandidate currentCandidate;

        void Clear()
        {
            Grid = new AiGridModelTile[15, 15];
        }

        void AddTilesToModel(IEnumerable<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                AddTileToModel(tile);
            }
        }

        void AddTileToModel(Tile tile)
        {
            Grid[tile.BoardPositionX, tile.BoardPositionY].Letter = tile.Letter;
            Grid[tile.BoardPositionX, tile.BoardPositionY].TileValue = tile.Value;
            SetNextToTile(tile.BoardPositionX - 1, tile.BoardPositionY);
            SetNextToTile(tile.BoardPositionX + 1, tile.BoardPositionY);
            SetNextToTile(tile.BoardPositionX, tile.BoardPositionY - 1);
            SetNextToTile(tile.BoardPositionX, tile.BoardPositionY + 1);
        }

        void SetNextToTile(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < 15 && y < 15)
            {
                Grid[x, y].IsNextToTile = true;
            }
        }

        bool NextCandidate()
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

        bool IsValidCandidate()
        {
            if (currentCandidate.Orientation == 0)
            {
                return IsValidHorizontalCandidate();
            }
            else
            {
                return IsValidVerticalCandidate();
            }
        }

        bool IsValidHorizontalCandidate()
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

            int tilesLeftToPlace = currentCandidate.TilesUsed;
            bool isJoined = false;
            currentCandidate.SearchPattern = string.Empty;
            int placeX = currentCandidate.StartX;

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

        bool IsValidVerticalCandidate()
        {
            if (!IsAboveOfStartingSpotEmpty() && (!IsStartAtBoardTop() || IsStartingSpotEmpty()))
            {
                return false;
            }

            if (currentCandidate.TilesUsed == 1 && IsStartingSpotEmpty() && (IsBelowOfStartingSpotEmpty() || IsStartAtBoardBottom()) && IsAboveOfStartingSpotEmpty())
            {
                return false;
            }

            int tilesLeftToPlace = currentCandidate.TilesUsed;
            bool isJoined = false;
            currentCandidate.SearchPattern = string.Empty;
            int placeY = currentCandidate.StartY;

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

        bool IsStartingSpotEmpty()
        {
            return Grid[currentCandidate.StartX, currentCandidate.StartY].Letter == 0;
        }

        bool IsLeftOfStartingSpotEmpty()
        {
            return currentCandidate.StartX > 0 && Grid[currentCandidate.StartX - 1, currentCandidate.StartY].Letter == 0;
        }

        bool IsRightOfStartingSpotEmpty()
        {
            return currentCandidate.StartX < 14 && Grid[currentCandidate.StartX + 1, currentCandidate.StartY].Letter == 0;
        }

        bool IsAboveOfStartingSpotEmpty()
        {
            return currentCandidate.StartY > 0 && Grid[currentCandidate.StartX, currentCandidate.StartY - 1].Letter == 0;
        }

        bool IsBelowOfStartingSpotEmpty()
        {
            return currentCandidate.StartY < 14 && Grid[currentCandidate.StartX, currentCandidate.StartY + 1].Letter == 0;
        }

        bool IsStartAtBoardCentre()
        {
            return currentCandidate.StartX == 7 && currentCandidate.StartY == 7;
        }

        bool IsStartAtBoardLeft()
        {
            return currentCandidate.StartX == 0;
        }

        bool IsStartAtBoardRight()
        {
            return currentCandidate.StartX == 14;
        }

        bool IsStartAtBoardTop()
        {
            return currentCandidate.StartY == 0;
        }

        bool IsStartAtBoardBottom()
        {
            return currentCandidate.StartY == 14;
        }

        bool IsSpecialStartingGo()
        {
            return IsStartAtBoardCentre() && IsAboveOfStartingSpotEmpty() && IsBelowOfStartingSpotEmpty();
        }
    }
}
