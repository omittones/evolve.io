using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using NeuralMotion.Intelligence;

namespace NeuralMotion.Evolution.GeneticSharp
{
    public class InverseDistanceFromValuesFitness : IFitness
    {
        public double[] Input { get; set; }
        public double[] ExpectedOutput { get; set; }

        private readonly IBrain evaluator;

        public InverseDistanceFromValuesFitness(
            IBrain evaluator,
            double[] input,
            double[] expectedOutput)
        {
            this.evaluator = evaluator;
            this.Input = input;
            this.ExpectedOutput = expectedOutput;
        }

        public double Evaluate(IChromosome chromosome)
        {
            var chromo = (ArrayChromosome) chromosome;

            evaluator.LoadBrain(chromo.Values);

            return Evaluate(this.evaluator);
        }

        public double Evaluate(IBrain brain)
        {
            return 1.0/(this.Input.Select((input, index) =>
            {
                var output = evaluator.GetOutput(new[] {(float) input})[0];
                var diff = ExpectedOutput[index] - output;
                return diff*diff;
            }).Average() + 0.00001);
        }
    }
}