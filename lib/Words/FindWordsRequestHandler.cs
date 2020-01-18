namespace words
{
    public class FindWordsRequestHandler
    {
        WordFindable wordFinder;
        public FindWordsRequestHandler(WordFindable wordFinder)
        {
            this.wordFinder = wordFinder;
        }

        public FindWordsResponseMessage FindWords(FindWordsRequestMessage message)
        {
            return new FindWordsResponseMessage()
            {
                SearchPattern = message.Pattern.ToLower(),
                Letters = message.Letters?.ToLower(),
                Words = wordFinder.FindWords(message.Pattern, message.Letters?.ToLower())
            };
        }
    }
}
