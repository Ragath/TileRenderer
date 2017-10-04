using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using TiledLib;

namespace TileRenderer.Pipeline
{
    [ContentImporter(".tmx", ".json", CacheImportedData = false, DisplayName = "Tiled Map Importer", DefaultProcessor = "PassThroughProcessor")]
    public class TiledMapImporter : ContentImporter<ContentItem<Map>> //TiledLib.Pipeline.TiledMapImporter
    {
        public override ContentItem<Map> Import(string filename, ContentImporterContext context)
        {
            var content = new ContentItem<Map>()
            {
                Identity = new ContentIdentity(filename),
                Name = Path.GetFileNameWithoutExtension(filename),
                Value = new TiledLib.Pipeline.TiledMapImporter().Import(filename, context)
            };

            return content;
        }
    }
}
