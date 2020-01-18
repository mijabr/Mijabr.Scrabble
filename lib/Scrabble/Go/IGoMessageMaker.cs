using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Go
{
    public interface IGoMessageMaker
    {
        string GetGoMessage(string playerName, IEnumerable<GoWord> goWords, int goScore);
    }
}
