using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using NeuralMotion.Evolution.GeneticSharp;
using System.Linq;
using Util;

namespace NeuralMotion.Evolution.Custom
{
    public class BruteForceSearch : IChromosomeEvolver
    {
        private readonly IFitness evaluator;
        private readonly double min;
        private readonly double max;
        private readonly double increment;
        private readonly int noIncrements;
        private readonly int[] dimensionPosition;

        public int Size => 1;

        public IEnumerable<string> StatusReport()
        {
            yield break;
        }

        public int CurrentGeneration { get; private set; }

        public IChromosome CurrentChampGenome { get; private set; }

        public double BestFitness => CurrentChampGenome.Fitness.Value;

        public BruteForceSearch(ArrayChromosome adam, IFitness evaluator, double min, double max, double increment)
        {
            this.evaluator = evaluator;
            this.CurrentChampGenome = adam.Clone();
            this.CurrentGeneration = 0;

            this.min = min;
            this.max = max;
            this.increment = increment;
            this.noIncrements = (int) ((max - min)/increment);
            this.dimensionPosition = new int[adam.Length];
            this.dimensionPosition.SetAll(0);
        }

        private bool IncrementDimension(int index)
        {
            if (index >= this.dimensionPosition.Length)
                return true;

            this.dimensionPosition[index]++;
            if (this.dimensionPosition[index] >= noIncrements)
            {
                this.dimensionPosition[index] = 0;
                return IncrementDimension(index + 1);
            }

            return false;
        }

        private ArrayChromosome ConvertToChromosome()
        {
            var genome = this.dimensionPosition.Select(dimension => min + (dimension*increment)).ToArray();

            var chromosome = (ArrayChromosome) this.CurrentChampGenome.Clone();

            chromosome.ReplaceGenes(0, genome);

            return chromosome;
        }

        public void PerformSingleStep()
        {
            if (!this.CurrentChampGenome.Fitness.HasValue)
                this.CurrentChampGenome.Fitness = this.evaluator.Evaluate(this.CurrentChampGenome);

            var chromosome = ConvertToChromosome();

            chromosome.Fitness = this.evaluator.Evaluate(chromosome);

            if (chromosome.Fitness.Value > this.CurrentChampGenome.Fitness.Value)
                this.CurrentChampGenome = chromosome;

            IncrementDimension(0);

            CurrentGeneration++;
        }

        public void Adjust(ParameterAdjuster adjuster)
        {
        }

        public IChromosome[] GetPopulation()
        {
            return new[] {this.CurrentChampGenome};
        }

        public void SetPopulation(IChromosome[] newPopulation)
        {
            this.CurrentChampGenome = newPopulation.First();
        }
    }
}