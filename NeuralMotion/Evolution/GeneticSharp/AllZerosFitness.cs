using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;

namespace NeuralMotion.Evolution.GeneticSharp
{
    public class AllZerosFitness : IFitness
    {
        public double Evaluate(IChromosome chromosome)
        {
            var chromo = (ArrayChromosome) chromosome;
            return Evaluate(chromo.Values);
        }

        private static double Evaluate(double[] values)
        {
            var avg = values.Average();
            var min = values.Min();
            var max = values.Max();
            return 1.0/(avg*avg + min*min + max*max + 1.0);
        }
    }
}