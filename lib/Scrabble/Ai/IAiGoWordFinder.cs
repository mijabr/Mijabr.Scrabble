using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Ai
{
    public interface IAiGoWordFinder
    {
        IEnumerable<GoWord> FindWords(string mainWord, AiCandidate candidate);
    }
}
