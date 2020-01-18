using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Go
{
    public interface IGoWordFinder
    {
        IEnumerable<GoWord> FindWords();
    }
}
