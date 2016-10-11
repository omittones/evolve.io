using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace NeuralMotion.Evolution.Neat
{
    public class NeatInverseDistanceFromValuesFitness : IPhenomeEvaluator<IBlackBox>
    {
        public double[] Input { get; set; }
        public double[] ExpectedOutput { get; set; }

        public NeatInverseDistanceFromValuesFitness(
            double[] input,
            double[] expectedOutput)
        {
            this.Input = input;
            this.ExpectedOutput = expectedOutput;
        }

        /// <summary>
        /// Gets the total number of evaluations that have been performed.
        /// </summary>
        public ulong EvaluationCount { get; private set; }

        /// <summary>
        /// Gets a value indicating whether some goal fitness has been achieved and that
        /// the the evolutionary algorithm/search should stop. This property's value can remain false
        /// to allow the algorithm to run indefinitely.
        /// </summary>
        public bool StopConditionSatisfied => false;

        public FitnessInfo Evaluate(IBlackBox box)
        {
            this.EvaluationCount++;

            var errorAcc = 0.00000001;

            var output = box.EvaluateYForX(Input);

            for (var i = 0; i < Input.Length; i++)
            {
                var diff = output[i] - this.ExpectedOutput[i];
                errorAcc += diff*diff;
            }

            return new FitnessInfo(1.0/errorAcc, 1.0/errorAcc);
        }

        public void Reset()
        {
        }
    }
}