using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.ObjectChunks
{
    internal abstract class A1PassObjectChunkLayer : AObjectChunkLayer
    {
        public int[,] AreaBuffer
        {
            get;
            private set;
        }

        public override int ObjectChunkMargin
        {
            get
            {
                return 1;
            }
        }

        public A1PassObjectChunkLayer(string id)
            : base(id)
        {       
            this.AreaBuffer = null;
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            int chunkSeed = this.GenerateChunkSeed(objectChunk, objectChunksMonitor.WorldSeed);
            Random random = new Random(chunkSeed);

            for (int i = -this.ObjectChunkMargin; i < objectChunk.NbCaseSide + this.ObjectChunkMargin; i++)
            {
                for (int j = -this.ObjectChunkMargin; j < objectChunk.NbCaseSide + this.ObjectChunkMargin; j++)
                {
                    Vector2i worldPosition = ChunkHelper.GetWorldPositionFromChunkPosition(objectChunk.NbCaseSide, new IntRect(objectChunk.Position.X, objectChunk.Position.Y, j, i));

                    this.ComputeBufferArea(objectChunksMonitor, random, objectChunk, new Vector2i(j, i), worldPosition);
                }
            }

            for (int i = 0; i < objectChunk.NbCaseSide; i++)
            {
                for (int j = 0; j < objectChunk.NbCaseSide; j++)
                {
                    this.ComputeChunkArea(objectChunksMonitor, random, objectChunk, new Vector2i(j, i), this.GetWorldPosition(objectChunk, j, i));
                }
            }
        }

        protected abstract void ComputeBufferArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition);

        public override void InitObjectChunkLayer(int nbCaseSide)
        {
            int caseSideExtended = nbCaseSide + this.ObjectChunkMargin * 2;
            this.AreaBuffer = new int[caseSideExtended, caseSideExtended];
        }

        public int GetAreaBufferValueAtLocal(int x, int y)
        {
            return this.AreaBuffer[y + this.ObjectChunkMargin, x + this.ObjectChunkMargin];
        }
    }
}