using System.Collections.Generic;

namespace words
{
    public class FindWordsResponseMessage
    {
        public string SearchPattern { get; set; }
        public string Letters { get; set; }
        public IEnumerable<string> Words { get; set; }
    }
}
