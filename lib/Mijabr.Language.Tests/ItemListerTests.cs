using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace Mijabr.Language.Tests
{
    [TestFixture]
    public class ItemListerTests
    {
        ItemLister itemLister;
        IEnumerable<string> words;
        string english;

        [SetUp]
        public void SetUp()
        {
            itemLister = new ItemLister();
        }

        void GivenWords(params string[] words)
        {
            this.words = words;
        }

        void WhenCreateLanguageList()
        {
            english = itemLister.ToString(words);
        }

        [Test]
        public void GivenListWithNoItems_ThenEmptyIsReturned()
        {
            WhenCreateLanguageList();
            english.ShouldBe("");
        }

        [Test]
        public void GivenListWithOneItems_ThenWordIsReturned()
        {
            GivenWords("one");
            WhenCreateLanguageList();
            english.ShouldBe("one");
        }

        [Test]
        public void GivenListWithTwoItems_ThenOneAndTwoIsReturned()
        {
            GivenWords("one", "two");
            WhenCreateLanguageList();
            english.ShouldBe("one and two");
        }

        [Test]
        public void GivenListWithThreeItems_ThenOneTwoAndThreeIsReturned()
        {
            GivenWords("one", "two", "three");
            WhenCreateLanguageList();
            english.ShouldBe("one, two and three");
        }

        [Test]
        public void GivenListWithFourItems_ThenOneTwoThreeAndFourIsReturned()
        {
            GivenWords("one", "two", "three", "four");
            WhenCreateLanguageList();
            english.ShouldBe("one, two, three and four");
        }
    }
}
