using System;
using System.Drawing;
using System.Windows.Forms;
using core.Graphics;

namespace core
{
    public partial class Main : Form
    {
        private EvolvioColor app;
        private Image image;

        public Main()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.image = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            this.app = new EvolvioColor();
            this.app.setup(new GraphicsEngine(this, this.image), this.image.Width, this.image.Height);

            Application.Idle += ExecuteStep;
        }

        private void ExecuteStep(object sender, EventArgs e)
        {
            this.app.draw();

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.image != null)
            {
                e.Graphics.DrawImageUnscaled(this.image, 0, 0);
            }
            else
            {
                e.Graphics.Clear(Color.Black);
            }
        }
    }
}
