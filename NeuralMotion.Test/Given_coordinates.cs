using System;
using System.Drawing;
using Util;
using Xunit;

namespace NeuralMotion.Test
{
    public class Given_coordinates
    {
        private PointF a;
        private PointF b;
        private PointF c;
        private PointF d;
        private PointF e;
        private PointF x;

        public Given_coordinates()
        {
            this.a = new PointF(1, 1);
            this.b = new PointF(-1, 1);
            this.c = new PointF(-1, -1);
            this.d = new PointF(1, -1);
            this.e = new PointF(0, 0);
            this.x = new PointF(2, 1);
        }

        [Fact]
        public void Polar_conversion_works()
        {
            var ap = this.a.FromCartesianToPolar();
            Assert.Equal(ap.Radius, 1.414, 2);
            Assert.Equal(ap.Angle, Math.PI/4, 2);

            var bp = this.b.FromCartesianToPolar();
            Assert.Equal(bp.Radius, 1.414, 2);
            Assert.Equal(bp.Angle, Math.PI/4*3, 2);

            var cp = this.c.FromCartesianToPolar();
            Assert.Equal(cp.Radius, 1.414, 2);
            Assert.Equal(cp.Angle, -Math.PI/4*3, 2);

            var dp = this.d.FromCartesianToPolar();
            Assert.Equal(dp.Radius, 1.414, 2);
            Assert.Equal(dp.Angle, -Math.PI/4, 2);

            var ep = this.e.FromCartesianToPolar();
            Assert.Equal(0, ep.Radius);
            Assert.Equal(0, ep.Angle);

            var xp = this.x.FromCartesianToPolar();
            Assert.True(xp.Radius > ap.Radius);
            Assert.True(xp.Angle < ap.Angle);

            Assert.Equal(0, ap.FromPolarToCartesian().Distance(a), 3);
            Assert.Equal(0, bp.FromPolarToCartesian().Distance(b), 3);
            Assert.Equal(0, cp.FromPolarToCartesian().Distance(c), 3);
            Assert.Equal(0, dp.FromPolarToCartesian().Distance(d), 3);
        }
    }
}