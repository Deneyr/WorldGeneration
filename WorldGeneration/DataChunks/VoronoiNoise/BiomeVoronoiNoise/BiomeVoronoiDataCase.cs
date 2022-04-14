using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WorldGeneration.DataChunks.VoronoiNoise.BiomeVoronoiNoise.BiomeVoronoiDataChunk;

namespace WorldGeneration.DataChunks.VoronoiNoise.BiomeVoronoiNoise
{
    internal class BiomeVoronoiDataCase : VoronoiDataCase
    {
        public float BorderValue
        {
            get;
            set;
        }

        public virtual int RiverValue
        {
            get
            {
                return (this.ParentDataPoint as BiomeVoronoiDataPoint).RiverPointValue;
            }
        }

        public float RiverBorderValue
        {
            get;
            set;
        }

        public BiomeVoronoiDataCase(int x, int y)
            : base(x, y)
        {
            this.BorderValue = 1;
            this.RiverBorderValue = float.MaxValue;
        }        
    }
}
