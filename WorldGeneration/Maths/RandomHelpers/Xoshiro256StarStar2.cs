using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.Maths.RandomHelpers
{
    // Xoshiro256** - Excellent compromis performance/qualité pour génération procédurale
    // Utilisé par Minecraft Java Edition et de nombreux jeux AAA
    public class Xoshiro256StarStar2
    {
        private ulong s0, s1, s2, s3;

        public Xoshiro256StarStar2(ulong seed)
        {
            // Initialisation avec SplitMix64 (évite les états faibles)
            s0 = SplitMix64(ref seed);
            s1 = SplitMix64(ref seed);
            s2 = SplitMix64(ref seed);
            s3 = SplitMix64(ref seed);
        }

        // Génère le prochain nombre aléatoire [0, ulong.MaxValue]
        public ulong NextULong()
        {
            ulong result = RotateLeft(s1 * 5, 7) * 9;
            ulong t = s1 << 17;

            s2 ^= s0;
            s3 ^= s1;
            s1 ^= s2;
            s0 ^= s3;

            s2 ^= t;
            s3 = RotateLeft(s3, 45);

            return result;
        }

        // Méthodes utilitaires compatibles System.Random
        // [0, int.MaxValue) - borne supérieure EXCLUSIVE comme Random.Next()
        public int Next() => (int)(NextULong() >> 33) & int.MaxValue;

        // [0, max) - borne supérieure EXCLUSIVE
        public int Next(int max)
        {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));
            // Utilise la partie haute de la multiplication pour éviter le biais
            ulong product = (NextULong() >> 32) * (ulong)max;
            return (int)(product >> 32);
        }

        // [min, max) - borne supérieure EXCLUSIVE
        public int Next(int min, int max)
        {
            if (min > max) throw new ArgumentOutOfRangeException(nameof(min));
            return min + Next(max - min);
        }

        public double NextDouble() => (NextULong() >> 11) * (1.0 / (1UL << 53));

        public float NextFloat() => (NextULong() >> 40) * (1.0f / (1U << 24));

        // Helpers
        private static ulong RotateLeft(ulong x, int k) => (x << k) | (x >> (64 - k));

        private static ulong SplitMix64(ref ulong x)
        {
            ulong z = (x += 0x9E3779B97F4A7C15UL);
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return z ^ (z >> 31);
        }
    }
}
