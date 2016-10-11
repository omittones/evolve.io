using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;

namespace NeuralMotion.Evolution.GeneticSharp.Validations
{
    public class MutationValidator : IMutation
    {
        private readonly IMutation mutation;

        public MutationValidator(IMutation mutation)
        {
            this.mutation = mutation;
        }

        public bool IsOrdered => this.mutation.IsOrdered;

        public void Mutate(IChromosome chromosome, float probability)
        {
            if (chromosome.Length == 0)
                throw new ValidationException();
            this.mutation.Mutate(chromosome, probability);
            if (chromosome.Length == 0)
                throw new ValidationException();
        }
    }
}