namespace Sharparam.Minesweeper.Engine
{
    using System;

    public static class Rng
    {
        private static readonly Lazy<Random> Rand = new Lazy<Random>();

        public static int Next()
        {
            return Rand.Value.Next();
        }

        public static int Next(int max)
        {
            return Rand.Value.Next(max);
        }

        public static int Next(int min, int max)
        {
            return Rand.Value.Next(min, max);
        }

        public static double NextDouble(double min, double max)
        {
            return Rand.Value.NextDouble() * (max - min) + min;
        }
    }
}
