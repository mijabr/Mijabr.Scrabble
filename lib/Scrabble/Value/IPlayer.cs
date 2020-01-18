using System.Collections.Generic;

namespace Scrabble.Value
{
    public interface IPlayer
    {
        string Name { get; set; }
        List<Tile> Tiles { get; set; }
        int Score { get; set; }
    }
}
