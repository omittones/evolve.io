using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Reinsertions;

namespace NeuralMotion.Evolution.GeneticSharp.Validations
{
    public class ReinsertionValidator : IReinsertion
    {
        private readonly IReinsertion reinsertion;

        public ReinsertionValidator(IReinsertion reinsertion)
        {
            this.reinsertion = reinsertion;
        }

        public bool CanCollapse => this.reinsertion.CanCollapse;
        public bool CanExpand => this.reinsertion.CanExpand;

        public IList<IChromosome> SelectChromosomes(IPopulation population, IList<IChromosome> offspring,
            IList<IChromosome> parents)
        {
            if (parents.Count == 0 && offspring.Count == 0)
                throw new ValidationException();

            if (offspring.Count == 0 && this.reinsertion is UniformReinsertion)
                offspring.Add(parents.OrderByDescending(p => p.Fitness.Value).First());

            var res = this.reinsertion.SelectChromosomes(population, offspring, parents);
            if (res.Count == 0)
                throw new ValidationException();

            return res;
        }
    }
}