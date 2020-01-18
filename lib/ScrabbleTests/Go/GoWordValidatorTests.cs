using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mijabr.Language;
using NSubstitute;
using Scrabble.Go;
using Scrabble.Value;
using Shouldly;
using System.Collections.Generic;
using words;

namespace Scrabble.Tests
{
    [TestClass]
    public class GoWordValidatorTests
    {
        WordValidatable wordValidator;
        IItemLister itemLister;
        GoWordValidator goWordValidator;
        List<GoWord> goWords;
        GoValidationResult result;

        [TestInitialize]
        public void Setup()
        {
            wordValidator = Substitute.For<WordValidatable>();
            itemLister = Substitute.For<IItemLister>();
            goWordValidator = new GoWordValidator(wordValidator, itemLister);
            wordValidator.IsWord("SUPER").Returns(true);
            goWords = new List<GoWord>();
        }

        void AssertGoIsInvalidWithMessage(string message)
        {
            result.IsValid.ShouldBeFalse();
            result.Message.ShouldBe(message);
        }

        [TestMethod]
        public void GivenNoWords_ThenGoIsInvalid()
        {
            result = goWordValidator.ValidateWords(goWords);
            AssertGoIsInvalidWithMessage("No words were made!");
        }

        [TestMethod]
        public void GivenAValidWord_ThenGoIsValid()
        {
            goWords.Add(new GoWord() { Word = "SUPER" });
            var result = goWordValidator.ValidateWords(goWords);
            result.IsValid.ShouldBeTrue();
        }

        [TestMethod]
        public void GivenAnInvalidWord_ThenGoIsInvalid()
        {
            goWords.Add(new GoWord() { Word = "ZUPER" });
            var result = goWordValidator.ValidateWords(goWords);
            result.IsValid.ShouldBeFalse();
        }

        [TestMethod]
        public void GivenAnInvalidWord_ThenMessageIsInvalidWord()
        {
            goWords.Add(new GoWord() { Word = "ZUPER" });
            itemLister.ToString(Arg.Any<IEnumerable<string>>()).Returns("ZUPER");
            var result = goWordValidator.ValidateWords(goWords);
            result.Message.ShouldBe("ZUPER is not a valid word");
        }

        [TestMethod]
        public void GivenAGoWithTwoInvalidWords_ThenMessageIsPluralised()
        {
            goWords.Add(new GoWord() { Word = "ZUPER" });
            goWords.Add(new GoWord() { Word = "DUPER" });
            itemLister.ToString(Arg.Any<IEnumerable<string>>()).Returns("ZUPER and DUPER");
            var result = goWordValidator.ValidateWords(goWords);
            result.IsValid.ShouldBeFalse();
            result.Message.ShouldBe("ZUPER and DUPER are not valid words");
        }
    }
}
