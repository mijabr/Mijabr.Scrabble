using Mijabr.Language;
using Scrabble.Value;
using System.Collections.Generic;
using System.Linq;
using words;

namespace Scrabble.Go
{
    public class GoWordValidator : IGoWordValidator
    {
        WordValidatable wordValidator;
        IItemLister itemLister;
        public GoWordValidator(WordValidatable wordValidator, IItemLister itemLister)
        {
            this.wordValidator = wordValidator;
            this.itemLister = itemLister;
        }

        public GoValidationResult ValidateWords(IEnumerable<GoWord> goWords)
        {
            if (goWords.Count() == 0)
            {
                return NoWordsErrorResult();
            }

            return CheckWords(goWords);
        }

        GoValidationResult CheckWords(IEnumerable<GoWord> goWords)
        {
            var invalidWords = new List<string>();
            foreach (var goWord in goWords)
            {
                if (!wordValidator.IsWord(goWord.Word))
                {
                    invalidWords.Add(goWord.Word);
                }
            }

            if (invalidWords.Count() > 0)
            {
                string words = itemLister.ToString(invalidWords);
                string areInvalid = (invalidWords.Count() == 1) ? " is not a valid word" : " are not valid words";
                return new GoValidationResult()
                {
                    IsValid = false,
                    Message = $"{words}{areInvalid}"
                };
            }

            return SuccessResult();
        }

        static GoValidationResult SuccessResult()
        {
            return new GoValidationResult()
            {
                IsValid = true
            };
        }

        static GoValidationResult NoWordsErrorResult()
        {
            return new GoValidationResult()
            {
                IsValid = false,
                Message = "No words were made!"
            };
        }
    }
}
