using System;

namespace core
{
    public class Helpers
    {
        protected bool keyPressed;
        protected int keyCode;
        protected char key;
        protected int mouseX;
        protected int mouseY;
        protected float screenWidth;
        protected float screenHeight;

        protected void noFill()
        {
        }

        protected void size(int width, int height)
        {
        }

        protected void colorMode(ColorMode mode, float gamma)
        {
        }

        protected PFont loadFont(string font)
        {
            return null;
        }

        protected void saveFrame(string filename)
        {
        }

        protected void saveStrings(string filename,  string[] data)
        {
        }

        protected void translate(float x, float y)
        {
        }

        protected void scale(float scale)
        {
        }

        protected void rotate(float rotate)
        {
        }

        protected void popMatrix()
        {

        }

        protected void pushMatrix()
        {

        }
        
        protected void noStroke()
        {
        }

        protected void ellipseMode(int mode)
        {

        }

        protected void ellipse(float a, float b, float c = 0, float d = 0)
        {
        }
        
        protected static color color(double a, double b, double c, double alpha = 1)
        {
            return new color();
        }

        protected void stroke(int color)
        {
        }

        protected void stroke(color color)
        {
        }

        protected void line(double a, double b, double c, double d)
        {
        }

        protected void stroke(float a, float b, float c, float d = 1.0f)
        {
        }

        protected void strokeWeight(float weight)
        {
        }

        protected void fill(color color)
        {

        }

        protected void fill(float a, float b, float c)
        {

        }

        protected void fill(float a, float b, float c, float d)
        {

        }

        protected void rect(float a, float b, float c, float d)
        {

        }

        protected float hue(color clr)
        {
            return 0;
        }

        protected float brightness(color clr)
        {
            return 0;
        }

        protected float saturation(color clr)
        {
            return 0;
        }

        protected void textAlign(AlignText where)
        {

        }

        protected void text(string text, float x, float y)
        {
        }

        protected void text(string text, float x1, float y1, float x2, float y2)
        {
        }

        protected float textWidth(string width)
        {
            return 0;
        }

        protected PFont font;

        protected void textFont(PFont font, float size)
        {

        }
    }
}