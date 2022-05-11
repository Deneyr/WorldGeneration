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
    public abstract class AObjectChunkLayer : IObjectChunkLayer
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

        public abstract void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk);

        protected virtual int GenerateChunkSeed(IObjectChunk objectChunk, int seed)
        {
            int realSeed = seed + this.Id.GetHashCode();
            return objectChunk.Position.X * objectChunk.Position.Y - realSeed - objectChunk.NbCaseSide - objectChunk.Position.X + objectChunk.Position.Y * objectChunk.Position.Y;
        }

        protected Vector2i GetWorldPosition(IObjectChunk objectChunk, int localX, int localY)
        {
            return ChunkHelper.GetWorldPositionFromChunkPosition(objectChunk.NbCaseSide, new IntRect(objectChunk.Position.X, objectChunk.Position.Y, localX, localY));
        }
    }
}
