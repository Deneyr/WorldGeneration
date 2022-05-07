using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.PureNoise
{
    internal class PureNoiseDataChunk : ADataChunk
    {
        public PureNoiseDataChunk(Vector2i position, int nbCaseSide, int sampleLevel):
            base(position, nbCaseSide, sampleLevel)
        {
        }

        public override void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            
        }

        protected override ICase GenerateCase(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer, int x, int y, Random random)
        {
            PureNoiseDataCase pureNoiseDataCase = new PureNoiseDataCase(x, y);

            pureNoiseDataCase.Value = (float) random.NextDouble();

            return pureNoiseDataCase;
        }        
    }
}