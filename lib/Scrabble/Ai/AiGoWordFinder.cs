using Scrabble.Go;
using Scrabble.Value;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Ai
{
    public class AiGoWordFinder : IAiGoWordFinder
    {
        IAiGridModel gridModel;
        Board board;
        public AiGoWordFinder(IAiGridModel gridModel, Board board)
        {
            this.gridModel = gridModel;
            this.board = board;
        }

        public IEnumerable<GoWord> FindWords(string mainWord, AiCandidate candidate)
        {
            goWords = new List<GoWord>();
            if (mainWord != null && mainWord.Length > 0)
            {
                this.mainWord = mainWord;
                this.candidate = candidate;
                FindWords();
            }

            return goWords;
        }

        List<GoWord> goWords;
        string mainWord;
        List<GoLetter> mainGoLetters;
        AiCandidate candidate;
        GoNavigator mainNav;
        GoNavigator sideNav;
        int patternPos;
        char currentPatternChar;

        void FindWords()
        {
            mainNav = new GoNavigator(candidate.StartX, candidate.StartY, candidate.Orientation == 0);

            TraverseMainWord();
        }

        void TraverseMainWord()
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

        void FindSideWord()
        {
            if (currentPatternChar == '?')
            {
                sideNav.Copy(mainNav);
                MoveToSideWordStart();
                (string word, List<GoLetter> goLetters) side = GetSideWord();
                AddSideWord(side.word, side.goLetters);
            }
        }

        void MoveToSideWordStart()
        {
            if (sideNav.Side > 0)
            {
                sideNav.Side--;
                while (sideNav.Side > 0 && gridModel.Grid[sideNav.X, sideNav.Y].Letter != 0)
                {
                    sideNav.Side--;
                }

                sideNav.Side++;
            }
        }

        (string, List<GoLetter>) GetSideWord()
        {
            string word = string.Empty;
            var goLetters = new List<GoLetter>();

            while (sideNav.Side < 15 && GetSideLetter() != 0)
            {
                word += GetSideLetter();
                goLetters.Add(GetGoLetter(sideNav.X, sideNav.Y, sideNav.X == mainNav.X && sideNav.Y == mainNav.Y));
                sideNav.Side++;
            }

            return (word, goLetters);
        }

        char GetSideLetter()
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

        GoLetter GetGoLetter(int x, int y, bool isPlayerTile)
        {
            if (isPlayerTile)
            {
                return GetPlayerGoLetter(x, y);
            }
            else
            {
                return GetBoardGoLetter(x, y);
            }
        }

        GoLetter GetPlayerGoLetter(int x, int y)
        {
            return new GoLetter()
            {
                TileValue = gridModel.PlayerTiles?.FirstOrDefault(t => t.Letter == mainWord[patternPos]).Value ?? 1,
                LetterBonus = board.GetSquare(x, y).LetterBonus,
                WordBonus = board.GetSquare(x, y).WordBonus
            };
        }

        GoLetter GetBoardGoLetter(int x, int y)
        {
            return new GoLetter()
            {
                TileValue = gridModel.Grid[x, y].TileValue,
                LetterBonus = 1,
                WordBonus = 1
            };
        }

        void AddMainWord()
        {
            goWords.Add(new GoWord()
            {
                Word = mainWord,
                GoLetters = mainGoLetters
            });
        }

        void AddSideWord(string word, List<GoLetter> goLetters)
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
