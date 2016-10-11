using System;
using System.Collections.Generic;
using core.Graphics;

namespace core
{
    public class SoftBody
    {
        public const float ENERGY_DENSITY = (float) (1.0/(0.2*0.2*Math.PI));

        public HSBColor myColor;

        public double px;
        public double py;
        public double vx;
        public double vy;
        public double energy;
        public double density;
        public double birthTime;
        public bool isCreature = false;
        public readonly float FRICTION = 0.03f;
        public readonly float COLLISION_FORCE = 0.1f;
        public readonly float FIGHT_RANGE = 2.0f;
        public double fightLevel;

        private int prevSBIPMinX;
        private int prevSBIPMinY;
        private int prevSBIPMaxX;
        private int prevSBIPMaxY;
        public int SBIPMinX;
        public int SBIPMinY;
        public int SBIPMaxX;
        public int SBIPMaxY;
        public List<SoftBody> colliders;
        public Board board;


        protected GraphicsEngine graphics;

        public SoftBody(
            GraphicsEngine graphics,
            double tpx, double tpy, double tvx, double tvy, double tenergy, double tdensity,
            HSBColor color, Board tb, double bt)
        {
            this.graphics = graphics;

            px = tpx;
            py = tpy;
            vx = tvx;
            vy = tvy;
            energy = tenergy;
            density = tdensity;
            myColor = color;
            board = tb;
            setSBIP(false);
            setSBIP(false); // just to set previous SBIPs as well.
            birthTime = bt;
        }

        public void setSBIP(bool shouldRemove)
        {
            var radius = getRadius()*FIGHT_RANGE;
            prevSBIPMinX = SBIPMinX;
            prevSBIPMinY = SBIPMinY;
            prevSBIPMaxX = SBIPMaxX;
            prevSBIPMaxY = SBIPMaxY;
            SBIPMinX = xBound((int) (Math.Floor(px - radius)));
            SBIPMinY = yBound((int) (Math.Floor(py - radius)));
            SBIPMaxX = xBound((int) (Math.Floor(px + radius)));
            SBIPMaxY = yBound((int) (Math.Floor(py + radius)));
            if (prevSBIPMinX != SBIPMinX || prevSBIPMinY != SBIPMinY ||
                prevSBIPMaxX != SBIPMaxX || prevSBIPMaxY != SBIPMaxY)
            {
                if (shouldRemove)
                {
                    for (var x = prevSBIPMinX; x <= prevSBIPMaxX; x++)
                    {
                        for (var y = prevSBIPMinY; y <= prevSBIPMaxY; y++)
                        {
                            if (x < SBIPMinX || x > SBIPMaxX ||
                                y < SBIPMinY || y > SBIPMaxY)
                            {
                                board.softBodiesInPositions[x, y].Remove(this);
                            }
                        }
                    }
                }
                for (var x = SBIPMinX; x <= SBIPMaxX; x++)
                {
                    for (var y = SBIPMinY; y <= SBIPMaxY; y++)
                    {
                        if (x < prevSBIPMinX || x > prevSBIPMaxX ||
                            y < prevSBIPMinY || y > prevSBIPMaxY)
                        {
                            board.softBodiesInPositions[x, y].Add(this);
                        }
                    }
                }
            }
        }

        public int xBound(int x)
        {
            return Math.Min(Math.Max(x, 0), board.boardWidth - 1);
        }

        public int yBound(int y)
        {
            return Math.Min(Math.Max(y, 0), board.boardHeight - 1);
        }

        public double xBodyBound(double x)
        {
            var radius = getRadius();
            return Math.Min(Math.Max(x, radius), board.boardWidth - radius);
        }

        public double yBodyBound(double y)
        {
            var radius = getRadius();
            return Math.Min(Math.Max(y, radius), board.boardHeight - radius);
        }

        public void collide(double timeStep)
        {
            colliders = new List<SoftBody>(0);
            for (var x = SBIPMinX; x <= SBIPMaxX; x++)
            {
                for (var y = SBIPMinY; y <= SBIPMaxY; y++)
                {
                    for (var i = 0; i < board.softBodiesInPositions[x, y].Count; i++)
                    {
                        var newCollider = board.softBodiesInPositions[x, y][i];
                        if (!colliders.Contains(newCollider) && newCollider != this)
                        {
                            colliders.Add(newCollider);
                        }
                    }
                }
            }

            for (var i = 0; i < colliders.Count; i++)
            {
                var collider = colliders[i];
                var distance = MathEx.Distance((float) px, (float) py, (float) collider.px, (float) collider.py);
                var combinedRadius = getRadius() + collider.getRadius();
                if (distance < combinedRadius && Math.Abs(distance) > 0)
                {
                    var force = combinedRadius*COLLISION_FORCE;
                    vx += ((px - collider.px)/distance)*force/getMass();
                    vy += ((py - collider.py)/distance)*force/getMass();

                    if (double.IsNaN(vx) || double.IsNaN(vy))
                        throw new ApplicationException("Invalid values!");
                }
            }

            fightLevel = 0;
        }

        public virtual void applyMotions(double timeStep)
        {
            px = xBodyBound(px + vx*timeStep);
            py = yBodyBound(py + vy*timeStep);
            vx *= (1 - FRICTION/getMass());
            vy *= (1 - FRICTION/getMass());
            setSBIP(true);

            if (double.IsNaN(px) || double.IsNaN(py))
                throw new ApplicationException("Invalid values!");
        }

        public void drawSoftBody(float scaleUp)
        {
            var radius = getRadius();
            this.graphics.stroke(0, 0, 0);
            this.graphics.strokeWeight(2);
            this.graphics.fill(this.myColor);
            this.graphics.ellipse((float) (px*scaleUp), (float) (py*scaleUp), (float) (radius*scaleUp), (float) (radius*scaleUp));
        }

        public double getRadius()
        {
            if (energy <= 0)
                return 0;
            return Math.Sqrt(energy/ENERGY_DENSITY/Math.PI);
        }

        protected double getMass()
        {
            return energy/ENERGY_DENSITY*density;
        }
    }
}