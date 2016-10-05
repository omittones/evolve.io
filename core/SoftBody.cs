using System;
using System.Collections.Generic;

public class SoftBody : Helpers
{
    public double px;
    public double py;
    public double vx;
    public double vy;
    public double energy;

    private readonly float ENERGY_DENSITY = (float) (1.0/(0.2*0.2*Math.PI));
    //set so when a creature is of minimum size, it equals one.

    public double density;
    public double hue;
    public double saturation;
    public double brightness;
    public double birthTime;
    public bool isCreature = false;
    public readonly float FRICTION = 0.03f;
    public readonly float COLLISION_FORCE = 0.1f;
    public readonly float FIGHT_RANGE = 2.0f;
    public double fightLevel = 0;

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


    public SoftBody(double tpx, double tpy, double tvx, double tvy, double tenergy, double tdensity,
        double thue, double tsaturation, double tbrightness, Board tb, double bt)
    {
        px = tpx;
        py = tpy;
        vx = tvx;
        vy = tvy;
        energy = tenergy;
        density = tdensity;
        hue = thue;
        saturation = tsaturation;
        brightness = tbrightness;
        board = tb;
        setSBIP(false);
        setSBIP(false); // just to set previous SBIPs as well.
        birthTime = bt;
    }

    public void setSBIP(bool shouldRemove)
    {
        double radius = getRadius()*FIGHT_RANGE;
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
                for (int x = prevSBIPMinX; x <= prevSBIPMaxX; x++)
                {
                    for (int y = prevSBIPMinY; y <= prevSBIPMaxY; y++)
                    {
                        if (x < SBIPMinX || x > SBIPMaxX ||
                            y < SBIPMinY || y > SBIPMaxY)
                        {
                            board.softBodiesInPositions[x, y].Remove(this);
                        }
                    }
                }
            }
            for (int x = SBIPMinX; x <= SBIPMaxX; x++)
            {
                for (int y = SBIPMinY; y <= SBIPMaxY; y++)
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
        double radius = getRadius();
        return Math.Min(Math.Max(x, radius), board.boardWidth - radius);
    }

    public double yBodyBound(double y)
    {
        double radius = getRadius();
        return Math.Min(Math.Max(y, radius), board.boardHeight - radius);
    }

    public void collide(double timeStep)
    {
        colliders = new List<SoftBody>(0);
        for (int x = SBIPMinX; x <= SBIPMaxX; x++)
        {
            for (int y = SBIPMinY; y <= SBIPMaxY; y++)
            {
                for (int i = 0; i < board.softBodiesInPositions[x, y].Count; i++)
                {
                    SoftBody newCollider = (SoftBody) board.softBodiesInPositions[x, y][i];
                    if (!colliders.Contains(newCollider) && newCollider != this)
                    {
                        colliders.Add(newCollider);
                    }
                }
            }
        }
        for (int i = 0; i < colliders.Count; i++)
        {
            SoftBody collider = colliders[i];
            float distance = dist((float) px, (float) py, (float) collider.px, (float) collider.py);
            double combinedRadius = getRadius() + collider.getRadius();
            if (distance < combinedRadius)
            {
                double force = combinedRadius*COLLISION_FORCE;
                vx += ((px - collider.px)/distance)*force/getMass();
                vy += ((py - collider.py)/distance)*force/getMass();
            }
        }
        fightLevel = 0;
    }

    public void applyMotions(double timeStep)
    {
        px = xBodyBound(px + vx*timeStep);
        py = yBodyBound(py + vy*timeStep);
        vx *= (1 - FRICTION/getMass());
        vy *= (1 - FRICTION/getMass());
        setSBIP(true);
    }

    public void drawSoftBody(float scaleUp)
    {
        double radius = getRadius();
        stroke(0);
        strokeWeight(2);
        fill((float) hue, (float) saturation, (float) brightness);
        this.ellipseMode(EllipseMode.RADIUS);
        this.ellipse((float) (px*scaleUp), (float) (py*scaleUp), (float) (radius*scaleUp), (float) (radius*scaleUp));
    }

    public double getRadius()
    {
        if (energy <= 0)
        {
            return 0;
        }
        else
        {
            return Math.Sqrt(energy/ENERGY_DENSITY/Math.PI);
        }
    }

    public double getMass()
    {
        return energy/ENERGY_DENSITY*density;
    }
}