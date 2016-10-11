using System.Linq;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace NeuralMotion.Views
{
    public partial class FitnessPlot : PlotView
    {
        private LineSeries plotFitness;
        private LineSeries plotAverage;
        
        private void EnsureInitialized()
        {
            if (this.Model == null)
            {
                this.Model = new PlotModel();
                this.plotFitness = new LineSeries
                {
                    BrokenLineColor = OxyColors.Automatic,
                    BrokenLineThickness = 1,
                    StrokeThickness = 1
                };
                this.Model.Series.Add(plotFitness);
                this.plotAverage = new LineSeries
                {
                    BrokenLineColor = OxyColors.Automatic,
                    BrokenLineThickness = 1,
                    StrokeThickness = 1
                };
                this.Model.Series.Add(plotAverage);
            }
        }

        public void AddPoint(int generation, double fitness, double average)
        {
            EnsureInitialized();

            if (this.plotFitness.Points.Count > 0 &&
                this.plotAverage.Points.Count > 0)
            {
                var lastFitness = this.plotFitness.Points.Last().Y;
                var lastAverage = this.plotAverage.Points.Last().Y;
                if (lastFitness == fitness && lastAverage == average)
                    return;
            }

            lock (this.plotFitness)
            {
                this.plotFitness.Points.Add(new DataPoint(generation, fitness));
                this.plotAverage.Points.Add(new DataPoint(generation, average));
                this.InvalidatePlot(true);
            }
        }
    }
}
