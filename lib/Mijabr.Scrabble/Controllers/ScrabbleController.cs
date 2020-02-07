using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrabble.Go;
using Scrabble.Persist;
using Scrabble.Play;
using Scrabble.Value;
using System.Collections.Generic;

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
        [Authorize]
        public IEnumerable<BoardSquare> GetSquares()
        {
            return scrabbleManager.GetSquares();
        }

        [HttpPost("newgame")]
        [Authorize]
        public Game NewGame()
        {
            return scrabbleManager.NewGame();
        }

        [HttpPost("submitgo")]
        [Authorize]
        public GoResult SubmitGo([FromBody]Game game)
        {
            return scrabbleManager.SubmitGo(game);
        }

        [HttpPost("aigo")]
        [Authorize]
        public GoResult AiGo([FromBody]Game game)
        {
            return scrabbleManager.AiGo(game);
        }

        [HttpPost("shortlist")]
        [Authorize]
        public List<ShortGame> ShortList()
        {
            return scrabbleManager.ShortList();
        }
    }
}
