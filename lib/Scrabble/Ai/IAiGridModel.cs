using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Ai
{
    public interface IAiGridModel
    {
        void Build(List<Tile> playerTiles, List<Tile> boardTiles);
        AiGridModelTile[,] Grid { get; }
        List<Tile> PlayerTiles { get; }
        IEnumerable<AiCandidate> Candidates { get; }
    }
}
