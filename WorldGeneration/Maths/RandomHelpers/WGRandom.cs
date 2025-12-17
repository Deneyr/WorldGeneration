using MathNet.Numerics.Random;
using Redzen.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.Maths.RandomHelpers
{
    public class WGRandom: Xoshiro256StarStar
    {
        public WGRandom(ulong seed) : base(HashHelpers.FoldToInt(seed)) { }
    }
}
