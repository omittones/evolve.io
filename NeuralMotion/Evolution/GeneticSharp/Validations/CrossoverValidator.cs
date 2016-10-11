using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;

namespace NeuralMotion.Evolution.GeneticSharp.Validations
{
    public class CrossoverValidator : ICrossover
    {
        private readonly ICrossover crossover;

        public CrossoverValidator(ICrossover crossover)
        {
            this.crossover = crossover;
        }

        public bool IsOrdered => crossover.IsOrdered;
        public int ParentsNumber => crossover.ParentsNumber;
        public int ChildrenNumber => crossover.ChildrenNumber;
        public int MinChromosomeLength => crossover.MinChromosomeLength;

        public IList<IChromosome> Cross(IList<IChromosome> parents)
        {
            var res = this.crossover.Cross(parents);
            if (res.Count == 0)
                throw new ValidationException();
            return res;
        }
    }
}