using NeuralMotion.Simulator;

namespace NeuralMotion.Evolution
{
    public struct BallFitness
    {
        public Ball Ball { get; set; }
        public double Fitness { get; set; }
        public BallFitness(Ball ball = null, double fitness = double.MinValue)
        {
            Ball = ball;
            Fitness = fitness;
        }
    }
}