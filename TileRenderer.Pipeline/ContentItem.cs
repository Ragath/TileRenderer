using Microsoft.Xna.Framework.Content.Pipeline;

namespace TileRenderer.Pipeline
{
    public class ContentItem<T> : ContentItem
    {
        public T Value { get; set; }
    }
}
