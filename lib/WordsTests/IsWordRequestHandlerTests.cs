using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Shouldly;
using words;

namespace words.tests
{
    [TestClass]
    public class IsWordRequestHandlerTests
    {
        WordValidatable wordValidator;
        IsWordRequestHandler requestHandler;

        [TestInitialize]
        public void Setup()
        {
            wordValidator = Substitute.For<WordValidatable>();
            requestHandler = new IsWordRequestHandler(wordValidator);
        }

        [TestMethod]
        public void GivenAMessage_ThenWordValidatorIsCalled()
        {
            var message = new IsWordRequestMessage() { Word = "plus" };
            requestHandler.IsWord(message);
            wordValidator.Received(1).IsWord("plus");
        }

        [TestMethod]
        public void GivenMixedCaseWord_ThenCheckedWordInReponseIsLowerCase()
        {
            var request = new IsWordRequestMessage() { Word = "Plus" };
            var response = requestHandler.IsWord(request);
            response.CheckedWord.ShouldBe("plus");
        }
    }
}
