using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NeuralMotion.Data;
using NeuralMotion.Views;
using SharpNeat.Core;
using SharpNeat.Domains;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;

namespace NeuralMotion.Evolution.Neat
{
    public class PrivateFunctionRegressionTest : BaseNeatExperiment
    {
        private readonly IFunctionResponse response;
        public override string Name => "Private function regression test";
        public override string Description => "No description";
        public override int InputCount => 1;
        public override int OutputCount => 1;
        public override int DefaultPopulationSize => 100;

        public PrivateFunctionRegressionTest(IFunctionResponse response)
        {
            this.response = response;
        }

        public PrivateFunctionRegressionTest() : this(new WaveformTarget(0, 0))
        {
        }

        public override void Initialize(string name, XmlElement xmlConfig)
        {
        }

        public override List<NeatGenome> LoadPopulation(XmlReader xr)
        {
            return this
                .CreateGenomeFactory()
                .CreateGenomeList(DefaultPopulationSize, 0);
        }

        public override void SavePopulation(XmlWriter xw, IList<NeatGenome> genomeList)
        {
        }

        public override AbstractView CreateGenomeView()
        {
            return new NeatGenomeView();
        }

        public override AbstractView CreateDomainView()
        {
            return new MultipleFunctionView(this.response.Input, this.response.Output);
        }

        public override IPhenomeEvaluator<IBlackBox> CreateEvaluator()
        {
            return new NeatInverseDistanceFromValuesFitness(this.response.Input, this.response.Output);
        }

        public static IEnumerable<string> Status(NeatEvolutionAlgorithm<NeatGenome> engine)
        {
            yield return $"Schema mode: {engine.ComplexityRegulationMode}";
            yield return $"Complexity: {engine.Statistics._meanComplexity}";
            yield return $"Speed: {engine.Statistics._evaluationsPerSec} eps";
            if (engine.CurrentChampGenome != null)
            {
                yield return $"Champ gene count: {engine.CurrentChampGenome.NeuronGeneList.Count}";
                yield return $"Max gene count: {engine.GenomeList.Max(e => e.NeuronGeneList.Count)}";
            }
        }
    }
}