using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileRenderer
{
    public class TileMap
    {
        public Texture2D Texture { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public Rectangle[,] Tiles { get; private set; }
    }
}