using System;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Domain.Reinsertions;
using GeneticSharp.Domain.Selections;
using NeuralMotion.Evolution.GeneticSharp.Config;

namespace NeuralMotion.Evolution.GeneticSharp
{
    public sealed class ConfigChromosome : ChromosomeBase
    {
        private readonly IConfig config;
        private readonly bool includeNumberMutation;
        private readonly int resultPopulationSize;
        private readonly int resultChromosomeSize;
        private readonly IFitness resultFitness;

        public ConfigChromosome(
            IConfig config,
            bool includeNumberMutation,
            int resultPopulationSize,
            int resultChromosomeSize,
            IFitness resultFitness) : base(6)
        {
            this.config = config;
            this.includeNumberMutation = includeNumberMutation;
            this.resultPopulationSize = resultPopulationSize;
            this.resultChromosomeSize = resultChromosomeSize;
            this.resultFitness = resultFitness;

            CreateGenes();
        }
        
        public ISelection Selection => (ISelection) GetGene(0).Value;
        public ICrossover Crossover => (ICrossover) GetGene(1).Value;
        public IMutation Mutation => (IMutation) GetGene(2).Value;
        public IReinsertion Reinsertion => (IReinsertion) GetGene(3).Value;
        public double MutationFactor => (double) GetGene(4).Value;
        public double CrossoverFactor => (double) GetGene(5).Value;

        public override IChromosome CreateNew()
        {
            return new ConfigChromosome(
                this.config,
                this.includeNumberMutation,
                this.resultPopulationSize,
                this.resultChromosomeSize,
                this.resultFitness);
        }

        private object CreateService(int geneIndex)
        {
            switch (geneIndex)
            {
                case 0:
                    return config.CreateSelection(this.resultPopulationSize);
                case 1:
                    return config.CreateCrossover(this.resultChromosomeSize);
                case 2:
                    return config.CreateMutation(this.includeNumberMutation);
                case 3:
                    return config.CreateReinsertion(this.resultFitness);
                case 4:
                    return RandomizationProvider.Current.GetDouble(0.1, 0.9);
                case 5:
                    return RandomizationProvider.Current.GetDouble(0.1, 0.9);
                default:
                    throw new InvalidOperationException("Invalid ConfigChromosome gene index.");
            }
        }

        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(CreateService(geneIndex));
        }

        public override string ToString()
        {
            return
                $@"Selection: {this.Selection.GetType().Name}
Crossover: {this.Crossover.GetType().Name}
Mutation: {this.Mutation.GetType().Name}
Reinsertion: {this.Reinsertion.GetType().Name}
MutationFactor:{this.MutationFactor}
CrossoverFactor:{this.CrossoverFactor}";
        }
    }
}