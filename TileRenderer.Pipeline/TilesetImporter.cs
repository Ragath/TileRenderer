using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System;
using System.IO;
using TiledLib;

namespace TileRenderer.Pipeline
{
    [ContentImporter(".tsx", ".json", DefaultProcessor = nameof(TextureProcessor))]
    public class TilesetImporter : ContentImporter<TextureContent>
    {
        public override TextureContent Import(string filename, ContentImporterContext context)
        {
            //using (var stream = File.OpenRead(filename))
            //{
            //    var ts = Tileset.FromStream(stream);
            //    var src = ts.ImagePath;
            //    if (!Path.IsPathRooted(src))
            //        src = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filename), src));
            //    context.AddDependency(src);
            //    return new TextureImporter().Import(src, context);
            //}
            throw new NotImplementedException();
        }
    }
}
