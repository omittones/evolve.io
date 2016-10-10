using System;
using System.Drawing;
using System.Windows.Forms;
using core.Graphics;

namespace core.test.drawing
{
    public partial class Main : Form
    {
        private Image image;
        private GraphicsEngine engine;
        private float angle;
        private float delta;
        private Brain brain;

        public Main()
        {
            InitializeComponent();

            this.angle = 0.0f;

            this.brain = new Brain();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.image = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            this.engine = new GraphicsEngine(this, this.image);
            
            Application.Idle += ExecuteStep;
        }

        private void ExecuteStep(object sender, EventArgs e)
        {
            this.angle += 0.01f;
            while (this.angle > Math.PI*2)
                this.angle -= (float) (Math.PI*2);
            this.delta = (float) (Math.Sin(this.angle) + 1.0)/2.0f;

            this.engine.pushMatrix();
            this.engine.fill(Color.Black);
            this.engine.rect(0, 0, this.image.Width, this.image.Height);
            this.engine.scale(this.image.Width/2.0f);

            this.engine.pushMatrix();
            this.engine.translate(0, 0);
            this.engine.scale(this.delta);
            this.brain.draw(this.engine, 0, 0);
            this.engine.popMatrix();

            this.engine.pushMatrix();
            this.engine.translate(1f, 0);
            this.engine.translate(this.delta/3f, this.delta/3f);
            this.brain.draw(this.engine, 0, 0);
            this.engine.popMatrix();

            this.engine.pushMatrix();
            this.engine.translate(0, 1);
            this.engine.translate(0.5f, 0.5f);
            this.engine.rotate(this.angle*360.0f);
            this.engine.translate(-0.2f, -0.5f);

            this.brain.draw(this.engine, 0, 0);
            this.engine.popMatrix();

            this.engine.pushMatrix();
            this.engine.translate(1f, 1f);
            this.brain.draw(this.engine, 0, 0);
            this.engine.popMatrix();

            this.engine.popMatrix();
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
