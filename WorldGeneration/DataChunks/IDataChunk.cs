using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.WorldGenerating;

namespace WorldGeneration.DataChunks
{
    internal interface IDataChunk : IChunk
    {
        void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer);

        void GenerateChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer);
    }
}
