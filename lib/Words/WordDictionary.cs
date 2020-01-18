using System.Collections.Generic;
using System.Linq;
using System.IO.Abstractions;
using System.IO;
using System;

namespace words
{
    public class WordDictionary : WordValidatable, WordFindable
    {
        private readonly IFileSystem fileSystem;

        public WordDictionary(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        LetterNode root = new LetterNode();

        public void AddWords(params string[] words)
        {
            words.ToList().ForEach(w => AddWord(w));
        }

        public void AddWord(string word)
        {
            if (!string.IsNullOrEmpty(word))
                root.AddWord(word.ToLower());
        }

        public bool IsWord(string word)
        {
            return root.IsWord(word.ToLower());
        }

        public IEnumerable<string> FindWords(string pattern, string letters = null)
        {
            if (letters?.Length == 0)
            {
                letters = null;
            }

            pattern = pattern.Replace('?', '$');

            var words = new List<string>();
            root.FindWords(pattern.ToLower(), words, letters?.ToList());
            return words;
        }

        public bool IsWordStart(string word)
        {
            return root.IsWord($"{word.ToLower()}-");
        }

        public void LoadFile(string filename)
        {
            try
            {
                var words = fileSystem.File.ReadAllLines(filename);
                foreach (var word in words)
                {
                    AddWord(word.ToLower());
                }
                Console.WriteLine($"Loaded dictionary file {filename} containing {words.Length} words");
            }
            catch (FileNotFoundException x)
            {
                Console.WriteLine($"Could not locate dictionary file {filename} in {Directory.GetCurrentDirectory()}");
            }
        }
    }
}
