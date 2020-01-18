using Scrabble.Draw;
using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Go
{
    public class GoHandler : IGoHandler
    {
        IGoValidator validator;
        ITileDrawer drawer;
        IGoWordFinder goWordFinder;
        IGoWordValidator goWordValidator;
        IGoScorer goScorer;
        IGoMessageMaker goMessageMaker;
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

        public GoResult Go(Game game)
        {
            this.game = game;

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

        void FindGoWords()
        {
            goWords = goWordFinder.FindWords();
        }

        Game game;
        GoValidationResult basicValidationResult;
        IEnumerable<GoWord> goWords;
        GoValidationResult wordValidationResult;
        int goScore;

        void DoBasicValidation()
        {
            basicValidationResult = validator.ValidateGo(game);
        }

        void DoGoWordValidation()
        {
            wordValidationResult = goWordValidator.ValidateWords(goWords);
        }

        GoResult BasicValidationErrorResult()
        {
            return new GoResult()
            {
                IsValid = false,
                Message = basicValidationResult.Message
            };
        }

        GoResult WordValidationErrorResult()
        {
            return new GoResult()
            {
                IsValid = false,
                Message = wordValidationResult.Message
            };
        }

        GoResult SuccessResult()
        {
            return new GoResult()
            {
                IsValid = true,
                Message = goMessageMaker.GetGoMessage(game.CurrentPlayer().Name, goWords, goScore),
                Game = game,
                GoScore = goScore
            };
        }

        void ScoreGo()
        {
            goScore = goScorer.ScoreGo(goWords);
            game.CurrentPlayer().Score += goScore;
        }

        void MovePlayerTilesToBoard()
        {
            foreach (var tile in game.CurrentPlayer().Tiles)
            {
                if (tile.Location == "board")
                {
                    game.BoardTiles.Add(tile);
                }
            }

            game.CurrentPlayer().Tiles.RemoveAll(t => t.Location == "board");
        }

        void SetTurnToNextPlayer()
        {
            game.PlayerTurn++;
            if (game.PlayerTurn >= game.Players.Count)
            {
                game.PlayerTurn = 0;
            }
        }
    }
}
