using System;

namespace core
{
    public static class MathEx
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
            return (float) Math.Sqrt(x*x + y*y);
        }

        public static byte From01To255(float value)
        {
            if (value < 0)
                return 0;
            else if (value > 1)
                return 255;
            return (byte) (value*255);
        }

        public static float Lerp(float min, float max, float amount)
        {
            if (amount > 1)
                return max;
            if (amount < 0)
                return min;
            return min + (max - min)*amount;
        }

        public static float LerpAngle(float a, float b, float amount)
        {
            while (a > b + 180)
                b += 360;
            while (b > a + 180)
                b -= 360;
            return Lerp(a, b, amount);
        }
    }
}