using System;
using System.Collections.Generic;
using System.Linq;
using NeuralMotion.Evolution.GeneticSharp;
using Util;
using Xunit;
using Xunit.Abstractions;

namespace NeuralMotion.Test
{
    public class Given_evolver
    {
        private readonly ITestOutputHelper output;
        private readonly TargetSumFitness fitness;
        private Evolver evolver;

        public Given_evolver(ITestOutputHelper output)
        {
            this.output = output;
            this.fitness = new TargetSumFitness(10);
            this.evolver = new Evolver(100, this.fitness, 10);
            this.fitness.TimesCalled = 0;
        }

        [Fact]
        public void Evaluating_fitness_is_done_at_most_once_per_chromosome()
        {
            Assert.Equal(0, this.fitness.TimesCalled);
            this.evolver.PerformSingleStep();
            Assert.Equal(100, this.fitness.TimesCalled);
            this.evolver.PerformSingleStep();
            Assert.True(this.fitness.TimesCalled <= 200);
            this.evolver.PerformSingleStep();
            Assert.True(this.fitness.TimesCalled <= 300);
        }

        [Fact]
        public void Run_progresses_single_generation()
        {
            Assert.Equal(0, this.evolver.CurrentGeneration);
            this.evolver.PerformSingleStep();
            Assert.Equal(1, this.evolver.CurrentGeneration);
            this.evolver.PerformSingleStep();
            this.evolver.PerformSingleStep();
            this.evolver.PerformSingleStep();
            Assert.Equal(4, this.evolver.CurrentGeneration);
        }

        [Fact]
        public void Next_population_is_better_than_one_before()
        {
            this.evolver.PerformSingleStep();
            var worseFitness = this.evolver.BestFitness;

            var betterFitness = 0.0;
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                    this.evolver.PerformSingleStep();

                betterFitness = this.evolver.BestFitness;
                this.output.WriteLine($"Fitness value: {betterFitness}");
            }

            Assert.True(betterFitness > worseFitness);
        }
        
        [Fact]
        public void Chromosme_creation_results_in_different_chromosomes()
        {
            var adam = new ArrayChromosome(10);
            var a = adam.CreateNew();
            var b = adam.CreateNew();

            var ada = adam.GetGenes().Select(g => (double) g.Value).ToArray();
            var aa = a.GetGenes().Select(g => (double) g.Value).ToArray();
            var ba = b.GetGenes().Select(g => (double) g.Value).ToArray();

            var avg = ada.Zip(aa, (x, y) => Math.Abs(x - y)).Average();
            Assert.True(avg > 3);

            avg = aa.Zip(ba, (x, y) => Math.Abs(x - y)).Average();
            Assert.True(avg > 3);

            avg = ada.Zip(ba, (x, y) => Math.Abs(x - y)).Average();
            Assert.True(avg > 3);
        }

        [Fact]
        public void Algorithm_should_be_fast()
        {
            var timer = new HiResTimer();
            timer.Reset();
            ConsumeTime();
            var unit = timer.Reset();

            evolver = new Evolver(new ArrayChromosome(20), 50, new AllZerosFitness());

            timer.Reset();
            for (var i = 0; i < 100; i++)
                evolver.PerformSingleStep();
            var durationInUnits = timer.Reset()/unit;

            this.output.WriteLine($"Duration: {durationInUnits}");

            Assert.True(durationInUnits < 70);
        }

        private static void ConsumeTime()
        {
            var list = new List<int>();
            for (var i = 0; i < 1000; i++)
                if (!list.Contains(i))
                    list.Add(i);
        }
    }
}