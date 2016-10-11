using System.Diagnostics;
using System.Linq;
using NeuralMotion.Simulator;
using Util;

namespace NeuralMotion.Intelligence
{
    public class SelectClosestAndDistancesInPolar : IInputSelectionStrategy
    {
        public int InputLength => 12;

        public float[] SelectInput(Ball[] allBalls, Ball ball)
        {
            var closestBalls = allBalls
                .OrderBy(b => b.Position.Distance(ball.Position))
                .Take(4)
                .ToArray();

            Debug.Assert(object.ReferenceEquals(closestBalls[0], ball));

            var ballSpeed = ball.Speed.FromCartesianToPolar();

            var polarClosest = closestBalls.Skip(1)
                .Select(b => b.Position.RelativeTo(ball.Position).FromCartesianToPolar())
                .ToArray();

            var selection = new[]
            {
                1 - ball.Position.X,
                ball.Position.X + 1,
                1 - ball.Position.Y,
                ball.Position.Y + 1,
                ballSpeed.Angle,
                polarClosest[0].Angle,
                polarClosest[1].Angle,
                polarClosest[2].Angle,
                ballSpeed.Radius,
                polarClosest[0].Radius,
                polarClosest[1].Radius,
                polarClosest[2].Radius
            };

            Debug.Assert(selection.Length == this.InputLength);

            return selection;
        }

        public float[] TransformOutput(float[] output)
        {
            var polar = new PointP(output[0]*2, output[1]*2*3.141592f);
            var cart = polar.FromPolarToCartesian();
            output[0] = cart.X;
            output[1] = cart.Y;
            return output;
        }
    }
}