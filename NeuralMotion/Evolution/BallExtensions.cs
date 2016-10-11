using System.Diagnostics;
using GeneticSharp.Domain.Chromosomes;
using NeuralMotion.Intelligence;
using NeuralMotion.Simulator;

namespace NeuralMotion.Evolution
{
    public static class BallExtensions
    {
        public static void LoadBrain(this IBrain brain, double[] values)
        {
            Debug.Assert(brain.NumberOfGenes == values.Length);
            for (var gene = 0; gene < values.Length; gene++)
                brain[gene] = (float) (values[gene]);
        }

        public static void LoadBrain(this Ball ball, double[] values)
        {
            ball.Brain.LoadBrain(values);
        }

        public static void LoadBrain(this IBrain brain, IChromosome values)
        {
            Debug.Assert(brain.NumberOfGenes == values.Length);
            for (var gene = 0; gene < values.Length; gene++)
                brain[gene] = (float) ((double)values.GetGene(gene).Value);
        }

        public static void LoadBrain(this Ball ball, IChromosome values)
        {
            ball.Brain.LoadBrain(values);
        }
    }
}