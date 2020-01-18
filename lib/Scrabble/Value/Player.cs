using System.Collections.Generic;

namespace Scrabble.Value
{
    public class Player : IPlayer
    {
        public string Name { get; set; }
        public List<Tile> Tiles { get; set; } = new List<Tile>();
        public int Score { get; set; }
    }
}
