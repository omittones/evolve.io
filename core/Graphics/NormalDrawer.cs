using System.Drawing;

namespace core.Graphics
{
    public class NormalDrawer : IDrawer
    {
        private readonly System.Drawing.Graphics engine;

        public NormalDrawer(System.Drawing.Graphics engine)
        {
            this.engine = engine;
        }

        public Pen StrokePen { get; set; }

        public Font Font { get; set; }

        public void Rectangle(float x, float y, float width, float height)
        {
            this.engine.DrawRectangle(this.StrokePen, x, y, width, height);
        }

        public void DrawLine(float x1, float y1, float x2, float y2)
        {
            this.engine.DrawLine(this.StrokePen, x1, y1, x2, y2);
        }

        public void Text(string text, float x, float y)
        {
            this.engine.DrawString(text, this.Font, this.StrokePen.Brush, x, y);
        }

        public void Text(string text, float x1, float y1, float x2, float y2)
        {
            this.engine.DrawString(text, this.Font, this.StrokePen.Brush, new RectangleF(x1, y1, x2, y2));
        }

        public void Ellipse(float x, float y, float width, float height)
        {
            this.engine.DrawEllipse(this.StrokePen, x, y, width, height);
        }
    }
}