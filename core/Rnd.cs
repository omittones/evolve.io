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

        public static float nextFloat(double min, double max)
        {
            return (float) (randomGenerator.NextDouble()*(max - min) + min);
        }

        public static int nextInt(int min, int max)
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
            return Perlin.Noise(x*2 + 7, y*2 + 7) +
                   Perlin.Noise(x + 100, y + 100);
        }

        public static void noiseSeed(int seed)
        {
        }
    }
}