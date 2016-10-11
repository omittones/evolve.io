using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NeuralMotion.Intelligence;
using NeuralMotion.Parallel;
using Util;

namespace NeuralMotion.Simulator
{
    public partial class BoxArena
    {
        public Ball[] EngineBalls { get; private set; }

        public float TimeStep { get; private set; }
        public float BallRadius { get; private set; }
        public bool RealTime { set; get; }
        public float SimulationDuration { get; private set; }
        public float CurrentSimulationTime { get; private set; }
        public int TotalCollisions { get; private set; }

        public float MaximumBallSpeed
        {
            get { return this.BallRadius/this.TimeStep; }
        }

        private readonly Func<Ball> ballFactory;
        private readonly IInputSelectionStrategy inputSelection;
        private readonly CollisionDetector collisionDetector;
        private Task task;

        public BoxArena(
            IInputSelectionStrategy inputSelection, 
            Func<Ball> ballFactory,
            int noBalls = 5,
            float ballRadius = 0.06f)
        {
            noBalls = Math.Max(2, noBalls);

            this.TimeStep = 0.02f;
            this.SimulationDuration = 10;
            this.RealTime = true;
            this.BallRadius = ballRadius;

            this.inputSelection = inputSelection;
            this.ballFactory = ballFactory;
            this.collisionDetector = new CollisionDetector
            {
                BallRadius = this.BallRadius
            };

            //stvori lopte i dodatne parametre koje engine koristi
            this.EngineBalls = new Ball[noBalls];
            for (var index = 0; index < EngineBalls.Length; index++)
                this.EngineBalls[index] = ballFactory();
        }

        public float RandomPosition(RandomEx generator)
        {
            return generator.NextF(-1.0f + this.BallRadius, 1.0f - this.BallRadius);
        }

        public Task RunAsync(Action<int, Ball> ballConfiguration)
        {
            if (this.task != null)
                throw new Exception("Allready started!");

            this.TotalCollisions = 0;

            for (var index = 0; index < EngineBalls.Length; index++)
            {
                EngineBalls[index].Reset();

                //stvori udaljenosti od ostalih loptica
                //zapamti pocetni polozaj
                EngineBalls[index].Distances = new float[EngineBalls.Length];
                EngineBalls[index].Speed = new PointF(0, 0);
                EngineBalls[index].Acceleration = new PointF(0, 0);

                ballConfiguration(index, EngineBalls[index]);

                EngineBalls[index].StartingPosition = EngineBalls[index].Position;
            }

            this.CurrentSimulationTime = 0;

            this.task = Task
                .Run((Action) Loop)
                .ContinueWith(previous =>
                {
                    this.CurrentSimulationTime = this.SimulationDuration;
                    this.task = null;
                });

            return this.task;
        }

        private float lastTime = -1;

        private void Loop()
        {
            try
            {
                while (this.CurrentSimulationTime < this.SimulationDuration)
                {
                    var start = DateTime.UtcNow;

                    PhysicsLoop();

                    DecisionLoop();

                    this.CurrentSimulationTime += this.TimeStep;

                    if (this.RealTime)
                    {
                        var duration = DateTime.UtcNow.Subtract(start).TotalSeconds;
                        duration = this.TimeStep - duration;
                        if (duration > 0)
                            Thread.Sleep((int) (duration*1000));
                    }
                }
            }
            catch
            {
                this.CurrentSimulationTime = SimulationDuration;
                throw;
            }
        }

        private void DecisionLoop()
        {
            if (this.lastTime > this.CurrentSimulationTime ||
                this.CurrentSimulationTime - lastTime >= 0.1)
            {
                lastTime = this.CurrentSimulationTime;

                this.EngineBalls
                    .AsParallelEx()
                    .ForAll(ball =>
                    {
                        var inputs = this.inputSelection.SelectInput(this.EngineBalls, ball);

                        var output = ball.Brain.GetOutput(inputs);

                        output = this.inputSelection.TransformOutput(output);

                        ball.Acceleration.X = output[0];
                        ball.Acceleration.Y = output[1];
                    });
            }
        }

        private void PhysicsLoop()
        {
            var detections = this.collisionDetector.Detect(this.EngineBalls, this.CurrentSimulationTime);
            this.TotalCollisions += detections;

            //pokreni svaku loptu
            //zapamti jesu li u stanju kolizije
            this.EngineBalls
                .AsParallelEx()
                .ForAll(ball =>
                {
                    //ograničenje brzine, simuliram light speed
                    var speed = ball.Speed.Offset(ball.Acceleration.Scale(this.TimeStep));
                    var speedLength = speed.Length();
                    if (speedLength < this.MaximumBallSpeed)
                        ball.Speed = speed;

                    ball.Position = ball.Position.Offset(ball.Speed.Scale(this.TimeStep));
                    ball.DistanceTravelled += speedLength*this.TimeStep;
                });
        }
    }
}
