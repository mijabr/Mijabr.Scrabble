using Scrabble.Value;

namespace Scrabble.Go
{
    public interface IGoHandler
    {
        GoResult Go(Game game);
    }
}
