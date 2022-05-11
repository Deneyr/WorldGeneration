using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks
{
    public interface IObjectChunkLayer
    {
        int ObjectChunkMargin
        {
            get;
        }

        string Id
        {
            get;
        }

        void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk);
    }
}
