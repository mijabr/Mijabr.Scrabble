using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Scrabble.Go;
using Scrabble.Persist;
using Scrabble.Play;
using Scrabble.Value;

namespace Mijabr.Scrabble.Controllers
{
    [Route("scrabble/api/[controller]")]
    public class ScrabbleController
    {
        private readonly IScrabbleManager scrabbleManager;

        public ScrabbleController(IScrabbleManager scrabbleManager)
        {
            this.scrabbleManager = scrabbleManager;
        }

        [HttpPost("getsquares")]
        public IEnumerable<BoardSquare> GetSquares()
        {
            return scrabbleManager.GetSquares();
        }

        [HttpPost("newgame")]
        public Game NewGame()
        {
            return scrabbleManager.NewGame();
        }

        [HttpPost("submitgo")]
        public GoResult SubmitGo([FromBody]Game game)
        {
            return scrabbleManager.SubmitGo(game);
        }

        [HttpPost("aigo")]
        public GoResult AiGo([FromBody]Game game)
        {
            return scrabbleManager.AiGo(game);
        }

        [HttpPost("shortlist")]
        public List<ShortGame> ShortList()
        {
            return scrabbleManager.ShortList();
        }
    }
}
