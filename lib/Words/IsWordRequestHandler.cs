namespace words
{
    public class IsWordRequestHandler
    {
        WordValidatable wordValidator;
        public IsWordRequestHandler(WordValidatable wordValidator)
        {
            this.wordValidator = wordValidator;
        }

        public IsWordResponseMessage IsWord(IsWordRequestMessage message)
        {
            return new IsWordResponseMessage
            {
                CheckedWord = message.Word.ToLower(),
                IsWord = wordValidator.IsWord(message.Word)
            };
        }
    }
}
