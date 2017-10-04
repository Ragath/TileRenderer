using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileRenderer
{
    public class RendererSB : IRenderer
    {
        readonly SpriteBatch Batch;
        readonly TileMap Map;

        public string Name => "Spritebatch Renderer";

        public RendererSB(TileMap map, SpriteBatch batch)
        {
            Map = map;
            Batch = batch;
        }

        public void Draw()
        {
            var gfx = Batch.GraphicsDevice;
            var w = (int)(gfx.Viewport.Width / Game1.Scale - 1) / Map.TileWidth + 1;
            var h = (int)(gfx.Viewport.Height / Game1.Scale - 1) / Map.TileHeight + 1;

            Batch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(Game1.Scale));
            {
                for (int y = 0; y < h && y < Map.Tiles.GetLength(1); y++)
                    for (int x = 0; x < w && x < Map.Tiles.GetLength(0); x++)
                        Batch.Draw(Map.Texture, new Vector2(x * Map.TileWidth, y * Map.TileHeight), Map.Tiles[x, y], Color.White);
            }
            Batch.End();
        }
    }
}
