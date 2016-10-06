namespace core.Graphics
{
    public interface IDrawer
    {
        void Rectangle(float x, float y, float width, float height);

        void DrawLine(float x1, float y1, float x2, float y2);

        void Text(string text, float x, float y);

        void Text(string text, float x1, float y1, float x2, float y2);
    }
}