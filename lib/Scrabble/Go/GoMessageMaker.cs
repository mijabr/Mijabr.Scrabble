using Mijabr.Language;
using Scrabble.Value;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Go
{
    public class GoMessageMaker : IGoMessageMaker
    {
        IItemLister itemLister;
        public GoMessageMaker(IItemLister itemLister)
        {
            this.itemLister = itemLister;
        }

        public string GetGoMessage(string playerName, IEnumerable<GoWord> goWords, int goScore)
        {
            var words = GetWordsList(goWords);
            var message = string.Empty;

            if (words.Count() == 0)
            {
                message = "You made no words.";
            }
            else if (words.Count() == 1)
            {
                message = $"{playerName}'s word is {words.First()}.";
            }
            else
            {
                message = $"{playerName}'s words are {itemLister.ToString(words)}.";
            }

            if (goScore > 0)
            {
                message += $" {playerName} scored {goScore}.";
            }

            return message;
        }

        List<string> GetWordsList(IEnumerable<GoWord> goWords)
        {
            var words = new List<string>();
            foreach (var goWord in goWords)
            {
                words.Add(goWord.Word);
            }

            return words;
        }
    }
}
