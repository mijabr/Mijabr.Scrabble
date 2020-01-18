using Scrabble.Value;
using System.Collections.Generic;

namespace Scrabble.Go
{
    public interface IGoWordValidator
    {
        GoValidationResult ValidateWords(IEnumerable<GoWord> goWords);
    }
}
