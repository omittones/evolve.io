using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AForge.Neuro;
using Util;

namespace NeuralMotion.Intelligence
{
    public class ActivationNetworkBrain : IBrain
    {
        private readonly int lastExpandedLayer;
        private KeyValuePair<ActivationNeuron, int>[] neuronWeights;
        public ActivationNetwork Network { get; }
        public IActivationFunction ActivationFunction { get; }

        public ActivationNetworkBrain(int inputsCount, params int[] layerCounts)
        {
            this.ActivationFunction = new BipolarSigmoidFunction();
            this.Network = new ActivationNetwork(this.ActivationFunction, inputsCount, layerCounts);
            this.lastExpandedLayer = 0;

            var initializator = new Accord.Neuro.NguyenWidrow(this.Network);
            initializator.Randomize();

            RenderGenome();
        }

        private ActivationNetworkBrain(ActivationNetwork network, IActivationFunction function, int lastExpandedLayer)
        {
            this.Network = network;
            this.ActivationFunction = function;
            this.lastExpandedLayer = lastExpandedLayer;

            RenderGenome();
        }

        public int NumberOfGenes => this.neuronWeights.Length;

        public float[] GetOutput(float[] inputs)
        {
            var doubles = inputs.Select(e => (double) e).ToArray();
            doubles = this.Network.Compute(doubles);
            return doubles.Select(e => (float) e).ToArray();
        }

        public float this[int geneIndex]
        {
            get
            {
                var neuron = this.neuronWeights[geneIndex];
                if (neuron.Value < 0)
                    return (float) neuron.Key.Threshold;
                else
                    return (float) neuron.Key.Weights[neuron.Value];
            }
            set
            {
                var neuron = this.neuronWeights[geneIndex];
                if (neuron.Value < 0)
                    neuron.Key.Threshold = value;
                else
                    neuron.Key.Weights[neuron.Value] = value;
            }
        }

        private void RenderGenome()
        {
            var neurons = this.Network.Layers
                .SelectMany(n => n.Neurons)
                .Cast<ActivationNeuron>()
                .ToArray();

            this.neuronWeights = neurons
                .SelectMany(neuron =>
                    neuron.Weights
                        .Select((value, weightIndex) => new KeyValuePair<ActivationNeuron, int>(neuron, weightIndex))
                        .Concat(new[]
                        {
                            new KeyValuePair<ActivationNeuron, int>(neuron, -1)
                        })).ToArray();
        }

        public IBrain ExpandGenome(int numberProcessingUnitsToAdd)
        {
            var newLayerSizes = this.Network.Layers.Select(l => l.Neurons.Length).ToArray();
            var startExpandingFrom = this.lastExpandedLayer;

            for (var i = 0; i < numberProcessingUnitsToAdd; i++)
            {
                //-1 because last layer is output
                startExpandingFrom = (startExpandingFrom + 1) % newLayerSizes.Length;
                newLayerSizes[startExpandingFrom]++;
            }

            var expandedNet = new ActivationNetwork(this.ActivationFunction, this.Network.InputsCount, newLayerSizes);
            for (var xLayer = 0; xLayer < newLayerSizes.Length; xLayer++)
            {
                var sourceLayer = this.Network.Layers[xLayer];
                var destinationLayer = expandedNet.Layers[xLayer];
                for (var xNeuron = 0; xNeuron < destinationLayer.Neurons.Length; xNeuron++)
                {
                    var destinationNeuron = (ActivationNeuron) destinationLayer.Neurons[xNeuron];
                    if (xNeuron < sourceLayer.Neurons.Length)
                    {
                        var sourceNeuron = (ActivationNeuron) sourceLayer.Neurons[xNeuron];
                        for (var xWeight = 0; xWeight < destinationNeuron.Weights.Length; xWeight++)
                        {
                            if (xWeight < sourceNeuron.Weights.Length)
                                destinationNeuron.Weights[xWeight] = sourceNeuron.Weights[xWeight];
                            else
                                destinationNeuron.Weights[xWeight] = 0;
                        }
                        destinationNeuron.Threshold = sourceNeuron.Threshold;
                    }
                    else
                    {
                        destinationNeuron.Weights.SetAll(0);
                        destinationNeuron.Threshold = 0;
                    }
                }
            }

            return new ActivationNetworkBrain(expandedNet, this.ActivationFunction, startExpandingFrom);
        }

        public IEnumerator<float> GetEnumerator()
        {
            for (var i = 0; i < this.NumberOfGenes; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
