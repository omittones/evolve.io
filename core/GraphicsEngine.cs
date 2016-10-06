using System;
using System.Drawing;
using System.Windows.Forms;

namespace core
{
    public interface IDrawer
    {
        void Rectangle(float x, float y, float width, float height);
    }

    public class FilledDrawer : IDrawer
    {
        private readonly Graphics engine;
        private readonly hslColor fillColor;

        public FilledDrawer(Graphics engine, hslColor fillColor)
        {
            this.engine = engine;
            this.fillColor = fillColor;
        }

        public void Rectangle(float x, float y, float width, float height)
        {
            this.engine.FillRectangle(new SolidBrush(this.fillColor), x, y, width, height);
        }
    }

    public class NormalDrawer : IDrawer
    {
        private Graphics engine;

        public NormalDrawer(Graphics engine)
        {
            this.engine = engine;
        }

        public void Rectangle(float x, float y, float width, float height)
        {
            this.engine.FillRectangle(new SolidBrush(Color.Black), x, y, width, height);
        }
    }

    public class GraphicsEngine
    {
        public readonly float screenWidth;
        public readonly float screenHeight;
        public bool keyPressed;
        public Keys keyCode;
        public char key;
        public int mouseX;
        public int mouseY;
        private readonly NormalDrawer normalDrawer;
        private readonly Graphics engine;
        private IDrawer drawer;
        public readonly PFont font;

        public GraphicsEngine(
            Form container,
            Image image)
        {
            this.engine = Graphics.FromImage(image);
            this.normalDrawer = new NormalDrawer(this.engine);

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
            this.drawer = new FilledDrawer(this.engine, color);
        }

        public void fill(float h, float s, float l, float alpha = 1)
        {
            this.drawer = new FilledDrawer(this.engine, new hslColor(h, s, l, alpha));
        }

        public void size(int width, int height)
        {
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
        }

        public void scale(float scale)
        {
        }

        public void rotate(float rotate)
        {
        }

        public void popMatrix()
        {

        }

        public void pushMatrix()
        {

        }

        public void noStroke()
        {
        }

        public void ellipseMode(int mode)
        {

        }

        public void ellipse(float a, float b, float c = 0, float d = 0)
        {
        }

        public void stroke(int color)
        {
        }

        public void stroke(hslColor color)
        {
        }

        public void line(double a, double b, double c, double d)
        {
        }

        public void stroke(float a, float b, float c, float d = 1.0f)
        {
        }

        public void strokeWeight(float weight)
        {
        }

        public void rect(float x, float y, float width, float height)
        {
            this.drawer.Rectangle(x, y, width, height);
        }

        public float hue(hslColor clr)
        {
            return 0;
        }

        public float brightness(hslColor clr)
        {
            return 0;
        }

        public float saturation(hslColor clr)
        {
            return 0;
        }

        public void textAlign(AlignText where)
        {

        }

        public void text(string text, float x, float y)
        {
        }

        public void text(string text, float x1, float y1, float x2, float y2)
        {
        }

        public float textWidth(string width)
        {
            return 0;
        }

        public void textFont(PFont font, float size)
        {
        }
    }
}