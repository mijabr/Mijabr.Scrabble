using Scrabble.Draw;
using Scrabble.Go;
using System;
using System.Collections.Generic;

namespace Scrabble.Value
{
    public class Game : Drawable, Validatable
    {
        public ICollection<Tile> BagTiles { get; set; }
        public List<Tile> BoardTiles { get; set; } = new List<Tile>();
        public List<Player> Players { get; set; }
        public int PlayerTurn { get; set; }
        public Guid Id { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset LastActiveTime { get; set; }
        public bool IsFinished { get; set; } = false;

        public IPlayer CurrentPlayer()
        {
            return Players[PlayerTurn];
        }
    }
}
