using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Ai
{
    public class AiValidGo
    {
        public string MainWord { get; set; }
        public AiCandidate Candidate { get; set; }
        public IEnumerable<GoWord> GoWords { get; set; }
        public int Score { get; set; }
    }
}
