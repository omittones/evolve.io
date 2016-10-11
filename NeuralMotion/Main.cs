using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NeuralMotion.Evolution;
using NeuralMotion.Evolution.GeneticSharp;
using NeuralMotion.Intelligence;
using NeuralMotion.Simulator;
using Util;

namespace NeuralMotion
{
    public partial class Main : Form
    {
        private static readonly HiResTimer hiResTimer = new HiResTimer();
        
        private bool closing;
        
        private readonly Settings uiSettings;
        private readonly DetermineFitnessBySimulation fitnessFunction;
        private readonly IInputSelectionStrategy selection;
        private readonly ParameterAdjuster adjuster;
        private readonly IChromosomeEvolver evolver;
        private int lastImprovementGeneration;

        public BoxArena BoxArena { get; private set; }

        public Main()
        {
            Func<Ball> ballFactory = () => new Ball(CreateBrain());

            this.lastImprovementGeneration = 0;
            this.adjuster = new ParameterAdjuster();
            this.selection = new SelectClosestAndDistancesInPolar();
            this.BoxArena = new BoxArena(selection, ballFactory, 10, 0.06f);
            this.fitnessFunction = new DetermineFitnessBySimulation(this.BoxArena);

            this.evolver = new Evolver(100, this.fitnessFunction, GeneCount());
            //this.evolver = new SimulatedAnnealing(adam, this.fitnessFunction);
            //this.evolver = new BruteForceSearch(adam, this.fitnessFunction, -1, 1, 0.5);

            InitializePopulation();

            InitializeComponent();

            this.uiSettings = new Settings();
            this.uiArena.Arena = this.BoxArena;
            this.BoxArena.RealTime = false;

            refreshTimer.Interval = 1000/60;
        }

        private void InitializePopulation()
        {
            var initialValues = CreateBrain()
                .Select(d => (double) d)
                .ToArray();
            var population = new double[evolver.Size][];
            population.SetAll(initialValues);
            evolver.SetPopulation(population);
        }

        private IBrain CreateBrain()
        {
            return new ActivationNetworkBrain(selection.InputLength, 10, 2);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.closing = true;
            base.OnClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.SetClientSizeCore(550, 700);

            uiSettings.Show(this);
            uiSettings.SetDesktopLocation(1000, 100);
            uiSettings.FormClosed += (sender, args) =>
            {
                if (!this.closing)
                    this.Close();
            };

            ConsoleWindow.Show();

            Console.WriteLine("Loaded...");

            hiResTimer.Reset();

            this.refreshTimer.Start();
            
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        this.evolver.PerformSingleStep();

                        this.ShowInfo();

                        this.AdaptToFitnessChange(this.evolver.BestFitness);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }

        private void AdaptToFitnessChange(double lastGenerationFitness)
        {
            this.adjuster.RecordLastGenerationFitness(lastGenerationFitness);

            this.evolver.Adjust(this.adjuster);

            if (uiSettings.EnableNetExpansion)
            {
                ImproveBrainsWhenConverged();
            }
        }

        private void ImproveBrainsWhenConverged()
        {
            if (this.adjuster.RelativeStdDev < 0.2)
            {
                if (this.evolver.CurrentGeneration - this.lastImprovementGeneration > 10)
                {
                    ImproveBrainsAndResetEvolver(2);
                    this.lastImprovementGeneration = this.evolver.CurrentGeneration;
                }
            }
            else
            {
                this.lastImprovementGeneration = this.evolver.CurrentGeneration;
            }
        }

        private void ImproveBrainsAndResetEvolver(int expandFactor)
        {
            Console.WriteLine("Expanding neural net...");

            var brain = this.BoxArena.EngineBalls.First().Brain;
            var populus = ChromosomeEvolverExtensions.GetPopulation(this.evolver);
            for (var i = 0; i < populus.Length; i++)
            {
                brain.LoadBrain(populus[i]);
                var expandedBrain = brain.ExpandGenome(expandFactor);
                populus[i] = expandedBrain.Select(g => (double) g).ToArray();
            }

            Array.ForEach(this.BoxArena.EngineBalls, b =>
            {
                b.Brain = brain.ExpandGenome(expandFactor);
            });

            this.evolver.SetPopulation(populus);

            var newGeneCount = this.BoxArena.EngineBalls.Max(b => b.Brain.NumberOfGenes);

            Console.WriteLine($"Expanded from {brain.NumberOfGenes} to {newGeneCount} genes.");
            Console.WriteLine();
        }

        private int GeneCount()
        {
            return this.BoxArena.EngineBalls.Max(ball => ball.Brain.NumberOfGenes);
        }
        
        private void ShowInfo()
        {
            var bestBall = this.fitnessFunction.BestBall.Ball;

            this.uiFitnessPlot.AddPoint(this.evolver.CurrentGeneration, this.evolver.BestFitness, this.adjuster.Average);

            if (this.uiSettings.WriteStatus)
            {
                Console.WriteLine("{0} generation done ({1} units)...", this.evolver.CurrentGeneration, this.evolver.Size);
                Console.WriteLine("Max possible speed: {0}", this.BoxArena.MaximumBallSpeed);
                Console.WriteLine("        Gene count: {0}", this.evolver.CurrentChampGenome.Length);

                var lines = this.evolver.StatusReport().Select(l =>
                {
                    var totalLength = Math.Max(0, 18 - l.IndexOf(':')) + l.Length;
                    return l.PadLeft(totalLength, ' ');
                }).ToArray();
                foreach (var line in lines)
                    Console.WriteLine(line);

                Console.WriteLine();
                Console.WriteLine("Best ball");
                Console.WriteLine("      Kicks To Ball: {0}", bestBall.KicksToBall);
                Console.WriteLine("      Kicks From Ball: {0}", bestBall.KicksFromBall);
                Console.WriteLine("      Kicks To Border: {0}", bestBall.KicksToBorder);
                Console.WriteLine("      Distance Travelled: {0}", bestBall.DistanceTravelled);
                Console.WriteLine("      Speed: {0}", bestBall.Speed.Distance(0, 0));
                Console.WriteLine("Total Collisions: " + this.BoxArena.TotalCollisions);
                Console.WriteLine("Best Fitness: {0}", this.evolver.BestFitness);
                Console.WriteLine();
                Console.WriteLine();
            }

            if (!this.uiSettings.DontPreviewBest)
            {
                this.uiArena.ShowPreviewFlag = true;

                if (!this.BoxArena.RealTime)
                    this.BoxArena.RealTime = true;

                this.fitnessFunction.Evaluate(this.evolver.CurrentChampGenome);

                if (this.BoxArena.RealTime)
                    this.BoxArena.RealTime = false;

                this.uiArena.ShowPreviewFlag = false;
            }

            Main.hiResTimer.Reset();
        }

        private void OnRefreshTimer(object sender, EventArgs e)
        {
            if (!this.uiSettings.DontShowSim)
            {
                this.uiArena.Refresh();
            }
        }
    }
}