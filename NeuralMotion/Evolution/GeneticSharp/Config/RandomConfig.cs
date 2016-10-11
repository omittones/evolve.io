using System;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Domain.Reinsertions;
using GeneticSharp.Domain.Selections;

namespace NeuralMotion.Evolution.GeneticSharp.Config
{
    public class RandomConfig : IConfig
    {
        private static readonly IRandomization randomization = RandomizationProvider.Current;

        public ISelection CreateSelection(int resultsPopulationSize)
        {
            var index = randomization.GetInt(0, 3);
            switch (index)
            {
                case 0:
                    return new EliteSelection();
                case 1:
                    return new RouletteWheelSelection();
                case 2:
                    return new StochasticUniversalSamplingSelection();
                //case 3:
                //    return new TournamentSelection(
                //        randomization.GetInt(2, resultsPopulationSize - 1),
                //        randomization.GetInt(0, 2) == 0);
                default:
                    throw new InvalidProgramException();
            }
        }

        public IMutation CreateMutation(bool includeNumberMutation)
        {
            var index = randomization.GetInt(0, 4);
            switch (index)
            {
                case 0:
                    return new ReverseSequenceMutation();
                case 1:
                    return new TworsMutation();
                case 2:
                    return new UniformMutation();
                case 3:
                    if (includeNumberMutation)
                        return new NumberMutation();
                    else
                        return CreateMutation(false);
                default:
                    throw new InvalidOperationException();
            }
        }
        
        public ICrossover CreateCrossover(int resultsChromosomeSize)
        {
            var index = randomization.GetInt(0, 10);
            switch (index)
            {
                case 0:
                    return new CutAndSpliceCrossover();
                case 1:
                    return new CycleCrossover();
                case 2:
                    var p = randomization.GetInt(1, resultsChromosomeSize - 1);
                    return new OnePointCrossover(p);
                case 3:
                    return new OrderBasedCrossover();
                case 4:
                    return new OrderedCrossover();
                case 5:
                    return new PartiallyMappedCrossover();
                case 6:
                    return new FixedPositionBasedCrossover();
                case 7:
                    return new ThreeParentCrossover();
                case 8:
                    var p1 = randomization.GetInt(1, resultsChromosomeSize/2);
                    var p2 = randomization.GetInt(p1 + 1, resultsChromosomeSize - 1);
                    return new TwoPointCrossover(p1, p2);
                case 9:
                    return new UniformCrossover();
                default:
                    throw new InvalidOperationException();
            }
        }

        public IReinsertion CreateReinsertion(IFitness resultFitness)
        {
            var index = randomization.GetInt(0, 3);
            switch (index)
            {
                case 0:
                    return new ElitistReinsertion();
                case 1:
                    return new KeepBestParentsReinsertion(resultFitness);
                case 2:
                    return new UniformReinsertion();
                //case 3:
                //    return new PureReinsertion();
                //case 4:
                //    return new FitnessBasedReinsertion();
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}