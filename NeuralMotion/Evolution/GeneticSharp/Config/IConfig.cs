using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Reinsertions;
using GeneticSharp.Domain.Selections;

namespace NeuralMotion.Evolution.GeneticSharp.Config
{
    public interface IConfig
    {
        ICrossover CreateCrossover(int resultsChromosomeSize);

        IMutation CreateMutation(bool includeNumberMutation);

        IReinsertion CreateReinsertion(IFitness resultFitness);

        ISelection CreateSelection(int resultsPopulationSize);
    }
}