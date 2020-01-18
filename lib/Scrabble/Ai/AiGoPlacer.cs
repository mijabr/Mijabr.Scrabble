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

            if (go == null || go.Candidate.SearchPattern == null)
            {
                return;
            }

            this.go = go;
            currentX = go.Candidate.StartX;
            currentY = go.Candidate.StartY;
            currentWordPosition = 0;

            PlaceTiles();
        }

        void ReturnAllTilesToTray()
        {
            bool done = false;
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

        AiValidGo go;
        Game game;
        int currentX;
        int currentY;
        int currentWordPosition;

        void PlaceTiles()
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

        void PlaceTile()
        {
            var letter = go.MainWord[currentWordPosition];
            var tile = game.CurrentPlayer().Tiles.FirstOrDefault(t => t.Location == "tray" && t.Letter == letter);
            if (game.CurrentPlayer().Tiles.Remove(tile))
            {
                tile.Location = "board";
                tile.BoardPositionX = currentX;
                tile.BoardPositionY = currentY;
                game.CurrentPlayer().Tiles.Add(tile);
            }
        }
    }
}
