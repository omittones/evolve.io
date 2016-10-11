using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralMotion.Evolution
{
    public class ParameterAdjuster
    {
        private readonly int maxSampleSize;
        private readonly Queue<double> fitnessHistory;

        public ParameterAdjuster(int maxSampleSize = 10)
        {
            this.maxSampleSize = maxSampleSize;
            this.fitnessHistory = new Queue<double>();
        }

        public void RecordLastGenerationFitness(double fitness)
        {
            fitnessHistory.Enqueue(fitness);

            if (fitnessHistory.Count > maxSampleSize)
                fitnessHistory.Dequeue();
        }

        public double Average
        {
            get
            {
                if (this.fitnessHistory.Count == 0)
                    return 0.0;
                return this.fitnessHistory.Average();
            }
        }

        public double StdDev
        {
            get
            {
                if (this.fitnessHistory.Count == 0)
                    return 0.0;

                var avg = fitnessHistory.Average();

                var variance = fitnessHistory
                    .Select(fitness => avg - fitness)
                    .Select(deviation => deviation*deviation)
                    .Average();

                return Math.Sqrt(variance);
            }
        }

        public double RelativeStdDev
        {
            get
            {
                if (fitnessHistory.Count == 0)
                    return 0.0;

                return Math.Abs(this.StdDev/this.fitnessHistory.Average());
            }
        }

        public double AdjustValue(double valueForMaxDeviation, double valueForMinDeviation)
        {
            if (fitnessHistory.Count < 2)
                return valueForMaxDeviation;

            var range = valueForMinDeviation - valueForMaxDeviation;

            var relativeDeviation = Math.Abs(this.StdDev/this.Average);
            var inverseRelativeDeviation = Math.Max(1.0 - relativeDeviation, 0.0);
            inverseRelativeDeviation = Math.Pow(inverseRelativeDeviation, 13.0);

            return valueForMaxDeviation + range*inverseRelativeDeviation;
        }
    }
}