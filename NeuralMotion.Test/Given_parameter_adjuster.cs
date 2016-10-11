using NeuralMotion.Evolution;
using Xunit;

namespace NeuralMotion.Test
{
    public class Given_parameter_adjuster
    {
        private readonly ParameterAdjuster adjuster;

        public Given_parameter_adjuster()
        {
            this.adjuster = new ParameterAdjuster();
        }

        [Fact]
        public void No_samples_return_normal()
        {
            Assert.Equal(0, this.adjuster.Average);
            Assert.Equal(0, this.adjuster.StdDev);
            Assert.Equal(0, this.adjuster.RelativeStdDev);
            Assert.Equal(0.1, this.adjuster.AdjustValue(0.1, 1.0));
        }

        [Fact]
        public void Reverse_adjustment_works()
        {
            this.adjuster.RecordLastGenerationFitness(1.0);
            this.adjuster.RecordLastGenerationFitness(1.0);

            var param = this.adjuster.AdjustValue(1.0, 0.1);
            Assert.True(param >= 0.098);

            this.adjuster.RecordLastGenerationFitness(10.0);
            this.adjuster.RecordLastGenerationFitness(40.0);

            param = this.adjuster.AdjustValue(1.0, 0.1);
            Assert.Equal(1, param);
        }

        [Fact]
        public void Single_sample_returns_min_adjustment()
        {
            this.adjuster.RecordLastGenerationFitness(0.0);

            Assert.Equal(0.1, this.adjuster.AdjustValue(0.1, 1.0));
        }

        [Fact]
        public void Big_deviation_returns_min_adjustment()
        {
            this.adjuster.RecordLastGenerationFitness(0.0);
            this.adjuster.RecordLastGenerationFitness(5.0);
            this.adjuster.RecordLastGenerationFitness(-6.0);
            this.adjuster.RecordLastGenerationFitness(3.0);

            var param = this.adjuster.AdjustValue(0.1, 1.0);

            Assert.True(0.1 <= param && 0.12 >= param);
        }

        [Fact]
        public void Medium_deviation_returns_med_adjustment()
        {
            this.adjuster.RecordLastGenerationFitness(1.0);
            this.adjuster.RecordLastGenerationFitness(0.8);
            this.adjuster.RecordLastGenerationFitness(1.2);
            this.adjuster.RecordLastGenerationFitness(0.9);

            var param = this.adjuster.AdjustValue(0.1, 1.0);

            Assert.True(0.15 <= param && 0.25 >= param);

            for (var i = 0; i < 20; i++)
            {
                this.adjuster.RecordLastGenerationFitness(103);
                this.adjuster.RecordLastGenerationFitness(83);
                this.adjuster.RecordLastGenerationFitness(125);
                this.adjuster.RecordLastGenerationFitness(94);
            }

            param = this.adjuster.AdjustValue(0.1, 1.0);

            Assert.True(0.15 <= param && 0.25 >= param);
        }

        [Fact]
        public void Small_deviation_returns_max_adjustment()
        {
            this.adjuster.RecordLastGenerationFitness(211.0);
            this.adjuster.RecordLastGenerationFitness(210.0);
            this.adjuster.RecordLastGenerationFitness(212.0);
            this.adjuster.RecordLastGenerationFitness(213.0);

            var param = this.adjuster.AdjustValue(0.1, 1.0);

            Assert.True(0.9 <= param && 1.0 >= param);
        }
    }
}