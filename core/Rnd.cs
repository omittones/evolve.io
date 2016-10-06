using System;

namespace core
{
    public static class Rnd
    {
        private static Random randomGenerator;

        static Rnd()
        {
            randomGenerator = new Random();
        }

        public static float next(double min, double max)
        {
            return (float) (randomGenerator.NextDouble()*(max - min) + min);
        }

        public static int next(int min, int max)
        {
            return randomGenerator.Next(min, max);
        }

        public static float next()
        {
            return (float) randomGenerator.NextDouble();
        }

        public static void randomSeed(int seed)
        {
            randomGenerator = new Random(seed);
        }

        public static float noise(float x, float y)
        {
            return Perlin.Noise(x, y);
        }

        public static float noise(double x, double y)
        {
            return Perlin.Noise((float) x, (float) y);
        }

        public static void noiseSeed(int seed)
        {
        }
    }
}