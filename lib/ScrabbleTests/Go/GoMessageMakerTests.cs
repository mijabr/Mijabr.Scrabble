using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mijabr.Language;
using Scrabble.Go;
using Scrabble.Value;
using Shouldly;
using System.Collections.Generic;

namespace Scrabble.Tests
{
    [TestClass]
    public class GoMessageMakerTests
    {
        GoMessageMaker maker;
        List<GoWord> goWords;
        int goScore;
        string goMessage;

        [TestInitialize]
        public void CanCreateGoMessageMaker()
        {
            var itemLister = new ItemLister();
            maker = new GoMessageMaker(itemLister);
            goWords = new List<GoWord>();
            goScore = 0;
        }

        void GivenTheWords(params string[] words)
        {
            foreach (var word in words)
            {
                goWords.Add(new GoWord() { Word = word });
            }
        }

        void GivenAScoreOf(int score)
        {
            goScore = score;
        }

        void WhenGetGoMessage()
        {
            goMessage = maker.GetGoMessage("NAME", goWords, goScore);
        }

        [TestMethod]
        public void GivenNoWords_ThenMessageIsYouMadeNoWords()
        {
            WhenGetGoMessage();
            goMessage.ShouldBe("You made no words.");
        }

        [TestMethod]
        public void GivenOneWord_ThenMessageIsYouMadeAWord()
        {
            GivenTheWords("CAT");
            WhenGetGoMessage();
            goMessage.ShouldBe("NAME's word is CAT.");
        }

        [TestMethod]
        public void GivenTwoWords_ThenMessageShouldBeYourWordsAre()
        {
            GivenTheWords("CAT", "HAT");
            WhenGetGoMessage();
            goMessage.ShouldBe("NAME's words are CAT and HAT.");
        }

        [TestMethod]
        public void GivenAScore_ThenMessageShouleBeYouScored()
        {
            GivenAScoreOf(1);
            WhenGetGoMessage();
            goMessage.Contains("NAME scored 1.").ShouldBeTrue();
        }

        [TestMethod]
        public void GivenSomeWordsAndAScore_ThenMessageShouldBeYouMadeWordsAndScored()
        {
            GivenAScoreOf(1);
            GivenTheWords("CAT", "HAT");
            WhenGetGoMessage();
            goMessage.ShouldBe("NAME's words are CAT and HAT. NAME scored 1.");
        }
    }
}
