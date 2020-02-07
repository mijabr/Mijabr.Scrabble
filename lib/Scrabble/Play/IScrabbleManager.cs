using Scrabble.Go;
using Scrabble.Persist;
using Scrabble.Value;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrabble.Play
{
    public interface IScrabbleManager
    {
        IEnumerable<BoardSquare> GetSquares();
        Game NewGame(string playerName);
        GoResult SubmitGo(Game game);
        GoResult AiGo(Game game);
        List<ShortGame> ShortList();
    }
}
