using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scrabble.Go;
using Scrabble.Persist;
using Scrabble.Play;
using Scrabble.Value;
using System.Collections.Generic;
using System.Security.Claims;

namespace Mijabr.Scrabble.Controllers
{
    [Route("scrabble/api/[controller]")]
    public class ScrabbleController
    {
        private readonly IScrabbleManager scrabbleManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ScrabbleController(
            IScrabbleManager scrabbleManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this.scrabbleManager = scrabbleManager;
            this.httpContextAccessor = httpContextAccessor;
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
            var user = (httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity);
            return scrabbleManager.NewGame(user?.Name ?? "Player");
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
