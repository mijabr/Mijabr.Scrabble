using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using words;

namespace Mijabr.Scrabble.Controllers
{
    [Route("scrabble/api/[controller]")]
    public class WordController
    {
        private readonly IsWordRequestHandler isWordRequestHandler;
        private readonly FindWordsRequestHandler findWordsRequestHandler;

        public WordController(
            IsWordRequestHandler isWordRequestHandler,
            FindWordsRequestHandler findWordsRequestHandler)
        {
            this.isWordRequestHandler = isWordRequestHandler;
            this.findWordsRequestHandler = findWordsRequestHandler;
        }

        [HttpPost("isword")]
        [Authorize]
        public IsWordResponseMessage IsWord([FromBody]IsWordRequestMessage message)
        {
            return isWordRequestHandler.IsWord(message);
        }

        [HttpPost("findwords")]
        [Authorize]
        public FindWordsResponseMessage FindWords([FromBody]FindWordsRequestMessage message)
        {
            return findWordsRequestHandler.FindWords(message);
        }
    }
}
