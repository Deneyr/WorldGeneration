﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.ObjectChunks
{
    public interface IObjectChunk : IChunk
    {
        bool GenerateChunk(ObjectChunkLayersMonitor dataChunksMonitor, IObjectChunkLayer parentLayer);
    }
}
