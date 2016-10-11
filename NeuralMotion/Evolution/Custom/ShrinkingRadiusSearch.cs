using System;
using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using NeuralMotion.Evolution.GeneticSharp;
using Util;

namespace NeuralMotion.Evolution.Custom
{
    public class ShrinkingRadiusSearch : IChromosomeEvolver
    {
        private ArrayChromosome[] population;
        private readonly RandomEx randomizer;
        private readonly IFitness fitnessEvaluator;
        private readonly double maxRadius;

        public IEnumerable<string> StatusReport()
        {
            yield return $"Radius: {GenerationRadius:0.00}";
        }

        public int CurrentGeneration { get; private set; }
        public IChromosome CurrentChampGenome { get; private set; }
        public double BestFitness => CurrentChampGenome.Fitness.Value;
        public int Size { get; set; }

        public double GenerationRadius => maxRadius * Math.Pow(0.99, CurrentGeneration);

        public ShrinkingRadiusSearch(
            int size,
            ArrayChromosome adam,
            IFitness fitnessEvaluator,
            double maxRadius = 100.0)
        {
            this.fitnessEvaluator = fitnessEvaluator;
            this.randomizer = RandomEx.Global;
            this.Size = size;
            this.CurrentGeneration = 0;

            this.maxRadius = maxRadius * adam.Length;
            this.population = ComposePopulation(adam, this.maxRadius);

            this.CurrentChampGenome = adam.Clone();
        }

        public void Adjust(ParameterAdjuster adjuster)
        {
        }

        public IChromosome[] GetPopulation()
        {
            return population.Cast<IChromosome>().ToArray();
        }

        public void SetPopulation(IChromosome[] newPopulation)
        {
            this.population = newPopulation.OfType<ArrayChromosome>().ToArray();
        }

        public void PerformSingleStep()
        {
            CalculateFitness();

            SetBestChromosome();

            var center = CalculateCenter();

            CurrentGeneration++;

            population = ComposePopulation(center, this.GenerationRadius);
        }

        private void SetBestChromosome()
        {
            if (!CurrentChampGenome.Fitness.HasValue)
                CurrentChampGenome.Fitness = this.fitnessEvaluator.Evaluate(CurrentChampGenome);

            foreach (var chromosome in this.population)
                if (this.CurrentChampGenome.Fitness < chromosome.Fitness)
                    this.CurrentChampGenome = chromosome;
        }

        private ArrayChromosome CalculateCenter()
        {
            var min = population.Select(p => p.Fitness.Value).Min();
            var max = population.Select(p => p.Fitness.Value).Max();
            var range = max - min;

            var factors = population
                .Select(p =>
                {
                    var factor = (p.Fitness.Value - min)/range;
                    factor = factor*factor*factor*factor;
                    return factor;
                }).ToArray();

            var total = factors.Sum();

            var middle = population
                .Select((c, i) =>
                {
                    var factor = factors[i]/total;
                    return c.Values
                        .Select(v => v*factor)
                        .ToArray();
                }).Aggregate((a, b) =>
                {
                    var sum = a.Zip(b, (a1, b1) => a1 + b1)
                        .ToArray();
                    return sum;
                });

            var best = (ArrayChromosome) this.CurrentChampGenome.CreateNew();
            best.ReplaceGenes(0, middle);
            return best;
        }

        private void CalculateFitness()
        {
            population = population
                .Where(c => !c.Fitness.HasValue)
                .ToArray();

            foreach (var chromosome in population)
                chromosome.Fitness = this.fitnessEvaluator.Evaluate(chromosome);
        }

        private ArrayChromosome[] ComposePopulation(IChromosome center, double radius)
        {
            var newPopulation = Enumerable.Range(0, this.Size - 1)
                .Select(i => center.Clone())
                .Cast<ArrayChromosome>()
                .ToList();

            foreach (var chromosome in newPopulation)
            {
                var difference = randomizer.NextDouble()*radius;
                while (difference > 0)
                {
                    var adjustment = randomizer.NextDouble();
                    adjustment = Math.Min(adjustment, difference);
                    difference -= adjustment;
                    adjustment = adjustment*randomizer.GetRandomSign();

                    var index = randomizer.Next(0, chromosome.Length);
                    var gene = (double) chromosome.GetGene(index).Value;
                    chromosome.ReplaceGene(index, new Gene(gene + adjustment));
                }
            }

            if (CurrentChampGenome != null)
            {
                newPopulation.Add((ArrayChromosome) this.CurrentChampGenome);
            }

            return newPopulation.ToArray();
        }
    }
}