using System.Collections.Generic;

namespace Scrabble.Value
{
    public class GoWord
    {
        public string Word { get; set; }

        public List<GoLetter> GoLetters { get; set; }

        public override string ToString()
        {
            return Word;
        }
    }
}
