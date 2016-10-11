using System;
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
        private readonly System.Drawing.Graphics engine;
        private readonly NormalDrawer normalDrawer;
        private readonly FilledDrawer fillDrawer;
        private IDrawer drawer;
        private readonly Stack<Matrix> transMatrix;

        public GraphicsEngine(Form container, Image image)
        {
            this.engine = System.Drawing.Graphics.FromImage(image);
            this.normalDrawer = new NormalDrawer(this.engine);
            this.normalDrawer.Font = container.Font;
            this.fillDrawer = new FilledDrawer(this.engine);
            this.fillDrawer.Font = container.Font;
            this.transMatrix = new Stack<Matrix>();
            this.screenWidth = image.Width;
            this.screenHeight = image.Height;
        }

        public void noFill()
        {
            this.drawer = this.normalDrawer;
        }

        public void fill(HSBColor color)
        {
            this.drawer = this.fillDrawer;
            this.fillDrawer.FillColor = color;
            this.fillDrawer.TextColor = color;
            this.normalDrawer.TextColor = color;
        }

        public void fill(float h, float s, float l, float alpha = 1)
        {
            var color = new HSBColor(h, s, l, alpha);
            this.fill(color);
        }

        public void stroke(HSBColor color)
        {
            this.normalDrawer.StrokeColor = color;
            this.normalDrawer.TextColor = color;
            this.fillDrawer.StrokeColor = color;
        }

        public void stroke(float h, float s, float l, float a = 1.0f)
        {
            var color = new HSBColor(h, s, l, a);
            this.stroke(color);
        }

        public void strokeWeight(float weight)
        {
            this.normalDrawer.StrokeWidth = weight;
            this.fillDrawer.StrokeWidth = weight;
        }

        public void noStroke()
        {
            this.normalDrawer.StrokeWidth = 0;
            this.normalDrawer.StrokeColor = Color.Transparent;
            this.fillDrawer.StrokeWidth = 0;
            this.fillDrawer.StrokeColor = Color.Transparent;
        }

        public void saveFrame(string filename)
        {
        }

        public void saveStrings(string filename,  string[] data)
        {
        }

        public void translate(float x, float y)
        {
            this.engine.TranslateTransform(x, y, MatrixOrder.Prepend);
        }

        public void scale(float scale)
        {
            this.engine.ScaleTransform(scale, scale, MatrixOrder.Prepend);
        }

        public void rotate(float rotate)
        {
            this.engine.RotateTransform(rotate, MatrixOrder.Prepend);
        }

        public void popMatrix()
        {
            if (this.transMatrix.Count > 0)
                this.engine.Transform = this.transMatrix.Pop();
        }

        public void pushMatrix()
        {
            this.transMatrix.Push(this.engine.Transform.Clone());
        }

        public MatrixScope newTransformScope()
        {
            return new MatrixScope(this.engine);
        }

        public void ellipse(float centerX, float centerY, float radiusX = 0, float radiusY = 0)
        {
            this.drawer.Ellipse(centerX - radiusX, centerY - radiusY, radiusX*2, radiusY*2);
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
            this.drawer.Align = where;
        }

        public void text(string text, float x, float y)
        {
            this.drawer.Text(text, x, y);
        }

        public void text(string text, float x1, float y1, float x2, float y2)
        {
            this.drawer.Text(text, x1, y1, x2, y2);
        }

        public float textWidth(string text)
        {
            return this.engine.MeasureString(text, this.normalDrawer.Font).Width;
        }

        public void textSize(float size)
        {
            this.normalDrawer.Font = new Font(this.normalDrawer.Font.FontFamily, size/1.5f, GraphicsUnit.Pixel);
            this.fillDrawer.Font = this.normalDrawer.Font;
        }

        public PointF transformFromWorld(float x, float y)
        {
            var points = new[] {new PointF(x, y)};
            this.engine.TransformPoints(CoordinateSpace.Device, CoordinateSpace.World, points);
            return points[0];
        }

        public PointF transformToWorld(float x, float y)
        {
            var points = new[] { new PointF(x, y) };
            this.engine.TransformPoints(CoordinateSpace.World, CoordinateSpace.Device, points);
            return points[0];
        }
    }
}