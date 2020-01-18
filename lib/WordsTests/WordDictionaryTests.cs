using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

namespace words.tests
{
    [TestClass]
    public class WordDictionaryTests
    {
        MockFileSystem fileSystem;
        WordDictionary dict;

        [TestInitialize]
        public void Setup()
        {
            fileSystem = new MockFileSystem();
            dict = new WordDictionary(fileSystem);
        }

        [TestMethod]
        public void GivenEmptyWord_ThenEmptyWordIsNotFound()
        {
            dict.AddWord("");
            dict.IsWord("").ShouldBe(false);
        }

        [TestMethod]
        public void GivenSingleLetterWord_ThenWordIsFound()
        {
            dict.AddWord("a");
            dict.IsWord("a").ShouldBe(true);
        }

        [TestMethod]
        public void GivenSingleLetterWord_ThenOtherSingleLetterWordsAreNotFound()
        {
            dict.AddWord("a");
            dict.IsWord("b").ShouldBe(false);
        }

        [TestMethod]
        public void GivenWords_ThenWordsAreFound()
        {
            dict.AddWords("apple", "above", "arc");
            dict.IsWord("apple").ShouldBe(true);
            dict.IsWord("above").ShouldBe(true);
            dict.IsWord("arc").ShouldBe(true);
        }

        [TestMethod]
        public void GivenWords_ThenOtherWordsAreNotFound()
        {
            dict.AddWords("apple", "above", "arc");
            dict.IsWord("ar").ShouldBe(false);
            dict.IsWord("bubble").ShouldBe(false);
            dict.IsWord("trouble").ShouldBe(false);
        }

        [TestMethod]
        public void GivenWords_WhenTestForStartingLetter_ThenShouldFindWordsStartWithLetter()
        {
            dict.AddWords("a", "purple", "car");
            dict.IsWordStart("a").ShouldBe(true);
            dict.IsWordStart("p").ShouldBe(true);
            dict.IsWordStart("c").ShouldBe(true);
        }

        [TestMethod]
        public void GivenWords_WhenTestForStartingLetter_ThenShouldNotFindWordsStartWithLetter()
        {
            dict.AddWords("a", "purple", "car");
            dict.IsWordStart("g").ShouldBe(false);
            dict.IsWordStart("x").ShouldBe(false);
            dict.IsWordStart("q").ShouldBe(false);
        }

        [TestMethod]
        public void GivenWords_WhenTestForStartingLetters_ThenShouldFindWordsStartWithLetters()
        {
            dict.AddWords("crash", "bang", "boom");
            dict.IsWordStart("cras").ShouldBe(true);
            dict.IsWordStart("b").ShouldBe(true);
            dict.IsWordStart("boom").ShouldBe(true);
        }

        [TestMethod]
        public void GivenWords_WhenTestForStartingLetters_ThenShouldNotFindWordsStartWithLetters()
        {
            dict.AddWords("crash", "bang", "boom");
            dict.IsWordStart("pow").ShouldBe(false);
            dict.IsWordStart("slap").ShouldBe(false);
            dict.IsWordStart("a").ShouldBe(false);
        }

        [TestMethod]
        public void GivenDictionaryFile_ThenWordsFromFileShouldBeAdded()
        {
            string dictionaryText = @"red
ruby
blue";
            fileSystem.AddFile("dictionary.txt", new MockFileData(dictionaryText));
            dict.LoadFile("dictionary.txt");
            dict.IsWord("red").ShouldBe(true);
            dict.IsWord("ruby").ShouldBe(true);
            dict.IsWord("blue").ShouldBe(true);
            dict.IsWord("white").ShouldBe(false);
            dict.IsWord("diamond").ShouldBe(false);
        }

        [TestMethod]
        public void GivenMixedCaseWords_ThenWordsShouldBeFound()
        {
            dict.AddWords("UP", "Down", "LefT");
            dict.IsWord("up").ShouldBe(true, "up is a word");
            dict.IsWord("down").ShouldBe(true, "down is a word");
            dict.IsWord("LEFT").ShouldBe(true, "left is a word");
        }

        [TestMethod]
        public void GivenSingleLetterWords_WhenFindWords_ThenMatchingSingleLetterWordIsReturned()
        {
            dict.AddWords("a", "i", "q");
            var words = dict.FindWords("a");
            words.ShouldContain("a");
            words.Count().ShouldBe(1);
        }

        [TestMethod]
        public void GivenWords_WhenFindWordsStartingWithLetter_ThenMatchingWordsAreReturned()
        {
            dict.AddWords("apple", "ape", "apply", "beacon", "butterfly", "butter", "best");
            var words = dict.FindWords("a$$");
            words.ShouldContain("ape");
            words.Count().ShouldBe(1);
        }

        [TestMethod]
        public void GivenWords_WhenFindWordsWithMiddlePattern_ThenMatchingWordsAreReturned()
        {
            dict.AddWords("apple", "ape", "apply", "beacon", "butterfly", "butter", "best");
            var words = dict.FindWords("$pp$$");
            words.ShouldContain("apple");
            words.ShouldContain("apply");
            words.Count().ShouldBe(2);
        }

        [TestMethod]
        public void GivenWords_WhenFindWordsWithMixedCase_ThenMatchingWordsAreReturned()
        {
            dict.AddWords("apple", "ape", "apply", "beacon", "butterfly", "butter", "best");
            var words = dict.FindWords("$pP$$");
            words.ShouldContain("apple");
            words.ShouldContain("apply");
            words.Count().ShouldBe(2);
        }

        [TestMethod]
        public void GivenWords_WhenFindWordsWithRestrictedLetters_ThenMatchingWordsAreReturned()
        {
            dict.AddWords("apple", "ape", "apply", "beacon", "butterfly", "butter", "best");
            var words = dict.FindWords("$pp$$", "aly");
            words.ShouldContain("apply");
            words.Count().ShouldBe(1);
        }

        [TestMethod]
        public void GivenDoubleLetterWords_WhenFindWordsWithOneLetter_ThenWordsAreNotFound()
        {
            dict.AddWords("apple", "apply");
            var words = dict.FindWords("a$$l$", "pey");
            words.Count().ShouldBe(0);
        }

        [TestMethod]
        public void GivenNoLetters_WhenFindWords_NoLetterRestrictionIsApplied()
        {
            dict.AddWords("apple", "apply");
            var words = dict.FindWords("a$$l$", "");
            words.Count().ShouldBe(2);
        }
    }
}
