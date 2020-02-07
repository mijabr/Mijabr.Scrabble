using Scrabble.Value;
using System.Linq;

namespace Scrabble.Ai
{
    public class AiGoPlacer : IAiGoPlacer
    {
        public void PlaceGo(AiValidGo go, Game game)
        {
            this.game = game;
            ReturnAllTilesToTray();

            if (go?.Candidate.SearchPattern == null)
            {
                return;
            }

            this.go = go;
            currentX = go.Candidate.StartX;
            currentY = go.Candidate.StartY;
            currentWordPosition = 0;

            PlaceTiles();
        }

        private void ReturnAllTilesToTray()
        {
            var done = false;
            while (!done)
            {
                var tile = game.CurrentPlayer().Tiles.FirstOrDefault(t => t.Location != "tray");
                if (game.CurrentPlayer().Tiles.Remove(tile))
                {
                    tile.Location = "tray";
                    game.CurrentPlayer().Tiles.Add(tile);
                }
                else
                {
                    done = true;
                }
            }
        }

        private AiValidGo go;
        private Game game;
        private int currentX;
        private int currentY;
        private int currentWordPosition;

        private void PlaceTiles()
        {
            foreach (var searchCharacter in go.Candidate.SearchPattern)
            {
                if (searchCharacter == '?')
                {
                    PlaceTile();
                }

                if (go.Candidate.Orientation == 0)
                {
                    currentX++;
                }
                else
                {
                    currentY++;
                }

                currentWordPosition++;
            }
        }

        private void PlaceTile()
        {
            var letter = go.MainWord[currentWordPosition];
            var tile = game.CurrentPlayer().Tiles.FirstOrDefault(t => t.Location == "tray" && t.Letter == letter);
            if (!game.CurrentPlayer().Tiles.Remove(tile)) return;

            tile.Location = "board";
            tile.BoardPositionX = currentX;
            tile.BoardPositionY = currentY;
            game.CurrentPlayer().Tiles.Add(tile);
        }
    }
}
