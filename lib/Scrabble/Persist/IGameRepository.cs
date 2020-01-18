using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Persist
{
    public interface IGameRepository
    {
        void Set(Game game);
        List<ShortGame> GetShortList();
    }
}
