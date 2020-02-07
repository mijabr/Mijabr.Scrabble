using Scrabble.Draw;
using Scrabble.Value;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Go
{
    public class GoHandler : IGoHandler
    {
        private readonly IGoValidator validator;
        private readonly ITileDrawer drawer;
        private readonly IGoWordFinder goWordFinder;
        private readonly IGoWordValidator goWordValidator;
        private readonly IGoScorer goScorer;
        private readonly IGoMessageMaker goMessageMaker;

        public GoHandler(
            IGoValidator validator,
            ITileDrawer drawer,
            IGoWordFinder goWordFinder,
            IGoWordValidator goWordValidator,
            IGoScorer goScorer,
            IGoMessageMaker goMessageMaker)
        {
            this.validator = validator;
            this.drawer = drawer;
            this.goWordFinder = goWordFinder;
            this.goWordValidator = goWordValidator;
            this.goScorer = goScorer;
            this.goMessageMaker = goMessageMaker;
        }

        public GoResult Go(Game theGame)
        {
            this.game = theGame;

            DoBasicValidation();
            if (!basicValidationResult.IsValid)
            {
                return BasicValidationErrorResult();
            }

            FindGoWords();
            DoGoWordValidation();
            if (!wordValidationResult.IsValid)
            {
                return WordValidationErrorResult();
            }

            ScoreGo();

            MovePlayerTilesToBoard();
            drawer.DrawTilesForPlayer(game);
            if (game.CurrentPlayer().Tiles.Count == 0)
            {
                game.IsFinished = true;
            }

            game.Players.ForEach(player =>
            {
                if (player.Tiles.Count == 0)
                {
                    game.IsFinished = true;
                }
            });

            var result = SuccessResult();

            SetTurnToNextPlayer();

            return result;
        }

        private void FindGoWords()
        {
            goWords = goWordFinder.FindWords();
        }

        private Game game;
        private GoValidationResult basicValidationResult;
        private IEnumerable<GoWord> goWords;
        private GoValidationResult wordValidationResult;
        private int goScore;

        private void DoBasicValidation()
        {
            basicValidationResult = validator.ValidateGo(game);
        }

        private void DoGoWordValidation()
        {
            wordValidationResult = goWordValidator.ValidateWords(goWords);
        }

        private GoResult BasicValidationErrorResult()
        {
            return new GoResult()
            {
                IsValid = false,
                Message = basicValidationResult.Message
            };
        }

        private GoResult WordValidationErrorResult()
        {
            return new GoResult()
            {
                IsValid = false,
                Message = wordValidationResult.Message
            };
        }

        private GoResult SuccessResult()
        {
            return new GoResult()
            {
                IsValid = true,
                Message = goMessageMaker.GetGoMessage(game.CurrentPlayer().Name, goWords, goScore),
                Game = game,
                GoScore = goScore
            };
        }

        private void ScoreGo()
        {
            goScore = goScorer.ScoreGo(goWords);
            game.CurrentPlayer().Score += goScore;
        }

        private void MovePlayerTilesToBoard()
        {
            foreach (var tile in game.CurrentPlayer().Tiles.Where(tile => tile.Location == "board"))
            {
                game.BoardTiles.Add(tile);
            }

            game.CurrentPlayer().Tiles.RemoveAll(t => t.Location == "board");
        }

        private void SetTurnToNextPlayer()
        {
            game.PlayerTurn++;
            if (game.PlayerTurn >= game.Players.Count)
            {
                game.PlayerTurn = 0;
            }
        }
    }
}
