using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using NeuralMotion.Evolution.Neat;
using SharpNeat.Core;
using SharpNeat.Domains;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using ZedGraph;

namespace NeuralMotion.Views
{
    public partial class MultipleFunctionView : AbstractView
    {
        private readonly double[] input;
        private readonly double[] output;
        
        private LineItem[] curves;
        private LineItem targetCurve;

        public MultipleFunctionView()
        {
            InitializeComponent();

            graph.GraphPane.Legend.IsVisible = false;
            graph.GraphPane.Chart.Fill.Brush = new SolidBrush(Color.LightYellow);
            graph.GraphPane.Chart.IsRectAuto = true;
            graph.GraphPane.XAxis.MajorGrid.IsVisible = true;
            graph.GraphPane.XAxis.MinorGrid.IsVisible = true;
            graph.GraphPane.YAxis.MajorGrid.IsVisible = true;
        }

        public MultipleFunctionView(double[] input, double[] output) : this()
        {
            this.input = input;
            this.output = output;
        }

        private void EnsureCurvesCreated()
        {
            if (this.curves == null)
            {
                this.curves = new LineItem[20];
                for (var i = this.curves.Length - 1; i >= 0; i--)
                {
                    var color = Color.FromArgb(100, 0, 0, 0);
                    var symbol = SymbolType.None;
                    if (i == 0)
                        color = Color.Red;

                    this.curves[i] = graph.GraphPane.AddCurve("", input, output, color, symbol);
                    this.curves[i].Line.IsSmooth = false;
                    this.curves[i].Line.StepType = StepType.NonStep;
                    this.curves[i].Line.Style = DashStyle.Solid;
                }
            }

            if (this.targetCurve == null)
            {
                this.targetCurve = graph.GraphPane.AddCurve("", input, output, Color.Green, SymbolType.None);
                this.targetCurve.Line.IsSmooth = false;
                this.targetCurve.Line.StepType = StepType.NonStep;
            }
            else
            {
                this.targetCurve.Points = new BasicArrayPointList(input, output);
            }

            graph.GraphPane.AxisChange();
        }

        public override void RefreshView(object genome, object[] population)
        {
            this.RefreshView(genome);

            var concreteGenomes = population.OfType<IGenome>().Take(this.curves.Length - 1).ToArray();
            for (var i = 1; i < curves.Length; i++)
            {
                if (i >= concreteGenomes.Length) break;
                var curve = curves[i];
                RenderToCurve(concreteGenomes[i], curve);
            }

            this.graph.Invalidate();
        }

        public override void RefreshView(object genome)
        {
            EnsureCurvesCreated();

            var concreteGenome = genome as IGenome;
            if (concreteGenome != null)
                RenderToCurve(concreteGenome, curves[0]);
        }

        private void RenderToCurve(IGenome neat, LineItem curve)
        {
            var box = neat.CachedPhenome as IBlackBox;
            var outputs = box.EvaluateYForX(this.input);
            curve.Points = new BasicArrayPointList(this.input, outputs);
        }
    }
}
