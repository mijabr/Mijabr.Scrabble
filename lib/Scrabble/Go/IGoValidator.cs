namespace Scrabble.Go
{
    public interface IGoValidator
    {
        GoValidationResult ValidateGo(Validatable validatable);
    }
}
