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

        int x = 0;
        int y = 0;

        BoardSquare NS()
        {
            Next();
            return BoardSquare.NormalSquare(x++, y);
        }

        BoardSquare DL()
        {
            Next();
            return BoardSquare.DoubleLetterSquare(x++, y);
        }

        BoardSquare TL()
        {
            Next();
            return BoardSquare.TrippleLetterSquare(x++, y);
        }

        BoardSquare DW()
        {
            Next();
            return BoardSquare.DoubleWordSquare(x++, y);
        }

        BoardSquare TW()
        {
            Next();
            return BoardSquare.TrippleWordSquare(x++, y);
        }

        BoardSquare ST()
        {
            Next();
            return BoardSquare.StartingSquare(x++, y);
        }

        void Next()
        {
            if (x >= 15)
            {
                x = 0;
                y++;
            }
        }
    }
}
