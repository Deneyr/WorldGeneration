using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks
{
    internal interface IObjectChunkLayer
    {
        int ObjectChunkMargin
        {
            get;
        }

        string Id
        {
            get;
        }

        void InitObjectChunkLayer(int nbCaseSide);

        void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk);
    }
}
