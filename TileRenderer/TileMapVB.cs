using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileRenderer
{
    public sealed class TileMapVB
    {
        public readonly VertexBuffer vb;
        const int PSIZE = 6;
        [ThreadStatic]
        static readonly VertexPositionTexture[] Quad = new VertexPositionTexture[PSIZE];

        TileMap Map { get; }

        static IEnumerable<(int x, int y)> PartitionPositions(int w, int h, int ph)
        {
            if (w <= 0)
                throw new ArgumentOutOfRangeException(nameof(w));
            if (h <= 0)
                throw new ArgumentOutOfRangeException(nameof(h));

            var bottom = h + ph - 1;
            for (int row = 0; row < bottom; row += ph)
                for (int x = 0; x < w; x++)
                    for (int y = row; y < row + ph; y++)
                        yield return (x, y);
        }

        readonly int RowHeight;
        readonly int RowSize;

        public TileMapVB(GraphicsDevice gfx, TileMap map, int partitionHeight = -1)
        {
            Map = map;
            var tiles = map.Tiles;

            if (partitionHeight > 0)
                RowHeight = partitionHeight;
            else
                RowHeight = (gfx.Viewport.Height - 1) / (4 * map.TileHeight) + 1;

            RowSize = tiles.GetLength(0) * RowHeight * PSIZE;

            var rows = (tiles.GetLength(1) - 1) / RowHeight + 1;

            vb = new VertexBuffer(gfx, VertexPositionTexture.VertexDeclaration, rows * RowSize, BufferUsage.WriteOnly);

            var stride = vb.VertexDeclaration.VertexStride;
            int i = 0;
            foreach ((var x, var y) in PartitionPositions(tiles.GetLength(0), tiles.GetLength(1), RowHeight))
            {
                var tile = y < tiles.GetLength(1) ? tiles[x, y] : Rectangle.Empty;
                if (tile.IsEmpty)
                    Quad.Initialize();
                else
                    FillQuad(x, y, tile, map.Texture.Bounds.Size.ToVector2());

                vb.SetData(i * stride, Quad, 0, PSIZE, stride);

                i += PSIZE;
            }
        }

        static void FillQuad(int x, int y, Rectangle tile, Vector2 textureSize)
        {
            Quad[0] = new VertexPositionTexture
            {
                Position = new Vector3((x + 1) * tile.Width, -y * tile.Height, 0f),
                TextureCoordinate = new Vector2(tile.Right, tile.Y) / textureSize
            };
            Quad[1] = Quad[3] = new VertexPositionTexture
            {
                Position = new Vector3((x + 1) * tile.Width, -(y + 1) * tile.Height, 0f),
                TextureCoordinate = new Vector2(tile.Right, tile.Bottom) / textureSize
            };
            Quad[2] = Quad[5] = new VertexPositionTexture
            {
                Position = new Vector3(x * tile.Width, -y * tile.Height, 0f),
                TextureCoordinate = new Vector2(tile.X, tile.Y) / textureSize
            };

            Quad[4] = new VertexPositionTexture
            {
                Position = new Vector3(x * tile.Width, -(y + 1) * tile.Height, 0f),
                TextureCoordinate = new Vector2(tile.X, tile.Bottom) / textureSize
            };
        }

        public void Draw(Rectangle src)
        {
            src = new Rectangle
            {
                X = src.X / Map.TileWidth,
                Y = src.Y / Map.TileHeight,
                Width = (src.Width - 1) / Map.TileWidth + 1,
                Height = (src.Height - 1) / Map.TileHeight + 1
            };


            var offset = src.X * PSIZE;
            var bottomRow = (src.Bottom - 1) / RowHeight + 1;
            for (int row = src.Y / RowHeight; row < bottomRow; row++)
            {
                var start = row * RowSize + offset;
                var count = Math.Min(src.Width * RowHeight * PSIZE, RowSize - offset);
                vb.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, start, count / 3);
            }
        }
    }
}
