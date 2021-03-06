﻿using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Go
{
    public interface Validatable
    {
        IPlayer CurrentPlayer();
        List<Tile> BoardTiles { get; }
    }
}
