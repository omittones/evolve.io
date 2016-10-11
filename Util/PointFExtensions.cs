using System;
using System.Drawing;

namespace Util
{
    public static class PointFExtensions
    {
        public static PointF Offset(this PointF obj, PointF point)
        {
            return new PointF(obj.X + point.X, obj.Y + point.Y);
        }

        public static float Distance(this PointF @from, float x, float y)
        {
            x = @from.X - x;
            y = @from.Y - y;
            return (float) Math.Sqrt(x * x + y * y);
        }

        public static PointF Offset(this PointF obj, float x, float y)
        {
            return new PointF(obj.X + x, obj.Y + y);
        }

        public static PointF Scale(this PointF obj, float x, float y)
        {
            return new PointF(obj.X * x, obj.Y * y);
        }

        public static PointF Scale(this PointF obj, float x)
        {
            return new PointF(obj.X * x, obj.Y * x);
        }

        public static PointF Negative(this PointF obj)
        {
            return new PointF(-obj.X, -obj.Y);
        }

        public static float Distance(this PointF @from, PointF to)
        {
            var x = @from.X - to.X;
            var y = @from.Y - to.Y;
            return (float) Math.Sqrt(x*x + y*y);
        }

        public static float Length(this PointF obj)
        {
            return obj.Distance(new PointF(0, 0));
        }
    }
}