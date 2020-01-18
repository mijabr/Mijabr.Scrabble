using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Go
{
    public class GoWordFinder : IGoWordFinder
    {
        IGridModel gridModel;
        public GoWordFinder(IGridModel gridModel)
        {
            this.gridModel = gridModel;
        }

        public IEnumerable<GoWord> FindWords()
        {
            words = new List<GoWord>();

            mainNav = new GoNavigator(gridModel.GoStartX, gridModel.GoStartY, gridModel.IsHorizontalGo);

            FindMainWord();

            return words;
        }

        List<GoWord> words;
        GoNavigator mainNav;
        GoNavigator sideNav;

        void FindMainWord()
        {
            string word = string.Empty;
            var goLetters = new List<GoLetter>();

            while (mainNav.Main < 15 && !gridModel.Grid[mainNav.X, mainNav.Y].IsEmpty())
            {
                word += gridModel.Grid[mainNav.X, mainNav.Y].Letter;
                goLetters.Add(gridModel.GetGoLetter(mainNav.X, mainNav.Y));

                if (gridModel.Grid[mainNav.X, mainNav.Y].Origin == GridModelTileOrigin.FromPlayer)
                {
                    FindSideWord();
                }

                mainNav.Main++;
            }

            AddMainWord(word, goLetters);
        }

        void FindSideWord()
        {
            sideNav.Copy(mainNav);
            MoveToSideWordStart();
            (string word, List<GoLetter> goLetters) side = GetSideWord();
            AddSideWord(side.word, side.goLetters);
        }

        void MoveToSideWordStart()
        {
            if (sideNav.Side >= 0)
            {
                sideNav.Side--;
                while (sideNav.Side >= 0 && !gridModel.Grid[sideNav.X, sideNav.Y].IsEmpty())
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

            while (sideNav.Side < 15 && !gridModel.Grid[sideNav.X, sideNav.Y].IsEmpty())
            {
                word += gridModel.Grid[sideNav.X, sideNav.Y].Letter;
                goLetters.Add(gridModel.GetGoLetter(sideNav.X, sideNav.Y));
                sideNav.Side++;
            }

            return (word, goLetters);
        }

        void AddMainWord(string word, List<GoLetter> goLetters)
        {
            if (word.Length > 0)
            {
                words.Insert(0, new GoWord()
                {
                    Word = word,
                    GoLetters = goLetters
                });
            }
        }

        void AddSideWord(string word, List<GoLetter> goLetters)
        {
            if (word.Length > 1)
            {
                words.Add(new GoWord()
                {
                    Word = word,
                    GoLetters = goLetters
                });
            }
        }
    }
}
