using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.WorldGenerating;

namespace WorldGeneration.DataChunks
{
    internal interface IDataChunkLayer
    {
        ChunksMonitor ChunksMonitor
        {
            get;
        }

        string Id
        {
            get;
        }

        int NbCaseSide
        {
            get;
        }

        DataChunkLayersMonitor DataChunksMonitor
        {
            set;
        }

        //bool LoadDataChunk(Vector2i chunkPosition);

        //bool UnLoadDataChunk(Vector2i chunkPosition);

        void UpdateLayerArea(IntRect newWorldArea);
    }
}
