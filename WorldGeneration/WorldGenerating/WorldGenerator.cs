using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks;
using WorldGeneration.DataChunks.PerlinNoise;
using WorldGeneration.ObjectChunks;

namespace WorldGeneration.WorldGenerating
{
    internal class WorldGenerator
    {
        private readonly object mainLock = new object();

        private bool wasAreaUpdated;
        private IntRect newWorldArea;
        private ChunkContainer chunkToGenerate;

        public event Action<IChunk> ChunkGenerated;

        private Task chunkGenerationTask;

        private DataChunkLayersMonitor dataChunksMonitor;
        private ObjectChunkLayersMonitor objectChunkMonitor;

        public int NbChunkCaseSide
        {
            get;
            private set;
        }

        public int WorldSeed
        {
            get;
            private set;
        }

        public WorldGenerator(int nbChunkCaseSide, int seed)
        {
            this.NbChunkCaseSide = nbChunkCaseSide;
            this.WorldSeed = seed;

            this.dataChunksMonitor = new DataChunkLayersMonitor(this.WorldSeed);
            this.objectChunkMonitor = new ObjectChunkLayersMonitor(this.dataChunksMonitor, nbChunkCaseSide, seed);

            this.wasAreaUpdated = false;

            this.chunkGenerationTask = null;
        }

        public void ConstructWorldGenerator()
        {
            PerlinDataChunkLayer perlinDataChunkLayer = new PerlinDataChunkLayer("landscape", 16, 8);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
        }

        private void InternalUpdate()
        {
            bool wasAreaUpdated = false;
            IntRect newWorldArea = new IntRect();
            ChunkContainer chunkToGenerate = null;
            lock (this.mainLock)
            {
                if (this.wasAreaUpdated)
                {
                    wasAreaUpdated = true;
                    newWorldArea = this.newWorldArea;
                    chunkToGenerate = this.chunkToGenerate;

                    this.wasAreaUpdated = false;
                }
            }

            if (wasAreaUpdated)
            {
                // Update data chunks
                this.dataChunksMonitor.UpdateWorldArea(newWorldArea);

                // Chunk generation
                IChunk chunkGenerated = this.objectChunkMonitor.GenerateChunkAt(chunkToGenerate.Position);

                // Raise Event
                this.ChunkGenerated?.Invoke(chunkGenerated);

                lock (this.mainLock)
                {
                    this.newWorldArea = new IntRect();

                    this.chunkToGenerate = null;

                    this.wasAreaUpdated = false;
                }

                this.chunkGenerationTask = null;
            }
        }

        internal bool OrderChunkGeneration(IntRect newWorldArea, ChunkContainer chunkToGenerate)
        {
            bool resultOrderGeneration = false;
            lock (this.mainLock)
            {
                if(this.wasAreaUpdated == false)
                {
                    this.newWorldArea = newWorldArea;
                    this.chunkToGenerate = chunkToGenerate;

                    this.wasAreaUpdated = true;

                    resultOrderGeneration = true;
                }
                else if(this.chunkToGenerate.Position == chunkToGenerate.Position)
                {
                    resultOrderGeneration = true;
                }
            }

            if (resultOrderGeneration)
            {
                this.chunkGenerationTask = new Task(this.InternalUpdate);
                this.chunkGenerationTask.Start();
            }

            return resultOrderGeneration;
        }

    }
}
