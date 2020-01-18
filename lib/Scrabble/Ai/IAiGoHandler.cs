using Scrabble.Go;
using Scrabble.Value;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrabble.Ai
{
    public interface IAiGoHandler
    {
        GoResult Go(Game game);
    }
}
