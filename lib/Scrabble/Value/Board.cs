using System.Collections.Generic;

namespace Scrabble.Value
{
    public class Board
    {
        public Board()
        {
            Squares = new List<BoardSquare>()
            {
                TW(), NS(), NS(), DL(), NS(), NS(), NS(), TW(), NS(), NS(), NS(), DL(), NS(), NS(), TW(),
                NS(), DW(), NS(), NS(), NS(), TL(), NS(), NS(), NS(), TL(), NS(), NS(), NS(), DW(), NS(),
                NS(), NS(), DW(), NS(), NS(), NS(), DL(), NS(), DL(), NS(), NS(), NS(), DW(), NS(), NS(),
                DL(), NS(), NS(), DW(), NS(), NS(), NS(), DL(), NS(), NS(), NS(), DW(), NS(), NS(), DL(),
                NS(), NS(), NS(), NS(), DW(), NS(), NS(), NS(), NS(), NS(), DW(), NS(), NS(), NS(), NS(),
                NS(), TL(), NS(), NS(), NS(), TL(), NS(), NS(), NS(), TL(), NS(), NS(), NS(), TL(), NS(),
                NS(), NS(), DL(), NS(), NS(), NS(), DL(), NS(), DL(), NS(), NS(), NS(), DL(), NS(), NS(),
                TW(), NS(), NS(), DL(), NS(), NS(), NS(), ST(), NS(), NS(), NS(), DL(), NS(), NS(), TW(),
                NS(), NS(), DL(), NS(), NS(), NS(), DL(), NS(), DL(), NS(), NS(), NS(), DL(), NS(), NS(),
                NS(), TL(), NS(), NS(), NS(), TL(), NS(), NS(), NS(), TL(), NS(), NS(), NS(), TL(), NS(),
                NS(), NS(), NS(), NS(), DW(), NS(), NS(), NS(), NS(), NS(), DW(), NS(), NS(), NS(), NS(),
                DL(), NS(), NS(), DW(), NS(), NS(), NS(), DL(), NS(), NS(), NS(), DW(), NS(), NS(), DL(),
                NS(), NS(), DW(), NS(), NS(), NS(), DL(), NS(), DL(), NS(), NS(), NS(), DW(), NS(), NS(),
                NS(), DW(), NS(), NS(), NS(), TL(), NS(), NS(), NS(), TL(), NS(), NS(), NS(), DW(), NS(),
                TW(), NS(), NS(), DL(), NS(), NS(), NS(), TW(), NS(), NS(), NS(), DL(), NS(), NS(), TW()
        };
        }

        public List<BoardSquare> Squares { get; }

        public virtual BoardSquare GetSquare(int x, int y)
        {
            return Squares[y * 15 + x];
        }

        private int x = 0;
        private int y = 0;

        private BoardSquare NS()
        {
            Next();
            return BoardSquare.NormalSquare(x++, y);
        }

        private BoardSquare DL()
        {
            Next();
            return BoardSquare.DoubleLetterSquare(x++, y);
        }

        private BoardSquare TL()
        {
            Next();
            return BoardSquare.TripleLetterSquare(x++, y);
        }

        private BoardSquare DW()
        {
            Next();
            return BoardSquare.DoubleWordSquare(x++, y);
        }

        private BoardSquare TW()
        {
            Next();
            return BoardSquare.TripleWordSquare(x++, y);
        }

        private BoardSquare ST()
        {
            Next();
            return BoardSquare.StartingSquare(x++, y);
        }

        private void Next()
        {
            if (x >= 15)
            {
                x = 0;
                y++;
            }
        }
    }
}
