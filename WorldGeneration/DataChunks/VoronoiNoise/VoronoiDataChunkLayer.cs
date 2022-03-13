using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.VoronoiNoise
{
    internal class VoronoiDataChunkLayer : AExtendedDataChunkLayer
    {
        public int NbPointsInside
        {
            get;
            private set;
        }

        //public int BlurLength
        //{
        //    get;
        //    private set;
        //}

        public override int NbCaseBorder
        {
            get
            {
                return 1;
            }
        }

        public VoronoiDataChunkLayer(string id, int nbPointsInside, int nbCaseSide)
            : base(id, nbCaseSide)
        {
            this.NbPointsInside = nbPointsInside;
        }

        protected override void InternalCreateChunks(ChunksMonitor dataChunksMonitor, List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainerToGenerate in obj)
            {
                VoronoiDataChunk voronoiDataChunk = new VoronoiDataChunk(chunkContainerToGenerate.Position, this.NbCaseSide, this.NbPointsInside, this.SampleLevel);

                dataChunksMonitor.AddChunkToMonitor(voronoiDataChunk);
            }
        }
    }
}