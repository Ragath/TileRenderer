using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Graphics;
using TiledLib;
using TiledLib.Layer;

namespace TileRenderer.Pipeline
{
    [ContentProcessor(DisplayName = nameof(TileMapContent) + " Processor")]
    public class TilemapProcessor : ContentProcessor<ContentItem<Map>, TileMapContent>
    {
        [DefaultValue(1)]
        public int Padding { get; set; } = 1;

        public override TileMapContent Process(ContentItem<Map> input, ContentProcessorContext context)
        {

            var map = input.Value;
            var ts = map.Tilesets.Single();
            var tiles = new Rectangle[map.Width, map.Height];

            foreach (var layer in map.Layers.OfType<TileLayer>())
                for (int y = 0; y < tiles.GetLength(1); y++)
                    for (int x = 0; x < tiles.GetLength(0); x++)
                    {
                        var gid = layer.data[x + y * tiles.GetLength(0)];
                        if (gid != 0)
                        {
                            var c = (gid - ts.firstgid) % ts.Columns;
                            var r = (gid - ts.firstgid) / ts.Columns;
                            var tile = ts[gid];
                            tiles[x, y] = new Rectangle
                            {
                                X = tile.Left + c * Padding * 2 + Padding,
                                Y = tile.Top + r * Padding * 2 + Padding,
                                Width = tile.Width,
                                Height = tile.Height
                            };
                        }

                    }

            var processorParameters = new OpaqueDataDictionary()
            {
                [nameof(PaddingTextureProcessor.GridWidth)] = ts.tilewidth,
                [nameof(PaddingTextureProcessor.GridHeight)] = ts.tileheight,
                [nameof(PaddingTextureProcessor.Padding)] = Padding,
            };
            return new TileMapContent
            {
                Texture = context.BuildAsset<Texture2D, Texture2D>(new ExternalReference<Texture2D>(ts.ImagePath, input.Identity), nameof(PaddingTextureProcessor), processorParameters, null, null),
                TileWidth = map.CellWidth,
                TileHeight = map.CellHeight,
                Tiles = tiles
            };
        }
    }

    static class Extensions
    {
        public static Rectangle ToRectangle(this Tile t) => new Rectangle(t.Left, t.Top, t.Width, t.Height);
    }
}
