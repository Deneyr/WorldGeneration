using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.Maths.RandomHelpers;

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

        public ulong HashedId
        {
            get;
            private set;
        }

        public AObjectChunkLayer(string id)
        {
            this.Id = id;
            this.HashedId = HashHelpers.HashString(id);
        }

        public virtual void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            ulong chunkSeed = this.GenerateChunkSeed(objectChunk, objectChunksMonitor.WorldSeed);
            WGRandom random = new WGRandom(chunkSeed);

            for (int i = 0; i < objectChunk.NbCaseSide; i++)
            {
                for (int j = 0; j < objectChunk.NbCaseSide; j++)
                {
                    this.ComputeChunkArea(objectChunksMonitor, random, objectChunk, new Vector2i(j, i), this.GetWorldPosition(objectChunk, j, i));
                }
            }
        }

        protected abstract void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, WGRandom random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition);

        protected virtual ulong GenerateChunkSeed(IObjectChunk objectChunk, ulong worldSeed)
        {
            ulong h = worldSeed;
            h ^= HashHelpers.Mix(this.HashedId * 0x8BADF00DDEADC0DEUL);
            h ^= HashHelpers.Mix((ulong)(long)objectChunk.Position.X);
            h ^= HashHelpers.Mix((ulong)(long)objectChunk.Position.Y);
            return HashHelpers.Mix(h);

            //ulong modifiedLayerSeed = (ulong)this.Id.GetHashCode() * 0x8BADF00DDEADC0DEUL;
            //seed ^= (ulong)(objectChunk.Position.X) * 0x9E3779B97F4A7C15UL;
            //seed ^= (ulong)(objectChunk.Position.Y) * 0xC2B2AE3D27D4EB4FUL;
            //seed ^= (ulong)(modifiedLayerSeed) * 0x165667B19E3779F9UL;
            //return HashHelpers.Hash(seed);

            //int realSeed = seed + this.Id.GetHashCode();
            //return objectChunk.Position.X * objectChunk.Position.Y - realSeed - objectChunk.NbCaseSide - objectChunk.Position.X + objectChunk.Position.Y * objectChunk.Position.Y;
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
