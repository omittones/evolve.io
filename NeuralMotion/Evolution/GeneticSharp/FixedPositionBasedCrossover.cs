using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;

namespace NeuralMotion.Evolution.GeneticSharp
{
    public class FixedPositionBasedCrossover : PositionBasedCrossover
    {
        protected override void ValidateParents(IList<IChromosome> parents)
        {
        }

        protected override IChromosome CreateChild(IChromosome firstParent, IChromosome secondParent, int[] swapIndexes)
        {
            var firstParentGenes = new List<Gene>(firstParent.GetGenes());

            var child = firstParent.CreateNew();

            for (int i = 0; i < firstParent.Length; i++)
            {
                if (swapIndexes.Contains(i))
                {
                    var gene = secondParent.GetGene(i);
                    firstParentGenes[i] = gene;
                }
            }

            child.ReplaceGenes(0, firstParentGenes.ToArray());

            return child;
        }
    }
}