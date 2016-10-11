using System;
using System.Linq;
using Neuro = AForge.Neuro;
using Genetic = AForge.Genetic;

namespace NeuralMotion.Test
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            const double scale = 1000.0;
            var random = new AForge.Math.Random.StandardGenerator();
            var inputs = Enumerable.Range(1, 20)
                .Select(a => new[] {(double) a, a + 1, a + 6})
                .ToArray();
            var outputs = inputs.Select(i => new[]
            {
                i[0]*i[1]*i[2]/scale
            }).ToArray();

            var net = new Neuro.ActivationNetwork(new Neuro.BipolarSigmoidFunction(), 3, 20, 20, 20, 1);
            var learner = new Neuro.Learning.EvolutionaryLearning(net, 100, random, random, random,
                new Genetic.EliteSelection(), 0.3, 0.1, 0.05);

            while (true)
            {
                learner.RunEpoch(inputs, outputs);

                var error = inputs.Zip(outputs, (input, target) =>
                {
                    var result = net.Compute(input);
                    var current = (target.Sum() - result.Sum())*scale;
                    return Math.Abs(current);
                }).Max();

                Console.WriteLine(error);

                if (Console.KeyAvailable)
                {
                    var info = Console.ReadKey();
                    if (info.KeyChar == ' ')
                        break;
                }
            }

            while (true)
            {
                var i1 = double.Parse(Console.ReadLine());
                var i2 = double.Parse(Console.ReadLine());
                var i3 = double.Parse(Console.ReadLine());

                var result = net.Compute(new[] {i1, i2, i3})[0]*1000.0;

                Console.WriteLine($"Result: {result}");
            }
        }
    }
}
