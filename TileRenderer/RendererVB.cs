using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileRenderer
{
    public class RendererVB : IRenderer
    {
        readonly TileMapVB vb;
        readonly BasicEffect basicEffect;

        public string Name => "Vertexbuffer Renderer";

        public RendererVB(TileMap map, GraphicsDevice gfx)
        {
            vb = new TileMapVB(gfx, map, (int)(gfx.Viewport.Height / Game1.Scale - 1) / map.TileHeight + 1);
            basicEffect = new BasicEffect(gfx);
            basicEffect.LightingEnabled = false;
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = map.Texture;
        }

        public void Draw()
        {
            var gfx = basicEffect.GraphicsDevice;
            var viewport = gfx.Viewport.Bounds;
            gfx.SamplerStates[0] = SamplerState.PointClamp;
            gfx.SetVertexBuffer(vb.vb);

            basicEffect.Projection = Matrix.CreateScale(Game1.Scale) * Matrix.CreateOrthographic(viewport.Width, viewport.Height, 0f, 1f) * Matrix.CreateTranslation(-1f, 1f, 0f);

            basicEffect.Techniques["BasicEffect_Texture_NoFog"].Passes[0].Apply();

            vb.Draw(new Rectangle(0, 0, (int)(viewport.Width / Game1.Scale), (int)(viewport.Height / Game1.Scale)));
        }
    }
}
