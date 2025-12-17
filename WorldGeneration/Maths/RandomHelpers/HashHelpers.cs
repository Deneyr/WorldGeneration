using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.Maths.RandomHelpers
{
    public static class HashHelpers
    {
        //public static ulong Hash(ulong x)
        //{
        //    x ^= x >> 30;
        //    x *= 0xBF58476D1CE4E5B9UL;
        //    x ^= x >> 27;
        //    x *= 0x94D049BB133111EBUL;
        //    x ^= x >> 31;
        //    return x;
        //}

        //public static int HashString(string str)
        //{
        //    uint hash = 0xcbf29ce4; // FNV-1a offset basis (32-bit)
        //    foreach (char c in str)
        //    {
        //        hash ^= c;
        //        hash *= 0x01000193; // FNV prime (32-bit)
        //    }
        //    return (int)hash;
        //}

        public static ulong HashString(string id)
        {
            ulong h = 0;
            foreach (char c in id)
                h = HashHelpers.Mix(h ^ c);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Mix(ulong x)
        {
            x += 0x9E3779B97F4A7C15UL;
            x = (x ^ (x >> 30)) * 0xBF58476D1CE4E5B9UL;
            x = (x ^ (x >> 27)) * 0x94D049BB133111EBUL;
            return x ^ (x >> 31);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FoldToInt(ulong seed)
        {
            seed = HashHelpers.Mix(seed);
            return unchecked((int)(seed ^ (seed >> 32)));
        }
    }
}
