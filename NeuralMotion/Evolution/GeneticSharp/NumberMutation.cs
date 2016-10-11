using System;
using System.Linq;
using AForge.Math.Random;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using Util;

namespace NeuralMotion.Evolution.GeneticSharp
{
    public class NumberMutation : IMutation
    {
        private readonly double intensity;
        private readonly GaussianGenerator gaussian;

        public bool IsOrdered => true;

        public NumberMutation(double intensity = 1.0f)
        {
            this.intensity = intensity;
            this.gaussian = new GaussianGenerator(0, 3.0f);
            gaussian.SetSeed(DateTime.Now.Millisecond);
        }

        public void Mutate(IChromosome chromosome, float geneMutationProbability)
        {
            var chromo = (ArrayChromosome) chromosome;
            var newValues = chromo.Values
                .Select(value =>
                {
                    if (RandomEx.Global.NextDouble() < geneMutationProbability)
                    {
                        var deviation = gaussian.Next()*intensity;
                        value = value + deviation;
                    }
                    return value;
                }).ToArray();
            chromo.ReplaceGenes(0, newValues);
        }
    }
}