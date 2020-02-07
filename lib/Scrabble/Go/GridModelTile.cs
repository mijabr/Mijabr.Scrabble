namespace Scrabble.Go
{
    public struct GridModelTile
    {
        public GridModelTileOrigin Origin { get; set; }
        public char Letter { get; set; }
        public int TileValue { get; set; }

        public bool IsEmpty()
        {
            return Letter == 0;
        }
    }
}
