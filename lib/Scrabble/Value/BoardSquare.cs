using System;

namespace Scrabble.Value
{
    public struct BoardSquare
    {
        public BoardSquare(int x, int y, string name, int letterBonus, int wordBonus)
        {
            X = x;
            Y = y;
            Name = name;
            LetterBonus = letterBonus;
            WordBonus = wordBonus;
        }

        public int X { get; }
        public int Y { get; }
        public string Name { get; private set;}
        public int LetterBonus { get; private set; }
        public int WordBonus { get; private set; }

        public static BoardSquare NormalSquare(int x = 0, int y = 0)
        {
            return new BoardSquare(x, y, "NS", 1, 1);
        }

        public static BoardSquare DoubleLetterSquare(int x = 0, int y = 0)
        {
            return new BoardSquare(x, y, "DL", 2, 1);
        }

        public static BoardSquare TripleLetterSquare(int x = 0, int y = 0)
        {
            return new BoardSquare(x, y, "TL", 3, 1);
        }

        public static BoardSquare DoubleWordSquare(int x = 0, int y = 0)
        {
            return new BoardSquare(x, y, "DW", 1, 2);
        }

        public static BoardSquare TripleWordSquare(int x = 0, int y = 0)
        {
            return new BoardSquare(x, y, "TW", 1, 3);
        }

        public static BoardSquare StartingSquare(int x = 0, int y = 0)
        {
            return new BoardSquare(x, y, "ST", 1, 2);
        }
    }
}
