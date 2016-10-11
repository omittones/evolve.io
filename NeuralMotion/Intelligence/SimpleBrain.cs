using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AForge.Neuro;

namespace NeuralMotion.Intelligence
{
    public class SimpleBrain : IBrain
    {
        private readonly float[] factors;
        private readonly IActivationFunction function;

        public SimpleBrain()
        {
            factors = new float[2];
            function = new BipolarSigmoidFunction();
        }

        public IEnumerator<float> GetEnumerator()
        {
            foreach (var factor in this.factors)
                yield return factor;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.factors.GetEnumerator();
        }

        public float[] GetOutput(float[] inputs)
        {
            return inputs.Select(i => (float) function.Function(i*factors[0] + factors[1])).ToArray();
        }

        public int NumberOfGenes => 2;

        public float this[int geneIndex]
        {
            get { return this.factors[geneIndex]; }
            set { this.factors[geneIndex] = value; }
        }

        public IBrain ExpandGenome(int numberProcessingUnitsToAdd)
        {
            return this;
        }
    }
}