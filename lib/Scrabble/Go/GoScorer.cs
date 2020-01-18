using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Go
{
    public class GoScorer : IGoScorer
    {
        public int ScoreGo(IEnumerable<GoWord> goWords)
        {
            int score = 0;
            foreach (var goWord in goWords)
            {
                var wordScore = 0;
                var wordBonus = 1;

                foreach (var goLetter in goWord.GoLetters)
                {
                    wordScore += goLetter.TileValue * goLetter.LetterBonus;
                    wordBonus *= goLetter.WordBonus;
                }

                score += wordScore * wordBonus;
            }

            return score;
        }
    }
}
