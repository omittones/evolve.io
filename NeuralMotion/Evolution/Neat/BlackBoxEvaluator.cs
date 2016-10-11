using SharpNeat.Phenomes;

namespace NeuralMotion.Evolution.Neat
{
    public static class BlackBoxEvaluator
    {
        public static double[] EvaluateYForX(this IBlackBox box, double[] input)
        {
            var output = new double[input.Length];
            for (var i = 0; i < output.Length; i++)
            {
                box.ResetState();
                box.InputSignalArray[0] = input[i];
                box.Activate();
                output[i] = box.OutputSignalArray[0]*2.0 - 1.0;
            }
            return output;
        }
    }
}