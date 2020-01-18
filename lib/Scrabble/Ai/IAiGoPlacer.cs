using Scrabble.Value;

namespace Scrabble.Ai
{
    public interface IAiGoPlacer
    {
        void PlaceGo(AiValidGo go, Game game);
    }
}
