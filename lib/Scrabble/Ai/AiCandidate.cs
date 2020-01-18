namespace Scrabble.Ai
{
    public struct AiCandidate
    {
        public int Orientation { get; set; }
        public int TilesUsed { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public string SearchPattern { get; set; }

        public bool Next(int maxTiles)
        {
            if (maxTiles == 0)
            {
                return false;
            }

            if (Orientation == 0)
            {
                TilesUsed--;
                if (TilesUsed > 0)
                {
                    return true;
                }

                TilesUsed = maxTiles;
                StartX++;
                if (StartX < 15)
                {
                    return true;
                }

                TilesUsed = maxTiles;
                StartX = 0;
                StartY++;
                if (StartY < 15)
                {
                    return true;
                }

                Orientation++;
                TilesUsed = maxTiles + 1;
                StartX = 0;
                StartY = 0;
            }

            if (Orientation == 1 && maxTiles > 0)
            {
                TilesUsed--;
                if (TilesUsed > 0)
                {
                    return true;
                }

                TilesUsed = maxTiles;
                StartY++;
                if (StartY < 15)
                {
                    return true;
                }

                TilesUsed = maxTiles;
                StartY = 0;
                StartX++;
                if (StartX < 15)
                {
                    return true;
                }

                Orientation++;
            }

            return false;
        }
    }
}
