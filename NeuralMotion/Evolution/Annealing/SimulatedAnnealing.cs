using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using NeuralMotion.Evolution.GeneticSharp;
using Util;

namespace NeuralMotion.Evolution.Annealing
{
    internal class SimulatedAnnealing : IChromosomeEvolver
    {
        private readonly IFitness fitnessEvaluator;

        private int geneIndex;
        private sbyte geneDirection;
        private double geneStep;

        public IChromosome CurrentChampGenome { get; private set; }
        public double BestFitness => this.CurrentChampGenome.Fitness.Value;
        
        public int Size => 1;

        public double currentTemperature;
        public double cutoffTemperature;
        public double alpha;

        public IEnumerable<string> StatusReport()
        {
            yield return $"Change: {geneStep:0.0000}";
            yield return $"Current Temp: {currentTemperature:0.0}";
            yield return $"Cutoff Temp: {cutoffTemperature:0.0}";
            yield return $"Alpha: {alpha:0.0000}";
            yield return $"Better Taken: {BetterSolutionsTaken}";
            yield return $"Worse Taken: {WorseSolutionsTaken}";
        }

        public int CurrentGeneration { get; private set; }
        public int BetterSolutionsTaken { get; private set; }
        public int WorseSolutionsTaken { get; private set; }

        public SimulatedAnnealing(ArrayChromosome adam, IFitness fitnessEvaluator, double alpha = 0.999)
        {
            this.fitnessEvaluator = fitnessEvaluator;
            this.geneIndex = 0;
            this.geneDirection = 1;
            this.geneStep = 0.1;

            this.currentTemperature = 100;
            this.cutoffTemperature = 1;
            this.alpha = alpha;

            this.CurrentChampGenome = adam;
        }

        public bool ShouldTakeSolution(double delta)
        {
            Debug.Assert(delta <= 0);

            var random = RandomEx.Global.NextDouble();
            random = random*(1.0 - delta/this.BestFitness);
            return random < (this.currentTemperature/100.0);
        }

        public void PerformSingleStep()
        {
            if (!this.CurrentChampGenome.Fitness.HasValue)
                this.CurrentChampGenome.Fitness = this.fitnessEvaluator.Evaluate(this.CurrentChampGenome);

            var changed = ApplyChangeAndCalculateFitness();

            var delta = changed.Fitness.Value - this.CurrentChampGenome.Fitness.Value;
            if (delta > 0)
            {
                this.CurrentChampGenome = changed;
                BetterSolutionsTaken++;
            }
            else
            {
                if (currentTemperature > cutoffTemperature &&
                    ShouldTakeSolution(delta))
                {
                    this.CurrentChampGenome = changed;
                    WorseSolutionsTaken++;
                }
                else
                {
                    geneIndex = RandomEx.Global.Next(0, this.CurrentChampGenome.Length);
                    geneDirection = RandomEx.Global.GetRandomSign();
                }
            }

            currentTemperature *= alpha;

            CurrentGeneration++;
        }

        private ArrayChromosome ApplyChangeAndCalculateFitness()
        {
            var changed = (ArrayChromosome) this.CurrentChampGenome.Clone();
            var value = changed.Values[geneIndex];
            value += geneDirection*geneStep;
            changed.ReplaceGene(geneIndex, new Gene(value));
            changed.Fitness = this.fitnessEvaluator.Evaluate(changed);

            return changed;
        }
        
        public void Adjust(ParameterAdjuster adjuster)
        {
            this.geneStep = adjuster.AdjustValue(0.01, 1);
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