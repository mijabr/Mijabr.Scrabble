using Mijabr.Language;
using Scrabble.Value;
using System.Collections.Generic;
using System.Linq;
using words;

namespace Scrabble.Go
{
    public class GoWordValidator : IGoWordValidator
    {
        private readonly WordValidatable wordValidator;
        private readonly IItemLister itemLister;

        public GoWordValidator(WordValidatable wordValidator, IItemLister itemLister)
        {
            this.wordValidator = wordValidator;
            this.itemLister = itemLister;
        }

        public GoValidationResult ValidateWords(IEnumerable<GoWord> goWords)
        {
            if (!goWords.Any())
            {
                return NoWordsErrorResult();
            }

            return CheckWords(goWords);
        }

        private GoValidationResult CheckWords(IEnumerable<GoWord> goWords)
        {
            var invalidWords = new List<string>();
            foreach (var goWord in goWords)
            {
                if (!wordValidator.IsWord(goWord.Word))
                {
                    invalidWords.Add(goWord.Word);
                }
            }

            if (!invalidWords.Any()) return SuccessResult();

            var words = itemLister.ToString(invalidWords);
            var areInvalid = (invalidWords.Count() == 1) ? " is not a valid word" : " are not valid words";
            return new GoValidationResult()
            {
                IsValid = false,
                Message = $"{words}{areInvalid}"
            };

        }

        private static GoValidationResult SuccessResult()
        {
            return new GoValidationResult
            {
                IsValid = true
            };
        }

        private static GoValidationResult NoWordsErrorResult()
        {
            return new GoValidationResult
            {
                IsValid = false,
                Message = "No words were made!"
            };
        }
    }
}
