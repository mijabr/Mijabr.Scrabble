using System;
using System.Collections.Generic;
using System.Linq;

namespace words
{
    internal class LetterNode
    {
        internal LetterNode(LetterNode parent = null)
        {
            this.parent = parent;
        }

        LetterNode parent;
        string letter { get; set; }
        Dictionary<char, LetterNode> nextLetter { get; } = new Dictionary<char, LetterNode>();

        internal bool IsWordEnd { get; private set; }

        internal void AddWord(string word)
        {
            if (word.Length == 0)
            {
                IsWordEnd = true;
                return;
            }

            var next = FindOrCreateNextLetterNode(word[0]);

            next.AddWord(word.Substring(1));
        }

        internal bool IsWord(string pattern)
        {
            if (pattern.Length == 0)
            {
                return IsWordEnd;
            }

            if (pattern[0] == '-')
            {
                return true;
            }

            if (nextLetter.TryGetValue(pattern[0], out LetterNode next))
            {
                return next.IsWord(pattern.Substring(1));
            }

            return false;
        }

        internal void FindWords(string pattern, List<string> words, List<char> letters = null)
        {
            if (IsEndOfSearchPatternAddWord(pattern, words))
            {
                return;
            }

            var selectedLetters = nextLetter.Where(kv => IsPatternMatch(pattern, kv.Key, letters));
            foreach (var letter in selectedLetters)
            {
                letter.Value.FindWords(pattern.Substring(1), words, GetRemainingLetters(letters, letter.Key));
            }
        }

        bool IsEndOfSearchPatternAddWord(string pattern, List<string> words)
        {
            if (pattern.Length == 0)
            {
                if (IsWordEnd)
                {
                    words.Add(GetWord());
                }

                return true;
            }

            return false;
        }

        string GetWord()
        {
            if (parent == null)
            {
                return letter;
            }

            return $"{parent.GetWord()}{letter}";
        }

        List<char> GetRemainingLetters(List<char> letters, char letter)
        {
            List<char> lettersRemaining = null;
            if (letters != null)
            {
                lettersRemaining = letters.ToList();
                lettersRemaining.Remove(letter);
            }

            return lettersRemaining;
        }

        bool IsPatternMatch(string pattern, char letter, List<char> letters)
        {
            if (pattern[0] == letter)
            {
                return true;
            }

            if (pattern[0] == '$')
            {
                if (letters != null)
                {
                    return letters.Contains(letter);
                }

                return true;
            }

            return false;
        }

        LetterNode FindOrCreateNextLetterNode(char nextLetter)
        {
            if (!this.nextLetter.TryGetValue(nextLetter, out LetterNode next))
            {
                next = new LetterNode(this)
                {
                    letter = nextLetter.ToString()
                };
                this.nextLetter.Add(nextLetter, next);
            }

            return next;
        }

    }
}
