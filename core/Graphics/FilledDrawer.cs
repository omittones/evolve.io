using System.Drawing;

namespace core.Graphics
{
    public class FilledDrawer : IDrawer
    {
        public Font Font { get; set; }
        public Color FillColor { get; set; }
        public Pen StrokePen { get; set; }

        private readonly System.Drawing.Graphics engine;

        public FilledDrawer(System.Drawing.Graphics engine)
        {
            this.engine = engine;
        }

        public void Rectangle(float x, float y, float width, float height)
        {
            this.engine.FillRectangle(new SolidBrush(this.FillColor), x, y, width, height);
        }

        public void DrawLine(float x1, float y1, float x2, float y2)
        {
            this.engine.DrawLine(this.StrokePen, x1, y1, x2, y2);
        }

        public void Text(string text, float x, float y)
        {
            this.engine.DrawString(text, this.Font, new SolidBrush(FillColor), x, y);
        }

        public void Text(string text, float x1, float y1, float x2, float y2)
        {
            this.engine.DrawString(text, this.Font, new SolidBrush(FillColor), RectangleF.FromLTRB(x1, y1, x2, y2));
        }
    }
}