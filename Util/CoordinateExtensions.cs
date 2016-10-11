using System;
using System.Drawing;

namespace Util
{
    public static class CoordinateExtensions
    {
        public static PointF FromPolarToCartesian(this PointP polar)
        {
            //x = r × cos(θ)
            //y = r × sin(θ)
            return new PointF(
                (float) (polar.Radius*Math.Cos(polar.Angle)),
                (float) (polar.Radius*Math.Sin(polar.Angle)));
        }

        public static PointP FromCartesianToPolar(this PointF cartesian)
        {
            //r = √ (x2 + y2)
            //θ = tan - 1(y / x)
            //I   no change
            //II  Add 180° to the calculator value
            //III Add 180° to the calculator value
            //IV  Add 360° to the calculator value
            return new PointP(cartesian.Length(),
                (float) Math.Atan2(cartesian.Y, cartesian.X));
        }

        public static PointF RelativeTo(this PointF point, PointF origin)
        {
            return new PointF(
                point.X - origin.X,
                point.Y - origin.Y);
        }
    }
}
