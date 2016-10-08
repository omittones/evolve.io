using core.Graphics;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace core
{
    public class Sprite
    {
        public void draw(GraphicsEngine graphics,
            float transX = 0,
            float transY = 0,
            float rotate = 0,
            float scale = 1)
        {
            graphics.pushMatrix();
            if (scale != 1.0f)
                graphics.scale(scale);
            if (rotate != 0)
                graphics.rotate(rotate);
            if (transX != 0 || transY != 0)
                graphics.translate(transX, transY);

            this.drawCore(graphics);

            graphics.popMatrix();
        }

        protected virtual void drawCore(GraphicsEngine graphics)
        {
        }
    }
}