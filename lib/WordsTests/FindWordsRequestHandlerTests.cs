using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Shouldly;
using words;

namespace words.tests
{
    [TestClass]
    public class FindWordsRequestHandlerTests
    {
        WordFindable wordFinder;
        FindWordsRequestHandler requestHandler;

        [TestInitialize]
        public void Setup()
        {
            wordFinder = Substitute.For<WordFindable>();
            requestHandler = new FindWordsRequestHandler(wordFinder);
        }

        [TestMethod]
        public void GivenAMessage_ThenWordFinderIsCalled()
        {
            var request = new FindWordsRequestMessage() { Pattern = "$pple" };
            requestHandler.FindWords(request);
            wordFinder.Received(1).FindWords("$pple");
        }

        [TestMethod]
        public void GivenAMessageWithLetters_ThenWordFinderIsCalledWithLetters()
        {
            var request = new FindWordsRequestMessage() { Pattern = "$pple", Letters = "a" };
            requestHandler.FindWords(request);
            wordFinder.Received(1).FindWords("$pple", "a");
        }

        [TestMethod]
        public void GivenMixedCasePattern_ThenSearchPatternInReponseIsLowerCase()
        {
            var request = new FindWordsRequestMessage() { Pattern = "$PPle" };
            var response = requestHandler.FindWords(request);
            response.SearchPattern.ShouldBe("$pple");
        }

        [TestMethod]
        public void GivenMixedCaseLetters_ThenLettersInReponseAreLowerCase()
        {
            var request = new FindWordsRequestMessage() { Pattern = "$pple", Letters = "A" };
            var response = requestHandler.FindWords(request);
            response.Letters.ShouldBe("a");
        }
    }
}
