using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Reinsertions;

namespace NeuralMotion.Evolution.GeneticSharp
{
    public class KeepBestParentsReinsertion : IReinsertion
    {
        private readonly IFitness evaluator;

        public bool CanCollapse => false;
        public bool CanExpand => false;

        public KeepBestParentsReinsertion(IFitness evaluator)
        {
            this.evaluator = evaluator;
        }

        public IList<IChromosome> SelectChromosomes(
            IPopulation population,
            IList<IChromosome> offspring,
            IList<IChromosome> parents)
        {
            var bestParents = parents
                .OrderByDescending(p =>
                {
                    if (!p.Fitness.HasValue)
                        p.Fitness = this.evaluator.Evaluate(p);
                    return p.Fitness.Value;
                })
                .Take(2)
                .ToArray();

            var diff = offspring.Count + bestParents.Length - population.MaxSize;
            if (diff > 0)
            {
                var parentFitness = bestParents.Last().Fitness.Value;
                for (var i = 0; i < offspring.Count; i++)
                {
                    if (!offspring[i].Fitness.HasValue)
                        offspring[i].Fitness = this.evaluator.Evaluate(offspring[i]);

                    if (offspring[i].Fitness.Value < parentFitness)
                    {
                        offspring[i] = null;
                        diff--;
                        if (diff == 0)
                            break;
                    }
                }

                return offspring
                    .Where(o => o != null)
                    .Concat(bestParents)
                    .Take(population.MaxSize)
                    .ToList();
            }
            else
            {
                return offspring
                    .Concat(bestParents)
                    .Concat(Enumerable.Range(0, -diff).Select(i => population.BestChromosome.CreateNew()))
                    .ToList();
            }
        }
    }
}