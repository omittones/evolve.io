using NeuralMotion.Evolution.Custom;
using NeuralMotion.Evolution.GeneticSharp;
using Xunit;

namespace NeuralMotion.Test
{
    public class Given_radius_search
    {
        private readonly ShrinkingRadiusSearch search;

        public Given_radius_search()
        {
            this.search = new ShrinkingRadiusSearch(
                100,
                new ArrayChromosome(100),
                new AllZerosFitness());
        }

        [Fact]
        public void Works()
        {
            while (search.CurrentGeneration < 100)
            {
                search.PerformSingleStep();
            }
        }
    }
}