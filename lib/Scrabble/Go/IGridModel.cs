using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Go
{
    public interface IGridModel
    {
        void Build(List<Tile> PlayerTiles, List<Tile> BoardTiles);
        GridModelTile[,] Grid { get; }
        BoardSquare GetBoardSquare(int x, int y);
        GoLetter GetGoLetter(int x, int y);
        bool IsPlayerTileOnOccupiedSpace { get; }
        int MinX { get; }
        int MinY { get; }
        int MaxX { get; }
        int MaxY { get; }
        int GoStartX { get; }
        int GoStartY { get; }
        bool IsHorizontalGo { get; }
        bool IsVerticalGo { get; }
    }
}
