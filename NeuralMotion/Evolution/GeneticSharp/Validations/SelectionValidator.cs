using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;

namespace NeuralMotion.Evolution.GeneticSharp.Validations
{
    public class SelectionValidator : ISelection
    {
        private readonly ISelection selection;

        public SelectionValidator(ISelection selection)
        {
            this.selection = selection;
        }

        public IList<IChromosome> SelectChromosomes(int number, Generation generation)
        {
            if (number == 0)
                throw new ValidationException();
            if (generation.Chromosomes.Count == 0)
                throw new ValidationException();
            var res = this.selection.SelectChromosomes(number, generation);
            if (res.Count == 0)
                throw new ValidationException();
            return res;
        }
    }
}