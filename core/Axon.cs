using System;

namespace core
{
    public class Axon
    {
        private readonly double MUTATE_MULTI;
        private const double MUTABILITY_MUTABILITY = 0.7;
        private const int MUTATE_POWER = 9;
        public readonly double weight;
        public readonly double mutability;

        public Axon(double weight, double mutability)
        {
            this.weight = weight;
            this.mutability = mutability;
            MUTATE_MULTI = Math.Pow(0.5, MUTATE_POWER);
        }

        public Axon mutateAxon()
        {
            var mutabilityMutate = Math.Pow(0.5, pmRan()*MUTABILITY_MUTABILITY);
            return new Axon(weight + r()*mutability/MUTATE_MULTI, mutability*mutabilityMutate);
        }

        private double r()
        {
            return Math.Pow(pmRan(), MUTATE_POWER);
        }

        private double pmRan()
        {
            return (Rnd.next()*2 - 1);
        }
    }
}