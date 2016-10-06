using System;
using System.Drawing;
using System.Windows.Forms;

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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.image = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            this.app = new EvolvioColor();
            this.app.setup(new GraphicsEngine(this, this.image));

            Application.Idle += ExecuteStep;
        }

        private void ExecuteStep(object sender, EventArgs e)
        {
            this.app.draw();
            this.Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(this.image, 0, 0);
        }
    }
}
