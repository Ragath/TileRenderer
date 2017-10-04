namespace TileRenderer
{
    public interface IRenderer
    {
        string Name { get; }
        void Draw();
    }
}