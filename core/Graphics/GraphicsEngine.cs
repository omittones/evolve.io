using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace core.Graphics
{
    public class GraphicsEngine
    {
        public readonly float screenWidth;
        public readonly float screenHeight;
        public bool keyPressed;
        public Keys keyCode;
        public char key;
        public int mouseX;
        public int mouseY;
        private readonly System.Drawing.Graphics engine;
        private readonly NormalDrawer normalDrawer;
        private readonly FilledDrawer fillDrawer;
        private IDrawer drawer;
        public readonly PFont font;
        private readonly Stack<Matrix> transMatrix;

        public GraphicsEngine(
            Form container,
            Image image)
        {
            this.engine = System.Drawing.Graphics.FromImage(image);
            this.normalDrawer = new NormalDrawer(this.engine);
            this.normalDrawer.Font = container.Font;
            this.fillDrawer = new FilledDrawer(this.engine);
            this.fillDrawer.Font = container.Font;
            this.transMatrix = new Stack<Matrix>();
            this.screenWidth = image.Width;
            this.screenHeight = image.Height;

            container.MouseMove += UpdateMouse;
            container.KeyDown += SetKeyCode;
            container.KeyPress += SetKeyChar;
            container.KeyUp += RemoveKeyCode;
        }

        private void SetKeyChar(object sender, KeyPressEventArgs e)
        {
            this.key = e.KeyChar;
        }

        private void SetKeyCode(object sender, KeyEventArgs e)
        {
            this.keyPressed = true;
            this.keyCode = e.KeyCode;
            this.key = char.MaxValue;
        }

        private void RemoveKeyCode(object sender, KeyEventArgs e)
        {
            this.keyPressed = false;
        }

        private void UpdateMouse(object sender, MouseEventArgs e)
        {
            this.mouseX = e.X;
            this.mouseY = e.Y;
        }

        public void noFill()
        {
            this.drawer = this.normalDrawer;
        }

        public void fill(hslColor color)
        {
            this.drawer = this.fillDrawer;
            this.fillDrawer.FillColor = color;
        }

        public void fill(float h, float s, float l, float alpha = 1)
        {
            var color = new hslColor(h, s, l, alpha);
            this.fill(color);
        }

        public void stroke(hslColor color)
        {
            this.normalDrawer.StrokePen = new Pen(color);
            this.fillDrawer.StrokePen = new Pen(color);
        }

        public void stroke(float h, float s, float l, float a = 1.0f)
        {
            var color = new hslColor(h, s, l, a);
            this.stroke(color);
        }

        public void strokeWeight(float weight)
        {
            this.normalDrawer.StrokePen.Width = weight;
            this.fillDrawer.StrokePen.Width = weight;
        }

        public void noStroke()
        {
            this.normalDrawer.StrokePen.Width = 1;
            this.fillDrawer.StrokePen.Width = 1;
        }

        public void colorMode(ColorMode mode, float gamma)
        {
        }

        public PFont loadFont(string font)
        {
            return null;
        }

        public void saveFrame(string filename)
        {
        }

        public void saveStrings(string filename,  string[] data)
        {
        }

        public void translate(float x, float y)
        {
            this.engine.TranslateTransform(x, y, MatrixOrder.Append);
        }

        public void scale(float scale)
        {
            this.engine.ScaleTransform(scale, scale, MatrixOrder.Append);
        }

        public void rotate(float rotate)
        {
            this.engine.RotateTransform(rotate, MatrixOrder.Append);
        }

        public void popMatrix()
        {
            if (this.transMatrix.Count > 0)
            {
                this.engine.Transform = this.transMatrix.Pop();
            }
        }

        public void pushMatrix()
        {
            this.transMatrix.Push(this.engine.Transform.Clone());
            this.engine.Transform.Reset();
        }
        
        public void ellipseMode(EllipseMode mode)
        {

        }

        public void ellipse(float a, float b, float c = 0, float d = 0)
        {
            this.drawer.Ellipse(a, b, c, d);
        }

        public void line(double x1, double y1, double x2, double y2)
        {
            this.drawer.DrawLine((float) x1, (float) y1, (float) x2, (float) y2);
        }

        public void rect(float x, float y, float width, float height)
        {
            this.drawer.Rectangle(x, y, width, height);
        }

        public void textAlign(AlignText where)
        {

        }

        public void text(string text, float x, float y)
        {
            this.drawer.Text(text, x, y);
        }

        public void text(string text, float x1, float y1, float x2, float y2)
        {
            this.drawer.Text(text, x1, y1, x2, y2);
        }

        public float textWidth(string width)
        {
            return this.engine.MeasureString(width,
                this.normalDrawer.Font).Width;
        }

        public void textFont(PFont font, float size)
        {
            this.normalDrawer.Font = new Font(this.normalDrawer.Font.FontFamily, size, GraphicsUnit.Pixel);
            this.fillDrawer.Font = this.normalDrawer.Font;
        }
    }
}