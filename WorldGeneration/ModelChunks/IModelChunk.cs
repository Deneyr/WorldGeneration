using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.ModelChunks
{
    public interface IModelChunk : IChunk
    {
        bool GenerateChunk(ModelChunkLayersMonitor dataChunksMonitor, IModelChunkLayer parentLayer);
    }
}
