using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using SharpNeat.Core;
using SharpNeat.DistanceMetrics;
using SharpNeat.Domains;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.EvolutionAlgorithms.ComplexityRegulation;
using SharpNeat.Genomes.Neat;
using SharpNeat.Genomes.RbfNeat;
using SharpNeat.Network;
using SharpNeat.Phenomes;
using SharpNeat.SpeciationStrategies;

namespace NeuralMotion.Evolution.Neat
{
    public abstract class BaseNeatExperiment : IGuiNeatExperiment
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract int InputCount { get; }
        public abstract int OutputCount { get; }
        public abstract int DefaultPopulationSize { get; }

        protected NeatEvolutionAlgorithmParameters algorithmParameters;
        public virtual NeatEvolutionAlgorithmParameters NeatEvolutionAlgorithmParameters
        {
            get
            {
                if (algorithmParameters == null)
                {
                    this.algorithmParameters = new NeatEvolutionAlgorithmParameters
                    {
                        OffspringAsexualProportion = 0.2,
                        OffspringSexualProportion = 0.75,
                        InterspeciesMatingProportion = 0.5,
                        SpecieCount = DefaultPopulationSize/50,
                        ElitismProportion = 0.2,
                        SelectionProportion = 0.5
                    };
                }

                return algorithmParameters;
            }
        }

        protected NeatGenomeParameters genomeParameters;
        public virtual NeatGenomeParameters NeatGenomeParameters
        {
            get
            {
                if (genomeParameters == null)
                {
                    genomeParameters = new NeatGenomeParameters
                    {
                        FeedforwardOnly = false,
                        ConnectionWeightRange = 30,
                        InitialInterconnectionsProportion = 0.2,
                        AddConnectionMutationProbability = 0.1,
                        AddNodeMutationProbability = 0.02,
                        DeleteConnectionMutationProbability = 0.1,
                        ConnectionWeightMutationProbability = 1.0,
                        NodeAuxStateMutationProbability = 0.2
                    };

                    var factor = 50.0;
                    var items = genomeParameters.ConnectionMutationInfoList.ToArray();
                    genomeParameters.ConnectionMutationInfoList.Clear();
                    foreach (var item in items)
                        genomeParameters.ConnectionMutationInfoList.Add(new ConnectionMutationInfo(
                            item.ActivationProbability,
                            item.PerturbanceType,
                            item.SelectionType,
                            item.SelectionProportion,
                            item.SelectionQuantity,
                            item.PerturbanceMagnitude*factor,
                            item.Sigma*factor));
                    genomeParameters.ConnectionMutationInfoList.Initialize();
                }

                return genomeParameters;
            }
        }

        public IGenomeDecoder<NeatGenome, IBlackBox> CreateGenomeDecoder()
        {
            return new DecodeToCyclicNetwork();
        }

        public IGenomeFactory<NeatGenome> CreateGenomeFactory()
        {
            return new RbfGenomeFactory(this.InputCount, this.OutputCount,
                DefaultActivationFunctionLibrary.CreateLibraryCppn(),
                this.NeatGenomeParameters);
        }

        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm()
        {
            return CreateEvolutionAlgorithm(DefaultPopulationSize);
        }

        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(int populationSize)
        {
            var factory = new RbfGenomeFactory(this.InputCount,
                this.OutputCount,
                DefaultActivationFunctionLibrary.CreateLibraryCppn(),
                this.NeatGenomeParameters);

            var list = factory.CreateGenomeList(populationSize, 0);

            return CreateEvolutionAlgorithm(factory, list);
        }

        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(IGenomeFactory<NeatGenome> genomeFactory,
            List<NeatGenome> genomeList)
        {
            var options = new ParallelOptions {MaxDegreeOfParallelism = 1};

            // Create distance metric. Mismatched genes have a fixed distance of 10; for matched genes the distance is their weigth difference.
            var distanceMetric = new ManhattanDistanceMetric(1.0, 0.0, 10.0);
            var speciationStrategy = new ParallelKMeansClusteringStrategy<NeatGenome>(distanceMetric, options);

            // Create complexity regulation strategy.
            var complexityRegulator = new DefaultComplexityRegulationStrategy(ComplexityCeilingType.Relative, 10.0);

            // Create the evolution algorithm.
            var engine = new NeatEvolutionAlgorithm<NeatGenome>(
                this.NeatEvolutionAlgorithmParameters,
                speciationStrategy,
                complexityRegulator);

            var decoder = CreateGenomeDecoder();

            var evaluator = new ParallelGenomeListEvaluator<NeatGenome, IBlackBox>(decoder, this.CreateEvaluator(),
                options);

            //Wrap the list evaluator in a 'selective' evaulator that will only evaluate new genomes. That is, we skip re-evaluating any genomes
            //that were in the population in previous generations (elite genomes). This is determiend by examining each genome's evaluation info object.
            var selectiveEvaluator = new SelectiveGenomeListEvaluator<NeatGenome>(evaluator,
                SelectiveGenomeListEvaluator<NeatGenome>.CreatePredicate_OnceOnly());

            // Initialize the evolution algorithm.
            engine.Initialize(selectiveEvaluator, genomeFactory, genomeList);

            return engine;
        }

        public abstract void Initialize(string name, XmlElement xmlConfig);

        public abstract IPhenomeEvaluator<IBlackBox> CreateEvaluator();

        public abstract List<NeatGenome> LoadPopulation(XmlReader xr);

        public abstract void SavePopulation(XmlWriter xw, IList<NeatGenome> genomeList);

        public abstract AbstractView CreateGenomeView();

        public abstract AbstractView CreateDomainView();
    }
}