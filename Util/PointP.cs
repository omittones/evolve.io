using System;

namespace Util
{
    public struct PointP
    {
        public float Radius { get; set; }
        public float Angle { get; set; }

        public PointP(float radius, float angle)
        {
            Radius = radius;
            Angle = angle;
        }

        public override string ToString()
        {
            return $"{this.Radius:0.000}pt {Angle/Math.PI*180:0.00}deg";
        }
    }
}