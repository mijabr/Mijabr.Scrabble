using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;
using words;

namespace words.tests
{
    [TestClass]
    public class WordDictionaryPerfTests
    {
        FileSystem fileSystem;
        WordDictionary dict;

        [TestInitialize]
        public void Setup()
        {
            fileSystem = new FileSystem();
            dict = new WordDictionary(fileSystem);
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void LoadDictionaryTest()
        {
            dict.LoadFile("dictionary.txt");
            dict.IsWord("Apple").ShouldBe(true);
            dict.IsWord("yummalicious").ShouldBe(false);
        }
    }
}
