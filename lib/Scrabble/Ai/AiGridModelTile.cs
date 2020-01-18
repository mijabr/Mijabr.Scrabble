namespace Scrabble.Ai
{
    public struct AiGridModelTile
    {
        public char Letter { get; set; }
        public bool IsNextToTile { get; set; }
        public int TileValue { get; set; }
    }
}
