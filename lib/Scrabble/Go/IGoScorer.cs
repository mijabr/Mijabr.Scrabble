using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Go
{
    public interface IGoScorer
    {
        int ScoreGo(IEnumerable<GoWord> goWords);
    }
}
