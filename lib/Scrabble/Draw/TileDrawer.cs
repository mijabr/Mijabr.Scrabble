using Scrabble.Value;
using System;
using System.Linq;

namespace Scrabble.Draw
{
    public class TileDrawer : ITileDrawer
    {
        public void DrawTilesForPlayer(Drawable drawable)
        {
            for (var i = 0; i < 7; i++)
            {
                DrawOneTile(drawable);
            }
        }

        public void DrawTilesForAllPlayers(Drawable drawable)
        {
            var currentPlayer = drawable.PlayerTurn;

            for(var n = 0; n < drawable.Players.Count; n++)
            {
                drawable.PlayerTurn = n;
                DrawTilesForPlayer(drawable);
            }

            drawable.PlayerTurn = currentPlayer;
        }

        private static void DrawOneTile(Drawable drawable)
        {
            if (drawable.BagTiles == null || drawable.CurrentPlayer().Tiles == null) return;

            if (drawable.BagTiles.Count <= 0 || drawable.CurrentPlayer().Tiles.Count >= 7) return;

            var r = new Random();
            var tile = drawable.BagTiles.ElementAt(r.Next(drawable.BagTiles.Count()));
            MoveTileToTray(tile, drawable);
        }

        private static void MoveTileToTray(Tile tile, Drawable drawable)
        {
            drawable.BagTiles.Remove(tile);
            tile.Location = "tray";
            tile.TrayPosition = GetFirstFreeTrayPosition(drawable);
            drawable.CurrentPlayer().Tiles.Add(tile);
        }

        private static int GetFirstFreeTrayPosition(Drawable drawable)
        {
            for (var position = 0; position < 7; position++)
            {
                if (drawable.CurrentPlayer().Tiles.FirstOrDefault(t => t.TrayPosition == position).IsDefault())
                {
                    return position;
                }
            }

            return 0;
        }
    }
}
