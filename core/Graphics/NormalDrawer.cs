using System.Drawing;

namespace core.Graphics
{
    public class NormalDrawer : IDrawer
    {
        private readonly System.Drawing.Graphics engine;

        public AlignText Align { get; set; }
        public Font Font { get; set; }

        private readonly SolidBrush textBrush;
        private readonly Pen strokePen;

        public Color TextColor
        {
            get { return textBrush.Color; }
            set { textBrush.Color = value; }
        }

        public Color StrokeColor
        {
            get { return strokePen.Color; }
            set { strokePen.Color = value; }
        }

        public float StrokeWidth
        {
            get { return strokePen.Width; }
            set { strokePen.Width = value; }
        }

        public NormalDrawer(System.Drawing.Graphics engine)
        {
            this.engine = engine;
            this.textBrush = new SolidBrush(Color.White);
            this.strokePen = new Pen(Color.White);
        }

        public void Text(string text, float x, float y)
        {
            var size = this.engine.MeasureString(text, this.Font);
            if (this.Align == AlignText.CENTER)
            {
                x = x - size.Width/2;
                y = y - size.Height;
            }
            else if (this.Align == AlignText.LEFT)
            {
                y = y - size.Height/1.2f;
            }
            else if (this.Align == AlignText.RIGHT)
            {
                x = x - size.Width;
                y = y - size.Height/1.2f;
            }

            this.engine.DrawString(text, this.Font, this.textBrush, x, y);
        }

        public void Text(string text, float x1, float y1, float x2, float y2)
        {
            var size = this.engine.MeasureString(text, this.Font);
            if (this.Align == AlignText.RIGHT)
                x1 = x2 - size.Width;
            else if (this.Align == AlignText.CENTER)
            {
                x1 = ((x1 + x2) - size.Width)/2;
                x2 = ((x1 + x2) + size.Width)/2;
            }
            else if (this.Align == AlignText.LEFT)
                x2 = x1 + size.Width;

            y1 = ((y1 + y2) - size.Height)/2;
            y2 = ((y1 + y2) + size.Height)/2;

            var location = new RectangleF(x1, y1, x2, y2);
            this.engine.DrawString(text, this.Font, this.textBrush, location);
        }

        public void DrawLine(float x1, float y1, float x2, float y2)
        {
            this.engine.DrawLine(this.strokePen, x1, y1, x2, y2);
        }

        public virtual void Rectangle(float x, float y, float width, float height)
        {
            this.engine.DrawRectangle(this.strokePen, x, y, width, height);
        }

        public virtual void Ellipse(float x, float y, float width, float height)
        {
            this.engine.DrawEllipse(this.strokePen, x, y, width, height);
        }
    }
}