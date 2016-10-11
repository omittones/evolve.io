using NeuralMotion.Simulator;

namespace NeuralMotion.Intelligence
{
    public interface IInputSelectionStrategy
    {
        int InputLength { get; }

        float[] SelectInput(Ball[] allBalls, Ball ball);

        float[] TransformOutput(float[] output);
    }
}