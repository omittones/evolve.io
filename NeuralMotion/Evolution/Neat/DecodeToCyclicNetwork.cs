using SharpNeat.Core;
using SharpNeat.Decoders;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;

namespace NeuralMotion.Evolution.Neat
{
    public class DecodeToCyclicNetwork : IGenomeDecoder<NeatGenome, IBlackBox>
    {
        private readonly NetworkActivationScheme scheme;

        public DecodeToCyclicNetwork()
        {
            this.scheme = NetworkActivationScheme.CreateCyclicRelaxingActivationScheme(0.01, 100, true);
        }

        public IBlackBox Decode(NeatGenome genome)
        {
            var network = FastCyclicNetworkFactory.CreateFastCyclicNetwork(genome, scheme);
            return network;
        }
    }
}