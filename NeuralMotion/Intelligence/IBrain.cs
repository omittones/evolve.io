using System.Collections.Generic;

namespace NeuralMotion.Intelligence
{
    public interface IBrain : IEnumerable<float>
    {
        float[] GetOutput(float[] inputs);

        int NumberOfGenes { get; }

        float this[int geneIndex] { get; set; }

        IBrain ExpandGenome(int numberProcessingUnitsToAdd);
    }
}