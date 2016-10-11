using System;
using System.Drawing;
using System.Linq;
using NeuralMotion.Simulator;
using Util;

namespace NeuralMotion.Evolution
{
    public class HandcraftedEvolver
    {
        private readonly Ball[] allBalls;
        private readonly float maxPlacementValue;

        private float[][] bestGeneticCode;
        private readonly PointF[] savedPositions;
        private readonly PointF[] savedSpeeds;

        public HandcraftedEvolver(Ball[] allBalls, float maxPlacementValue)
        {
            this.allBalls = allBalls;
            this.maxPlacementValue = maxPlacementValue;
            this.savedPositions = new PointF[this.allBalls.Length];
            this.savedSpeeds = new PointF[this.allBalls.Length];
        }

        public static float Fitness(Ball ball)
        {
            return -(float) ball.Position.Distance(ball.StartingPosition);
        }

        public Ball[] DetermineSurvivors(int count)
        {
            //sortiraj kugle po fittnessu           
            //veæi fittness je bolji fitness
            var bestBalls = this.allBalls
                .OrderByDescending(Fitness)
                .Take(count)
                .ToArray();

            if (bestGeneticCode == null)
                bestGeneticCode = new float[bestBalls.Length][];
            
            //skupi najbolje mozgove po Fittness vrijednosti
            for (var ballIndex = 0; ballIndex < bestBalls.Length; ballIndex++)
            {
                if (bestGeneticCode[ballIndex] == null)
                {
                    var numberOfGenes = allBalls.Max(e => e.Brain.NumberOfGenes);
                    bestGeneticCode[ballIndex] = new float[numberOfGenes];
                }

                var ball = bestBalls[ballIndex];
                for (var geneIndex = 0; geneIndex < ball.Brain.NumberOfGenes; geneIndex++)
                    bestGeneticCode[ballIndex][geneIndex] = ball.Brain[geneIndex];
            }

            return bestBalls;
        }

        public void MakeBetterBall(int index, Ball ball)
        {
            ball.Position = this.savedPositions[index];
            ball.Speed = this.savedSpeeds[index];
            ball.Acceleration = new PointF(0, 0);

            //svaka sljedeæa je mješavina dva roditelja
            var first = RandomEx.Global.Next(5);
            var second = RandomEx.Global.Next(5);
            for (var c = 0; c < ball.Brain.NumberOfGenes; c++)
            {
                if (RandomEx.Global.NextDouble() > 0.5)
                    ball.Brain[c] = bestGeneticCode[first][c];
                else
                    ball.Brain[c] = bestGeneticCode[second][c];
            }

            var geneMutation = Math.Max(0.1f, bestGeneticCode.Max(e => e.Max())/10);

            //plus malo mutacija
            var noMutations = RandomEx.Global.Next(ball.Brain.NumberOfGenes/8);
            for (var mutation = 0; mutation < noMutations; mutation++)
            {
                var randomGene = RandomEx.Global.Next(ball.Brain.NumberOfGenes);
                ball.Brain[randomGene] += geneMutation;
            }
        }

        private void PlaceBall(Ball ball)
        {
            ball.Position.X = (float) RandomEx.Global.GetUniformChange(0, true)*this.maxPlacementValue;
            ball.Position.Y = (float) RandomEx.Global.GetUniformChange(0, true)*this.maxPlacementValue;
            ball.Speed.X = 0;
            ball.Speed.Y = 0;
            ball.Acceleration.X = 0;
            ball.Acceleration.Y = 0;
        }

        public void MakeRandomBall(int index, Ball ball)
        {
            PlaceBall(ball);

            this.savedPositions[index] = ball.Position;
            this.savedSpeeds[index] = ball.Speed;

            for (var cGene = 0; cGene < ball.Brain.NumberOfGenes; cGene++)
                ball.Brain[cGene] = (float) RandomEx.Global.GetUniformChange(0, true);
        }
    }
}