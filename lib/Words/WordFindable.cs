using System.Collections.Generic;

namespace words
{
    public interface WordFindable
    {
        IEnumerable<string> FindWords(string pattern, string letters = null);
    }
}
