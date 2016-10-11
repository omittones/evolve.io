using System.Collections.Generic;

namespace NeuralMotion.Evolution
{
    public interface IEvolver<TChromosome>
    {
        int Size { get; }

        void PerformSingleStep();
        
        void Adjust(ParameterAdjuster adjuster);
        
        int CurrentGeneration { get; }

        double BestFitness { get; }

        IEnumerable<string> StatusReport();

        TChromosome CurrentChampGenome { get; }

        TChromosome[] GetPopulation();

        void SetPopulation(TChromosome[] newPopulation);
    }
}