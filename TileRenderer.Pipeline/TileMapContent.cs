using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;

namespace TileRenderer.Pipeline
{
    [ContentSerializerRuntimeType("TileRenderer.TileMap, TileRenderer")]
    public class TileMapContent
    {
        public ExternalReference<Texture2D> Texture { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public Rectangle[,] Tiles { get; set; }
    }
}