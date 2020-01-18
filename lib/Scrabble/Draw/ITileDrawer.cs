
namespace Scrabble.Draw
{
    public interface ITileDrawer
    {
        void DrawTilesForPlayer(Drawable drawable);
        void DrawTilesForAllPlayers(Drawable drawable);
    }
}
