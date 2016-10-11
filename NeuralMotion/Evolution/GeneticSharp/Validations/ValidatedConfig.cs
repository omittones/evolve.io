using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Reinsertions;
using GeneticSharp.Domain.Selections;
using NeuralMotion.Evolution.GeneticSharp.Config;

namespace NeuralMotion.Evolution.GeneticSharp.Validations
{
    public class ValidatedConfig : IConfig
    {
        private readonly IConfig inner;

        public ValidatedConfig(IConfig inner)
        {
            this.inner = inner;
        }

        public ICrossover CreateCrossover(int resultsChromosomeSize)
        {
            return new CrossoverValidator(this.inner.CreateCrossover(resultsChromosomeSize));
        }

        public IMutation CreateMutation(bool includeNumberMutation)
        {
            return new MutationValidator(this.inner.CreateMutation(includeNumberMutation));
        }

        public IReinsertion CreateReinsertion(IFitness resultFitness)
        {
            return new ReinsertionValidator(this.inner.CreateReinsertion(resultFitness));
        }

        public ISelection CreateSelection(int resultsPopulationSize)
        {
            return new SelectionValidator(this.inner.CreateSelection(resultsPopulationSize));
        }
    }
}