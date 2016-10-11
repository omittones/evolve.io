using System.Linq;
using AForge.Math;
using AForge.Math.Random;
using GeneticSharp.Domain.Chromosomes;

namespace NeuralMotion.Evolution.GeneticSharp
{
    public class ArrayChromosome : ChromosomeBase
    {
        private readonly GaussianGenerator random;

        public double[] Values => this.GetGenes().Select(g => g.Value).Cast<double>().ToArray();

        public ArrayChromosome(int length) : base(length)
        {
            this.random = new GaussianGenerator(0, 1.0f);
            this.ReplaceGenes(0, Enumerable.Range(0, length)
                .Select(this.GenerateGene)
                .ToArray());
        }

        public void ReplaceGenes(int index, double[] genes)
        {
            this.ReplaceGenes(index, genes
                .Select(g => new Gene(g))
                .ToArray());
        }

        public void ReplaceGenes(int index, float[] genes)
        {
            this.ReplaceGenes(index, genes
                .Select(g => new Gene((double) g))
                .ToArray());
        }

        public override Gene GenerateGene(int geneIndex)
        {
            double value = random.Next();
            return new Gene(value);
        }

        public override IChromosome CreateNew()
        {
            return new ArrayChromosome(this.Length);
        }
    }
}