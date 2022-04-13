using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.WebNoise
{
    internal class WebDataChunkLayer : AExtendedDataChunkLayer
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

        public int WebMargin
        {
            get;
            private set;
        }

        public WebDataChunkLayer(string id, int nbCaseSide, int webMargin)
            : base(id, nbCaseSide)
        {
            this.WebMargin = webMargin;
        }

        protected override void InternalCreateChunks(ChunksMonitor dataChunksMonitor, List<ChunkContainer> obj)
        {
            foreach (ChunkContainer chunkContainerToGenerate in obj)
            {
                WebDataChunk voronoiDataChunk = new WebDataChunk(chunkContainerToGenerate.Position, this.NbCaseSide, this.WebMargin, this.SampleLevel);

                dataChunksMonitor.AddChunkToMonitor(voronoiDataChunk);
            }
        }
    }
}