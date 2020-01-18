using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Draw
{
    public interface Drawable
    {
        ICollection<Tile> BagTiles { get; }
        IPlayer CurrentPlayer();
        List<Player> Players { get; }
        int PlayerTurn { get; set; }
    }
}
