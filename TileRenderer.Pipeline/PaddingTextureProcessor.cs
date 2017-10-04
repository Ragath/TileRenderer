using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace TileRenderer.Pipeline
{
    [ContentProcessor(DisplayName = "Padding Texture Processor")]
    public class PaddingTextureProcessor : TextureProcessor
    {
        public int GridWidth { get; set; }
        public int GridHeight { get; set; }

        [DefaultValue(1)]
        public int Padding { get; set; } = 1;

        public override TextureContent Process(TextureContent input, ContentProcessorContext context)
        {
            var face = input.Faces.First();
            var src = face[0];
            var columns = src.Width / GridWidth;
            var rows = src.Height / GridHeight;

            var dst = new PixelBitmapContent<Color>(src.Width + columns * 2, src.Height + rows * 2);
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < columns; c++)
                {
                    var srcRect = new Rectangle
                    {
                        X = c * GridWidth,
                        Y = r * GridHeight,
                        Width = GridWidth,
                        Height = GridHeight
                    };
                    var dstRect = new Rectangle
                    {
                        X = c * (GridWidth + Padding * 2),
                        Y = r * (GridHeight + Padding * 2),
                        Width = (GridWidth + Padding * 2),
                        Height = (GridHeight + Padding * 2)
                    };

                    BitmapContent.Copy(src, srcRect, dst, dstRect);
                    dstRect.Inflate(-Padding, -Padding);
                    BitmapContent.Copy(src, srcRect, dst, dstRect);
                }

            face[0] = dst;
            return base.Process(input, context);
        }
    }
}
