using System.Collections.Generic;
using System.Drawing;
using NeuralMotion.Intelligence;

namespace NeuralMotion.Simulator
{
    public class Ball
    {
        public long Id { get; set; }

        public IBrain Brain { get; set; }

        public int KicksToBorder = 0;
        public int KicksToBall = 0;
        public int KicksFromBall = 0;

        public PointF StartingPosition = new PointF(0, 0);

        public float[] Distances = null;

        public PointF Position = new PointF(0, 0);
        public PointF Speed = new PointF(0, 0);
        public PointF Acceleration = new PointF(0, 0);

        public float LastCollisionTime { get; private set; }
        public float DistanceTravelled { get; set; }

        private readonly List<int> collisions = new List<int>();
        public int CollisionCount => collisions.Count;

        public Ball(IBrain brain)
        {
            this.Brain = brain;
        }

        public void UnsetCollision(int collidedBallIndex)
        {
            collisions.Remove(collidedBallIndex);
        }

        public void SetCollision(int collidedBallIndex, float currentTime)
        {
            if (!collisions.Contains(collidedBallIndex))
                collisions.Add(collidedBallIndex);
            this.LastCollisionTime = currentTime;
        }

        public void Reset()
        {
            this.KicksFromBall = 0;
            this.KicksToBall = 0;
            this.KicksToBorder = 0;
            this.DistanceTravelled = 0;
            this.LastCollisionTime = -1;
            this.collisions.Clear();
        }
    }
}