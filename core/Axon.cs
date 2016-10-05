using System;

public class Axon : Helpers
{
    private readonly double MUTABILITY_MUTABILITY = 0.7;
    private readonly int mutatePower = 9;
    private readonly double MUTATE_MULTI;

    public double weight;
    public double mutability;

    public Axon(double w, double m)
    {
        weight = w;
        mutability = m;
        MUTATE_MULTI = Math.Pow(0.5, mutatePower);
    }

    public Axon mutateAxon()
    {
        double mutabilityMutate = Math.Pow(0.5, pmRan()*MUTABILITY_MUTABILITY);
        return new Axon(weight + r()*mutability/MUTATE_MULTI, mutability*mutabilityMutate);
    }

    public double r()
    {
        return Math.Pow(pmRan(), mutatePower);
    }

    public double pmRan()
    {
        return (random()*2 - 1);
    }
}