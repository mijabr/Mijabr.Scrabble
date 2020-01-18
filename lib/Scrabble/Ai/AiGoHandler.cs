using Scrabble.Go;
using Scrabble.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using words;

namespace Scrabble.Ai
{
    public class AiGoHandler : IAiGoHandler
    {
        IAiGridModel aiGridModel;
        WordFindable wordFinder;
        IAiGoWordFinder goWordFinder;
        WordValidatable wordValidator;
        IGoScorer goScorer;
        IAiGoPlacer goPlacer;
        public AiGoHandler(
            IAiGridModel aiGridModel,
            WordFindable wordFinder,
            IAiGoWordFinder goWordFinder,
            WordValidatable wordValidator,
            IGoScorer goScorer,
            IAiGoPlacer goPlacer)
        {
            this.aiGridModel = aiGridModel;
            this.goWordFinder = goWordFinder;
            this.wordFinder = wordFinder;
            this.wordValidator = wordValidator;
            this.goScorer = goScorer;
            this.goPlacer = goPlacer;
        }

        public GoResult Go(Game game)
        {
            aiGridModel.Build(game.CurrentPlayer().Tiles, game.BoardTiles);

            ValidGoes = new List<AiValidGo>();
            if (game.CurrentPlayer().Tiles.Count == 0)
            {
                return NoPlayerTilesResult();
            }

            ProcessCandidates();

            if (ValidGoes.Count > 0)
            {
                int bestScore = ValidGoes.Max(g => g.Score);
                var bestGoes = ValidGoes.Where(g => g.Score == bestScore).ToList();
                var go = bestGoes[random.Next(bestGoes.Count)];
                goPlacer.PlaceGo(go, game);
                return BestGoResult(game);
            }

            return NoGoesResult();
        }

        public List<AiValidGo> ValidGoes { get; set; }

        AiCandidate currentCandidate;
        IEnumerable<string> currentMainWords;

        static Random random = new Random();

        void ProcessCandidates()
        {
            var playerLetters = GetPlayerLettersString();
            foreach (var candidate in aiGridModel.Candidates)
            {
                currentCandidate = candidate;
                currentMainWords = wordFinder.FindWords(candidate.SearchPattern, playerLetters);
                ProcessMainWords();
            }
        }

        string GetPlayerLettersString()
        {
            if (aiGridModel.PlayerTiles.Count == 0)
            {
                return null;
            }

            string letters = string.Empty;
            foreach (var tile in aiGridModel.PlayerTiles)
            {
                letters += tile.Letter;
            }

            return letters.ToLower();
        }

        void ProcessMainWords()
        {
            foreach (var mainWord in currentMainWords)
            {
                ProcessMainWord(mainWord);
            }
        }

        void ProcessMainWord(string mainWord)
        {
            var goWords = goWordFinder.FindWords(mainWord, currentCandidate);

            if (goWords.All(w => wordValidator.IsWord(w.Word)))
            {
                var score = goScorer.ScoreGo(goWords);
                ValidGoes.Add(new AiValidGo()
                {
                    MainWord = mainWord.ToUpper(),
                    Candidate = currentCandidate,
                    GoWords = goWords,
                    Score = score
                });
            }
        }

        GoResult NoPlayerTilesResult()
        {
            return new GoResult()
            {
                Message = "Player does not have any tiles.",
                IsValid = false
            };
        }

        GoResult NoGoesResult()
        {
            return new GoResult()
            {
                Message = "Cannot find any places to go.",
                IsValid = false
            };
        }

        GoResult BestGoResult(Game game)
        {
            return new GoResult()
            {
                IsValid = true,
                Game = game
            };
        }
    }
}
