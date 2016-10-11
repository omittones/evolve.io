using AForge.Neuro;
using System.Linq;
using NeuralMotion.Intelligence;
using Util;
using Xunit;

namespace NeuralMotion.Test
{
    public class Given_empty_neural_net
    {
        private double[] testData;
        private ActivationNeuron neuron;
        private ActivationNetwork net;

        public Given_empty_neural_net()
        {
            this.testData = new double[] {4, 4};
            this.neuron = new ActivationNeuron(2, new BipolarSigmoidFunction());
            this.net = new ActivationNetwork(new BipolarSigmoidFunction(), 2, 2, 1);
        }

        [Fact]
        public void Zero_weights_output_zero()
        {
            neuron.Weights[0] = 0;
            neuron.Weights[1] = 0;
            neuron.Threshold = 0;
            Assert.Equal(0, neuron.Compute(testData));
        }

        [Fact]
        public void Result_is_within_bounds()
        {
            neuron.Weights[0] = 1;
            neuron.Weights[1] = 1;
            neuron.Threshold = 0;
            var output = neuron.Compute(testData);
            Assert.Equal(1, output, 2);

            neuron.Weights[0] = -1;
            neuron.Weights[1] = -1;
            neuron.Threshold = 0;
            output = neuron.Compute(testData);
            Assert.Equal(-1, output, 2);
        }

        [Fact]
        public void Expansion_expands_genome()
        {
            IBrain brain = new ActivationNetworkBrain(2, 3, 3, 1);
            var smallGenomeCount = brain.NumberOfGenes;

            brain = brain.ExpandGenome(2);
            var medGenomeCount = brain.NumberOfGenes;

            brain = brain.ExpandGenome(5);
            var highGenomeCount = brain.NumberOfGenes;

            Assert.True(smallGenomeCount < medGenomeCount);
            Assert.True(medGenomeCount < highGenomeCount);
        }

        [Fact]
        public void Expansion_produces_same_results()
        {
            IBrain brain = new ActivationNetworkBrain(2, 3, 3, 1);

            var first = brain.ExpandGenome(1);
            var second = brain.ExpandGenome(1);

            Assert.Equal(first.NumberOfGenes, second.NumberOfGenes);

            var third = brain.ExpandGenome(4);

            var fourth = brain.ExpandGenome(2)
                .ExpandGenome(1)
                .ExpandGenome(1);

            Assert.Equal(third.NumberOfGenes, fourth.NumberOfGenes);
        }

        [Fact]
        public void Expansion_keeps_same_functionality()
        {
            IBrain brain = new ActivationNetworkBrain(2, 3, 3, 1);
            for (var i = 0; i < brain.NumberOfGenes; i++)
                brain[i] = (float) RandomEx.Global.NextDouble();

            var testInputs = Enumerable.Range(0, 100)
                .Select(i => new float[]
                {
                    (float) (RandomEx.Global.GetRandomSign()*RandomEx.Global.NextDouble()),
                    (float) (RandomEx.Global.GetRandomSign()*RandomEx.Global.NextDouble())
                }).ToArray();

            var testOutputs = testInputs.Select(i => brain.GetOutput(i)[0]).ToArray();

            brain = brain.ExpandGenome(20);

            var newOutputs = testInputs.Select(i => brain.GetOutput(i)[0]).ToArray();

            for (var i = 0; i < testOutputs.Length; i++)
                Assert.Equal(testOutputs[i], newOutputs[i]);
        }
    }
}