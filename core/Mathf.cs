using System;

namespace core
{
    public static class MathF
    {
        public static int FloorToInt(float x)
        {
            return (int) Math.Floor(x);
        }

        public static float Floor(float x)
        {
            return (float) Math.Floor(x);
        }

        public static float Distance(float x1, float y1, float x2, float y2)
        {
            var x = x2 - x1;
            var y = y2 - y1;
            return (float)Math.Sqrt(x * x + y * y);
        }
    }
}