using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.DataChunks.VoronoiNoise.BiomeVoronoiNoise
{
    internal class BiomeVoronoiDataCase : VoronoiDataCase
    {
        public float BorderValue
        {
            get;
            set;
        }

        public BiomeVoronoiDataCase(int x, int y)
            : base(x, y)
        {
            this.BorderValue = 1;
        }        
    }
}
