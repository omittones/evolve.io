using System.Collections.Generic;
using System.Linq;
using Accord.Neuro.Learning;
using NeuralMotion.Intelligence;

namespace NeuralMotion.Evolution.Supervised
{
    public class Backpropagator : IEvolver<ActivationNetworkBrain>
    {
        private readonly double[][] inputs;
        private readonly double[][] outputs;
        private readonly ParallelResilientBackpropagationLearning engine;

        public ActivationNetworkBrain CurrentChampGenome { get; private set; }
        public double BestFitness { get; private set; }
        public int CurrentGeneration { get; private set; }

        public int Size => 1;

        public Backpropagator(ActivationNetworkBrain brain, double[][] inputs, double[][] outputs)
        {
            this.CurrentChampGenome = brain;
            this.inputs = inputs;
            this.outputs = outputs;
            this.engine = new ParallelResilientBackpropagationLearning(brain.Network);
            this.CurrentGeneration = 0;
        }

        public void PerformSingleStep()
        {
            this.BestFitness = this.engine.RunEpoch(inputs, outputs);
            CurrentGeneration ++;
        }

        public IEnumerable<string> StatusReport()
        {
            yield break;
        }

        public ActivationNetworkBrain[] GetPopulation()
        {
            return new[]
            {
                this.CurrentChampGenome
            };
        }

        public void SetPopulation(ActivationNetworkBrain[] newPopulation)
        {
            this.CurrentChampGenome = newPopulation.First();
        }

        public void Adjust(ParameterAdjuster adjuster)
        {
        }
    }
}