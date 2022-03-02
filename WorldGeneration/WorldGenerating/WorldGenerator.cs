﻿using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.DataChunks.PerlinNoise;
using WorldGeneration.DataChunks.VoronoiNoise;
using WorldGeneration.ObjectChunks;

namespace WorldGeneration.WorldGenerating
{
    internal class WorldGenerator: IDisposable
    {
        private readonly object mainLock = new object();

        private bool wasAreaUpdated;
        private IntRect newWorldArea;
        private ChunkContainer chunkToGenerate;

        public event Action<IChunk> ChunkGenerated;

        private Thread chunkGenerationThread;
        private volatile bool isThreadRunning;

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

            this.isThreadRunning = true;
            this.chunkGenerationThread = new Thread(new ThreadStart(this.InternalUpdate));
            this.chunkGenerationThread.Start();
        }

        public void ConstructWorldGenerator()
        {
            BiomeDataAgreggator biomeDataAgreggator = new BiomeDataAgreggator(4);

            VoronoiDataChunkLayer voronoiDataChunkLayer = new VoronoiDataChunkLayer("biome", 2, 16, 32);
            this.dataChunksMonitor.AddDataLayerToGenerator(voronoiDataChunkLayer);
            biomeDataAgreggator.BiomeLayer = voronoiDataChunkLayer;

            // Region is 1024 cases width
            // high period 2048 cases ? lets try three octaves deep after it
            AltitudeDataAgreggator altitudeDataAgreggator = new AltitudeDataAgreggator(24);

            PerlinDataChunkLayer perlinDataChunkLayer = new PerlinDataChunkLayer("landscape", 32, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(1, perlinDataChunkLayer);

            perlinDataChunkLayer = new PerlinDataChunkLayer("landscapeLevel2", 16, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(0.5f, perlinDataChunkLayer);

            perlinDataChunkLayer = new PerlinDataChunkLayer("landscapeLevel3", 8, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(0.25f, perlinDataChunkLayer);

            perlinDataChunkLayer = new PerlinDataChunkLayer("landscapeLevel4", 2, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(0.15f, perlinDataChunkLayer);

            this.dataChunksMonitor.AddDataAgreggatorToGenerator("altitude", altitudeDataAgreggator);
            this.dataChunksMonitor.AddDataAgreggatorToGenerator("biome", biomeDataAgreggator);

            //PerlinDataChunkLayer perlinDataChunkLayer = new PerlinDataChunkLayer("landscape", 1024, 1);
            //this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);

            //perlinDataChunkLayer = new PerlinDataChunkLayer("landscapeLevel2", 512, 1);
            //this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);

            //perlinDataChunkLayer = new PerlinDataChunkLayer("landscapeLevel3", 256, 1);
            //this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);

            //perlinDataChunkLayer = new PerlinDataChunkLayer("landscapeLevel4", 32, 1);
            //this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
        }

        public void ConstructWorldGenerator2()
        {
            BiomeDataAgreggator biomeDataAgreggator = new BiomeDataAgreggator(4);

            VoronoiDataChunkLayer voronoiDataChunkLayer = new VoronoiDataChunkLayer("biome", 2, 256, 1024);
            this.dataChunksMonitor.AddDataLayerToGenerator(voronoiDataChunkLayer);
            biomeDataAgreggator.BiomeLayer = voronoiDataChunkLayer;

            // Region is 1024 cases width
            // high period 2048 cases ? lets try three octaves deep after it
            AltitudeDataAgreggator altitudeDataAgreggator = new AltitudeDataAgreggator(24);

            PerlinDataChunkLayer perlinDataChunkLayer = new PerlinDataChunkLayer("landscape", 1024, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(1, perlinDataChunkLayer);

            perlinDataChunkLayer = new PerlinDataChunkLayer("landscapeLevel2", 512, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(0.5f, perlinDataChunkLayer);

            perlinDataChunkLayer = new PerlinDataChunkLayer("landscapeLevel3", 256, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(0.25f, perlinDataChunkLayer);

            perlinDataChunkLayer = new PerlinDataChunkLayer("landscapeLevel4", 32, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(0.15f, perlinDataChunkLayer);

            this.dataChunksMonitor.AddDataAgreggatorToGenerator("altitude", altitudeDataAgreggator);
            this.dataChunksMonitor.AddDataAgreggatorToGenerator("biome", biomeDataAgreggator);
        }

        private void InternalUpdate()
        {
            while (this.isThreadRunning)
            {
                bool wasAreaUpdated = false;
                IntRect newWorldArea = new IntRect();
                ChunkContainer chunkToGenerate = null;
                lock (this.mainLock)
                {
                    if (this.wasAreaUpdated
                        || Monitor.Wait(this.mainLock))
                    {
                        wasAreaUpdated = true;
                        newWorldArea = this.newWorldArea;
                        chunkToGenerate = this.chunkToGenerate;
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
                }

                //Thread.Sleep(20);
            }
        }

        internal bool OrderChunkGeneration(IntRect newWorldArea, ChunkContainer chunkToGenerate)
        {
            bool resultOrderGeneration = false;

            lock (this.mainLock)
            {
                if (this.wasAreaUpdated == false)
                {
                    this.newWorldArea = newWorldArea;
                    this.chunkToGenerate = chunkToGenerate;

                    this.wasAreaUpdated = true;

                    resultOrderGeneration = true;

                    Monitor.Pulse(this.mainLock);
                }
                else if(this.chunkToGenerate.Position == chunkToGenerate.Position)
                {
                    resultOrderGeneration = true;
                }
            }

            return resultOrderGeneration;
        }

        public void Dispose()
        {
            this.isThreadRunning = false;
        }
    }
}
