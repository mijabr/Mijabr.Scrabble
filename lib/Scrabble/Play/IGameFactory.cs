using Scrabble.Value;

namespace Scrabble.Play
{
    public interface IGameFactory
    {
        Game NewGame(string playerName);
    }
}
