using System;

namespace core
{
    public class GraphicsEngine
    {
        public bool keyPressed;
        public int keyCode;
        public char key;
        public int mouseX;
        public int mouseY;
        public float screenWidth;
        public float screenHeight;

        public void noFill()
        {
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

        public void stroke(color color)
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

        public void fill(color color)
        {

        }

        public void fill(float a, float b, float c)
        {

        }

        public void fill(float a, float b, float c, float d)
        {

        }

        public void rect(float a, float b, float c, float d)
        {

        }

        public float hue(color clr)
        {
            return 0;
        }

        public float brightness(color clr)
        {
            return 0;
        }

        public float saturation(color clr)
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

        public PFont font;

        public void textFont(PFont font, float size)
        {

        }
    }
}