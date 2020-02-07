using Scrabble.Go;
using Scrabble.Value;

namespace Scrabble.Ai
{
    public interface IAiGoHandler
    {
        GoResult Go(Game game);
    }
}
