using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using static WorldGeneration.DataChunks.VoronoiNoise.VoronoiDataChunk;

namespace WorldGeneration.DataChunks.VoronoiNoise
{
    internal class VoronoiDataCase : ICase
    {
        internal VoronoiDataPoint ParentDataPoint
        {
            get;
            set;
        }

        public Vector2i Position
        {
            get;
            private set;
        }

        public Vector2i CellCenterPosition
        {
            get
            {
                return this.ParentDataPoint.PointPosition;
            }
        }

        public virtual int Value
        {
            get
            {
                return this.ParentDataPoint.PointValue;
            }
        }

        public VoronoiDataCase(int x, int y)
        {
            this.Position = new Vector2i(x, y);
        }
    }
}