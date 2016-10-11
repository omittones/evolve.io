using System.Linq;
using GeneticSharp.Domain.Fitnesses;

namespace NeuralMotion.Evolution.GeneticSharp
{
    public class TargetSumFitness : IFitness
    {
        public int TimesCalled { get; set; }
        
        private readonly double targetSum;

        public TargetSumFitness(double targetSum)
        {
            this.targetSum = targetSum;
        }

        public double Evaluate(global::GeneticSharp.Domain.Chromosomes.IChromosome chromosome)
        {
            var values = ((ArrayChromosome) chromosome).Values;
            return Evaluate(values);
        }

        public double Evaluate(double[] values)
        {
            this.TimesCalled++;

            var total = values.Sum();
            total = targetSum - total;
            return 1.0/(total*total + 0.0001);
        }
    }
}