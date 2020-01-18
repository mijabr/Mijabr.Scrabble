using Scrabble.Value;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrabble.Go
{
    public interface IGoScorer
    {
        int ScoreGo(IEnumerable<GoWord> goWords);
    }
}
