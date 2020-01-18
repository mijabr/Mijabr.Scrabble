using Scrabble.Value;

namespace Scrabble.Go
{
    public class GoResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public Game Game { get; set; }
        public int GoScore { get; set; }
    }
}
