using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks;

namespace WorldGeneration.ObjectChunks
{
    public class ObjectChunkLayersMonitor
    {
        internal DataChunkLayersMonitor DataChunkMonitor
        {
            get;
            private set;
        }

        public int NbCaseSide
        {
            get;
            private set;
        }

        internal int WorldSeed
        {
            get;
            private set;
        }

        internal ObjectChunkLayersMonitor(DataChunkLayersMonitor dataChunkLayersMonitor, int nbCaseSide, int worldSeed)
        {
            this.WorldSeed = worldSeed;
            this.NbCaseSide = nbCaseSide;

            this.DataChunkMonitor = dataChunkLayersMonitor;
        }

        public IChunk GenerateChunkAt(Vector2i position)
        {
            TestChunk newChunk = new TestChunk(position, this.NbCaseSide);

            newChunk.GenerateChunk(this, null);

            return newChunk;
        }
    }
}
