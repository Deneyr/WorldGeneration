using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.ObjectChunks.ObjectStructures;

namespace WorldGeneration.ObjectChunks
{
    internal abstract class AObjectStructureChunkLayer : IObjectChunkLayer
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

        public AObjectStructureChunkLayer(string id)
        {
            this.Id = id;
        }

        public virtual void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            int chunkSeed = this.GenerateChunkSeed(objectChunk, objectChunksMonitor.WorldSeed);
            Random random = new Random(chunkSeed);

            IntRect worldArea = ChunkHelper.GetWorldAreaFromChunkArea(objectChunk.NbCaseSide, new IntRect(objectChunk.Position.X, objectChunk.Position.Y, 1, 1));

            worldArea = new IntRect(
                worldArea.Left - this.ObjectChunkMargin,
                worldArea.Top - this.ObjectChunkMargin,
                worldArea.Width + 2 * this.ObjectChunkMargin,
                worldArea.Height + 2 * this.ObjectChunkMargin);

            List<IDataStructure> dataStructuresInWorldArea = this.GetDataStructuresInWorldArea(worldArea);

            foreach(IDataStructure dataStructure in dataStructuresInWorldArea)
            {
                this.ConstructObjectStructureFrom(objectChunksMonitor, objectChunk, random, dataStructure);
            }
        }

        protected abstract List<IDataStructure> GetDataStructuresInWorldArea(IntRect worldArea);

        protected abstract IObjectStructure ConstructObjectStructureFrom(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk, Random random, IDataStructure dataStructure);

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
