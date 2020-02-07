using Scrabble.Go;
using Scrabble.Value;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Ai
{
    public class AiGoWordFinder : IAiGoWordFinder
    {
        private readonly IAiGridModel gridModel;
        private readonly Board board;

        public AiGoWordFinder(IAiGridModel gridModel, Board board)
        {
            this.gridModel = gridModel;
            this.board = board;
        }

        public IEnumerable<GoWord> FindWords(string theMainWord, AiCandidate theCandidate)
        {
            goWords = new List<GoWord>();
            if (string.IsNullOrEmpty(theMainWord)) return goWords;

            this.mainWord = theMainWord;
            this.candidate = theCandidate;
            FindWords();

            return goWords;
        }

        private List<GoWord> goWords;
        private string mainWord;
        private List<GoLetter> mainGoLetters;
        private AiCandidate candidate;
        private GoNavigator mainNav;
        private GoNavigator sideNav;
        private int patternPos;
        private char currentPatternChar;

        private void FindWords()
        {
            mainNav = new GoNavigator(candidate.StartX, candidate.StartY, candidate.Orientation == 0);

            TraverseMainWord();
        }

        private void TraverseMainWord()
        {
            mainGoLetters = new List<GoLetter>();
            patternPos = 0;
            while (patternPos < candidate.SearchPattern.Length)
            {
                currentPatternChar = candidate.SearchPattern[patternPos];

                FindSideWord();

                mainGoLetters.Add(GetGoLetter(mainNav.X, mainNav.Y, currentPatternChar == '?'));

                patternPos++;
                mainNav.Main++;
            }

            AddMainWord();
        }

        private void FindSideWord()
        {
            if (currentPatternChar == '?')
            {
                sideNav.Copy(mainNav);
                MoveToSideWordStart();
                (string word, List<GoLetter> goLetters) side = GetSideWord();
                AddSideWord(side.word, side.goLetters);
            }
        }

        private void MoveToSideWordStart()
        {
            if (sideNav.Side <= 0) return;

            sideNav.Side--;
            while (sideNav.Side > 0 && gridModel.Grid[sideNav.X, sideNav.Y].Letter != 0)
            {
                sideNav.Side--;
            }

            sideNav.Side++;
        }

        private (string, List<GoLetter>) GetSideWord()
        {
            var word = string.Empty;
            var goLetters = new List<GoLetter>();

            while (sideNav.Side < 15 && GetSideLetter() != 0)
            {
                word += GetSideLetter();
                goLetters.Add(GetGoLetter(sideNav.X, sideNav.Y, sideNav.X == mainNav.X && sideNav.Y == mainNav.Y));
                sideNav.Side++;
            }

            return (word, goLetters);
        }

        private char GetSideLetter()
        {
            if (sideNav.X == mainNav.X && sideNav.Y == mainNav.Y)
            {
                return mainWord[patternPos];
            }
            else
            {
                return gridModel.Grid[sideNav.X, sideNav.Y].Letter;
            }
        }

        private GoLetter GetGoLetter(int x, int y, bool isPlayerTile)
        {
            return isPlayerTile ? GetPlayerGoLetter(x, y) : GetBoardGoLetter(x, y);
        }

        private GoLetter GetPlayerGoLetter(int x, int y)
        {
            return new GoLetter()
            {
                TileValue = gridModel.PlayerTiles?.FirstOrDefault(t => t.Letter == mainWord[patternPos]).Value ?? 1,
                LetterBonus = board.GetSquare(x, y).LetterBonus,
                WordBonus = board.GetSquare(x, y).WordBonus
            };
        }

        private GoLetter GetBoardGoLetter(int x, int y)
        {
            return new GoLetter()
            {
                TileValue = gridModel.Grid[x, y].TileValue,
                LetterBonus = 1,
                WordBonus = 1
            };
        }

        private void AddMainWord()
        {
            goWords.Add(new GoWord()
            {
                Word = mainWord,
                GoLetters = mainGoLetters
            });
        }

        private void AddSideWord(string word, List<GoLetter> goLetters)
        {
            if (word.Length > 1)
            {
                goWords.Add(new GoWord()
                {
                    Word = word,
                    GoLetters = goLetters
                });
            }
        }
    }
}
