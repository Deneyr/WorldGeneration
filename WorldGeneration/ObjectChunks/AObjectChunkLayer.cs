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
    internal abstract class AObjectChunkLayer : IObjectChunkLayer
    {
        public virtual int ObjectChunkMargin
        {
            get
            {
                return 0;
            }
        }

        public string Id
        {
            get;
            private set;
        }

        public AObjectChunkLayer(string id)
        {
            this.Id = id;
        }

        public virtual void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            int chunkSeed = this.GenerateChunkSeed(objectChunk, objectChunksMonitor.WorldSeed);
            Random random = new Random(chunkSeed);

            for (int i = 0; i < objectChunk.NbCaseSide; i++)
            {
                for (int j = 0; j < objectChunk.NbCaseSide; j++)
                {
                    this.ComputeChunkArea(objectChunksMonitor, random, objectChunk, new Vector2i(j, i), this.GetWorldPosition(objectChunk, j, i));
                }
            }
        }

        protected abstract void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition);

        protected virtual int GenerateChunkSeed(IObjectChunk objectChunk, int seed)
        {
            int realSeed = seed + this.Id.GetHashCode();
            return objectChunk.Position.X * objectChunk.Position.Y - realSeed - objectChunk.NbCaseSide - objectChunk.Position.X + objectChunk.Position.Y * objectChunk.Position.Y;
        }

        protected Vector2i GetWorldPosition(IObjectChunk objectChunk, int localX, int localY)
        {
            return ChunkHelper.GetWorldPositionFromChunkPosition(objectChunk.NbCaseSide, new IntRect(objectChunk.Position.X, objectChunk.Position.Y, localX, localY));
        }

        public virtual void InitObjectChunkLayer(int nbCaseSide)
        {
            // To override
        }
    }
}
