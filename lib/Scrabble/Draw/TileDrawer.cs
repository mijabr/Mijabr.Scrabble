using Scrabble.Value;
using System;
using System.Linq;

namespace Scrabble.Draw
{
    public class TileDrawer : ITileDrawer
    {
        public void DrawTilesForPlayer(Drawable drawable)
        {
            this.drawable = drawable;
            for (int i = 0; i < 7; i++)
            {
                DrawOneTile();
            }
        }

        public void DrawTilesForAllPlayers(Drawable drawable)
        {
            int currentPlayer = drawable.PlayerTurn;

            for(int n = 0; n < drawable.Players.Count; n++)
            {
                drawable.PlayerTurn = n;
                DrawTilesForPlayer(drawable);
            }

            drawable.PlayerTurn = currentPlayer;
        }

        Drawable drawable;

        void DrawOneTile()
        {
            if (drawable.BagTiles != null && drawable.CurrentPlayer().Tiles != null)
            {
                if (drawable.BagTiles.Count() > 0 && drawable.CurrentPlayer().Tiles.Count() < 7)
                {
                    Random r = new Random();
                    var tile = drawable.BagTiles.ElementAt(r.Next(drawable.BagTiles.Count()));
                    MoveTileToTray(tile);
                }
            }
        }

        void MoveTileToTray(Tile tile)
        {
            drawable.BagTiles.Remove(tile);
            tile.Location = "tray";
            tile.TrayPosition = GetFirstFreeTrayPosition();
            drawable.CurrentPlayer().Tiles.Add(tile);
        }

        int GetFirstFreeTrayPosition()
        {
            for (int position = 0; position < 7; position++)
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
