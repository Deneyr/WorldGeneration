using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.Maths.RandomHelpers
{
    public sealed class Xoshiro256StarStarRandom: Random
    {
        // 4x64-bit state
        private ulong s0, s1, s2, s3;

        // Constructors -----------------------------------------------------------------

        /// <summary>Creates an instance seeded from a default time-based seed.</summary>
        public Xoshiro256StarStarRandom() : this(DefaultSeed()) { }

        /// <summary>Creates an instance seeded from a 32-bit int (System.Random compatibility).</summary>
        public Xoshiro256StarStarRandom(int seed) : this((ulong)(uint)seed) { }

        /// <summary>Creates an instance seeded from a 64-bit seed (recommended).</summary>
        public Xoshiro256StarStarRandom(ulong seed)
        {
            SeedFromUlong(seed);
        }

        // Seeding helpers --------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong DefaultSeed()
        {
            // Non-cryptographic default seed combining time and environment.
            // Good enough for default use; provide explicit seed for reproducibility.
            unchecked
            {
                ulong t = (ulong)DateTime.UtcNow.Ticks;
#if NETSTANDARD2_0 || NETFRAMEWORK
            // Environment.TickCount is int; mix in to add some entropy
            t ^= (ulong)(uint)Environment.TickCount;
#else
                t ^= (ulong)Environment.TickCount;
#endif
                return SplitMix64(ref t);
            }
        }

        /// <summary>Seed the internal state using SplitMix64 to expand a 64-bit seed into four 64-bit words.</summary>
        private void SeedFromUlong(ulong seed)
        {
            // Use local copy for SplitMix64 generation
            ulong x = seed;
            s0 = SplitMix64(ref x);
            s1 = SplitMix64(ref x);
            s2 = SplitMix64(ref x);
            s3 = SplitMix64(ref x);

            // xoshiro authors recommend non-zero state; if all zero, tweak slightly
            if ((s0 | s1 | s2 | s3) == 0UL)
            {
                s0 = 0x9E3779B97F4A7C15UL;
                s1 = 0xBF58476D1CE4E5B9UL;
                s2 = 0x94D049BB133111EBUL;
                s3 = 0xD6E8FEB86659FD93UL;
            }
        }

        // SplitMix64: used for seeding -----------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong SplitMix64(ref ulong x)
        {
            // x is mutated in-place to advance the stream (standard usage when expanding a seed)
            x += 0x9E3779B97F4A7C15UL;
            ulong z = x;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return z ^ (z >> 31);
        }

        // xoshiro256** core ---------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong Rotl(ulong x, int k) => (x << k) | (x >> (64 - k));

        /// <summary>Generates the next 64-bit output and advances internal state.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong NextUInt64()
        {
            // xoshiro256**: result = rotl(s1 * 5, 7) * 9
            ulong result = Rotl(s1 * 5UL, 7) * 9UL;

            ulong t = s1 << 17;

            s2 ^= s0;
            s3 ^= s1;
            s1 ^= s2;
            s0 ^= s3;
            s2 ^= t;
            s3 = Rotl(s3, 45);

            return result;
        }

        /// <summary>Generates a 32-bit unsigned integer from the generator.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint NextUInt32() => (uint)(NextUInt64() >> 32);

        // Jump functions (optional) --------------------------------------------------

        /// <summary>
        /// Equivalent to 2^128 calls to Next; can be used to create non-overlapping subsequences for parallel generation.
        /// </summary>
        public void Jump()
        {
            // jump constants for xoshiro256**
            ulong[] JUMP = new ulong[] {
            0x180ec6d33cfd0abaUL,
            0xd5a61266f0c9392cUL,
            0xa9582618e03fc9aaUL,
            0x39abdc4529b1661cUL
        };

            ulong s0j = 0, s1j = 0, s2j = 0, s3j = 0;
            for (int i = 0; i < JUMP.Length; i++)
            {
                ulong bits = JUMP[i];
                for (int b = 0; b < 64; b++)
                {
                    if ((bits & 1UL) != 0UL)
                    {
                        s0j ^= s0;
                        s1j ^= s1;
                        s2j ^= s2;
                        s3j ^= s3;
                    }
                    NextUInt64();
                    bits >>= 1;
                }
            }
            s0 = s0j; s1 = s1j; s2 = s2j; s3 = s3j;
        }

        /// <summary>Equivalent to 2^192 calls to Next; further distant subsequence.</summary>
        public void LongJump()
        {
            ulong[] LONG_JUMP = new ulong[] {
            0x76e15d3efefdcbbfUL,
            0xc5004e441c522fb3UL,
            0x77710069854ee241UL,
            0x39109bb02acbe635UL
        };

            ulong s0j = 0, s1j = 0, s2j = 0, s3j = 0;
            for (int i = 0; i < LONG_JUMP.Length; i++)
            {
                ulong bits = LONG_JUMP[i];
                for (int b = 0; b < 64; b++)
                {
                    if ((bits & 1UL) != 0UL)
                    {
                        s0j ^= s0;
                        s1j ^= s1;
                        s2j ^= s2;
                        s3j ^= s3;
                    }
                    NextUInt64();
                    bits >>= 1;
                }
            }
            s0 = s0j; s1 = s1j; s2 = s2j; s3 = s3j;
        }

        // System.Random interoperability -------------------------------------------

        /// <summary>
        /// Sample must return a double in [0,1). We produce a 53-bit resolution double using top bits.
        /// Overriding Sample makes Next(), NextDouble(), etc. from Random work.
        /// </summary>
        protected override double Sample()
        {
            // take top 53 bits from a 64-bit value
            // shift right by 11 -> 53-bit value in range [0, 2^53)
            // divide by 2^53 to get [0,1)
            const double invDoubleScale = 1.0 / (1UL << 53); // 1/2^53
            ulong v = NextUInt64();
            ulong top53 = v >> 11;
            return top53 * invDoubleScale;
        }

        /// <summary>Fills buffer with random bytes.</summary>
        public override void NextBytes(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            int i = 0;
            int len = buffer.Length;
            while (len >= 8)
            {
                ulong v = NextUInt64();
                buffer[i++] = (byte)v;
                buffer[i++] = (byte)(v >> 8);
                buffer[i++] = (byte)(v >> 16);
                buffer[i++] = (byte)(v >> 24);
                buffer[i++] = (byte)(v >> 32);
                buffer[i++] = (byte)(v >> 40);
                buffer[i++] = (byte)(v >> 48);
                buffer[i++] = (byte)(v >> 56);
                len -= 8;
            }
            if (len > 0)
            {
                ulong v = NextUInt64();
                for (int j = 0; j < len; j++)
                {
                    buffer[i++] = (byte)(v >> (j * 8));
                }
            }
        }

        // Overloads for convenience -------------------------------------------------

        /// <summary>Returns an unsigned 32-bit integer in [0, uint.MaxValue]</summary>
        public uint NextUInt()
        {
            return NextUInt32();
        }

        /// <summary>Returns a signed 32-bit int in [0, Int32.MaxValue)</summary>
        public override int Next()
        {
            // Next() should return value in [0, Int32.MaxValue)
            // We'll produce 31 bits.
            return (int)(NextUInt32() & 0x7FFFFFFF);
        }

        /// <summary>Returns a non-negative random integer less than maxValue (no modulo bias).</summary>
        public override int Next(int maxValue)
        {
            if (maxValue <= 0) throw new ArgumentOutOfRangeException(nameof(maxValue));
            // Rejection sampling on 31-bit positive range
            uint bound = (uint)maxValue;
            uint threshold = (uint)(uint.MaxValue % bound);
            while (true)
            {
                uint r = NextUInt32();
                if (r >= threshold)
                {
                    return (int)(r % bound);
                }
            }
        }

        /// <summary>Returns a random integer in [minValue, maxValue)</summary>
        public override int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue));
            long range = (long)maxValue - minValue;
            if (range <= int.MaxValue)
            {
                return minValue + Next((int)range);
            }
            // If range is larger than int.MaxValue, use 64-bit sampling
            return (int)(minValue + (long)(NextUInt64() % (ulong)range));
        }

        /// <summary>Returns a double in [0,1)</summary>
        public override double NextDouble()
        {
            return Sample();
        }

        // Utilities ------------------------------------------------------------------

        /// <summary>Return the internal state as an array (useful for debugging or saving state).</summary>
        public ulong[] GetState()
        {
            return new ulong[] { s0, s1, s2, s3 };
        }

        /// <summary>Set the internal state directly (use carefully).</summary>
        public void SetState(ulong[] state)
        {
            if (state == null || state.Length != 4) throw new ArgumentException("state must be length 4");
            s0 = state[0]; s1 = state[1]; s2 = state[2]; s3 = state[3];
        }
    }
}
