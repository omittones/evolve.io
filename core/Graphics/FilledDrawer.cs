using System.Drawing;

namespace core.Graphics
{
    public class FilledDrawer : NormalDrawer, IDrawer
    {
        private readonly SolidBrush fillBrush;

        public Color FillColor
        {
            get { return fillBrush.Color; }
            set { fillBrush.Color = value; }
        }

        private readonly System.Drawing.Graphics engine;

        public FilledDrawer(System.Drawing.Graphics engine) : base(engine)
        {
            this.engine = engine;
            this.fillBrush = new SolidBrush(Color.White);
        }

        public override void Rectangle(float x, float y, float width, float height)
        {
            this.engine.FillRectangle(this.fillBrush, x, y, width, height);

            base.Rectangle(x, y, width, height);
        }

        public override void Ellipse(float x, float y, float width, float height)
        {
            this.engine.FillEllipse(this.fillBrush, x, y, width, height);

            base.Ellipse(x, y, width, height);
        }
    }
}