using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using NeuralMotion.Evolution.GeneticSharp;

namespace NeuralMotion.Evolution
{
    public static class ChromosomeEvolverExtensions
    {
        public static double[][] GetPopulation(this IEvolver<IChromosome> evolver)
        {
            return evolver
                .GetPopulation()
                .Cast<ArrayChromosome>()
                .Select(c => c.Values)
                .ToArray();
        }

        public static void SetPopulation(this IEvolver<IChromosome> evolver, double[][] newPopulation)
        {
            var populus = newPopulation.Select(p =>
            {
                var chromo = new ArrayChromosome(p.Length);
                chromo.ReplaceGenes(0, p);
                return chromo;
            }).ToArray();

            evolver.SetPopulation(populus);
        }
    }
}