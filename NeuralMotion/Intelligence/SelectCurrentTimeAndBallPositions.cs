using System;
using NeuralMotion.Simulator;

namespace NeuralMotion.Intelligence
{
    public class SelectCurrentTimeAndBallPositions : IInputSelectionStrategy
    {
        private readonly Func<float> timeSelector;

        public SelectCurrentTimeAndBallPositions(Func<float> timeSelector)
        {
            this.timeSelector = timeSelector;
        }

        public int InputLength => 9;

        public float[] SelectInput(Ball[] allBalls, Ball ball)
        {
            var minIndex = 0;
            for (var i = 0; i < ball.Distances.Length; i++)
                if (ball.Distances[i] < ball.Distances[minIndex])
                    minIndex = i;
            var closestBall = allBalls[minIndex];

            return new[]
            {
                timeSelector(),
                closestBall.Position.X,
                closestBall.Position.Y,
                ball.StartingPosition.X,
                ball.StartingPosition.Y,
                ball.Position.X,
                ball.Position.Y,
                ball.Speed.X,
                ball.Speed.Y
            };
        }

        public float[] TransformOutput(float[] output)
        {
            return output;
        }
    }
}
