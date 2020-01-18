using System;

namespace Scrabble.Value
{
    public struct Tile
    {
        public Tile(char letter)
        {
            Letter = letter;
            IsBlank = false;
            if (Letter == ' ')
            {
                Value = 0;
                IsBlank = true;
            }
            else if (Letter == 'D' || Letter == 'G')
            {
                Value = 2;
            }
            else if (Letter == 'B' || Letter == 'C' || Letter == 'M' || Letter == 'P')
            {
                Value = 3;
            }
            else if (Letter == 'F' || Letter == 'H' || Letter == 'V' || Letter == 'W' || Letter == 'Y')
            {
                Value = 4;
            }
            else if (Letter == 'K')
            {
                Value = 5;
            }
            else if (Letter == 'J' || Letter == 'X')
            {
                Value = 8;
            }
            else if (Letter == 'Q' || Letter == 'Z')
            {
                Value = 10;
            }
            else
            {
                Value = 1;
            }

            Location = "bag";
            TrayPosition = 0;
            BoardPositionX = 0;
            BoardPositionY = 0;
        }

        public char Letter { get; set; }
        public bool IsBlank { get; set; }
        public int Value { get; set; }
        public string Location { get; set; }
        public int TrayPosition { get; set; }
        public int BoardPositionX { get; set; }
        public int BoardPositionY { get; set; }
        internal bool IsDefault()
        {
            return Letter == 0;
        }
    }
}
