using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Reinsertions;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;

namespace NeuralMotion.Evolution.GeneticSharp
{
    public class Evolver : IChromosomeEvolver
    {
        private readonly GeneticAlgorithm runner;
        private readonly GenerationNumberTermination termination;
        private readonly Population population;

        public Evolver(
            int populationSize,
            IFitness fitness,
            int numberOfGenes) : this(new ArrayChromosome(numberOfGenes), populationSize, fitness)
        {
        }

        public Evolver(
            IChromosome protoChromosome,
            int populationSize,
            IFitness fitness,
            ISelection selection = null,
            ICrossover crossover = null,
            IMutation mutation = null,
            IReinsertion reinsertion = null)
        {
            this.population = new Population(populationSize, populationSize, protoChromosome);
            this.termination = new GenerationNumberTermination(0);

            runner = new GeneticAlgorithm(
                population,
                fitness,
                selection ?? new EliteSelection(),
                crossover ?? new UniformCrossover(),
                mutation ?? new UniformMutation(true));
            runner.Reinsertion = reinsertion ?? new KeepBestParentsReinsertion(fitness);
            runner.Termination = termination;
            runner.Population.GenerationStrategy = new PerformanceGenerationStrategy(0);
        }

        public double MutationProbability
        {
            get { return this.runner.MutationProbability; }
            set { this.runner.MutationProbability = (float)value; }
        }

        public double CrossoverProbability
        {
            get { return this.runner.CrossoverProbability; }
            set { this.runner.CrossoverProbability = (float)value; }
        }

        public IEnumerable<string> StatusReport()
        {
            yield return $"Mutation Probability: {this.runner.MutationProbability:0.000}";
            yield return $"Crossover Probability: {this.runner.CrossoverProbability:0.000}";
        }

        public void Adjust(ParameterAdjuster adjuster)
        {
            this.MutationProbability = (float)adjuster.AdjustValue(0.1, 0.2);
            this.CrossoverProbability = (float)adjuster.AdjustValue(0.75, 0.8);
        }
        
        public IChromosome CurrentChampGenome => runner.BestChromosome;

        public double BestFitness => (runner.Population?.CurrentGeneration?.BestChromosome?.Fitness).GetValueOrDefault(0);
        
        public int CurrentGeneration => runner.GenerationsNumber;

        public int Size => this.population.MaxSize;

        public void PerformSingleStep()
        {
            this.termination.ExpectedGenerationNumber++;

            if (this.CurrentGeneration == 0)
                this.runner.Start();
            else
                this.runner.Resume();

            Debug.Assert(!this.runner.IsRunning);
            Debug.Assert(this.runner.Population.CurrentGeneration.Chromosomes.Count == this.Size);
        }

        public IChromosome[] GetPopulation()
        {
            if (population.CurrentGeneration == null)
                this.runner.Population.CreateInitialGeneration();

            return this.population.CurrentGeneration.Chromosomes.OrderByDescending(c => c.Fitness ?? 0)
                .ToArray();
        }

        public void SetPopulation(IChromosome[] newPopulation)
        {
            if (this.population.CurrentGeneration == null)
                this.population.CreateInitialGeneration();
            if (this.population.CurrentGeneration.Chromosomes.Count != newPopulation.Length)
                throw new NotSupportedException("New population size is not supported!");

            this.population.CurrentGeneration.Chromosomes.Clear();
            foreach (var item in newPopulation)
                this.population.CurrentGeneration.Chromosomes.Add(item);
        }
    }
}