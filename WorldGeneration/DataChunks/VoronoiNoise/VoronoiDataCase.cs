using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.VoronoiNoise
{
    internal class VoronoiDataCase : ICase
    {
        public Vector2i Position
        {
            get;
            private set;
        }

        public int Value
        {
            get;
            set;
        }

        public float BorderValue
        {
            get;
            set;
        }

        public VoronoiDataCase(int x, int y)
        {
            this.Position = new Vector2i(x, y);
            this.Value = 0;
        }
    }
}