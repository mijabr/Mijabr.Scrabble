using Microsoft.AspNetCore.Mvc;
using words;

namespace Mijabr.Scrabble.Controllers
{
    [Route("scrabble/api/[controller]")]
    public class WordController
    {
        private IsWordRequestHandler isWordRequestHandler;
        private FindWordsRequestHandler findWordsRequestHandler;

        public WordController(
            IsWordRequestHandler isWordRequestHandler,
            FindWordsRequestHandler findWordsRequestHandler)
        {
            this.isWordRequestHandler = isWordRequestHandler;
            this.findWordsRequestHandler = findWordsRequestHandler;
        }

        [HttpPost("isword")]
        public IsWordResponseMessage IsWord([FromBody]IsWordRequestMessage message)
        {
            return isWordRequestHandler.IsWord(message);
        }

        [HttpPost("findwords")]
        public FindWordsResponseMessage FindWords([FromBody]FindWordsRequestMessage message)
        {
            return findWordsRequestHandler.FindWords(message);
        }
    }
}
