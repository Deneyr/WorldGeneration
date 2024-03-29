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
using WorldGeneration.DataChunks.DSNoise;
using WorldGeneration.DataChunks.DSNoise.BiomeDSNoise;
using WorldGeneration.DataChunks.PerlinNoise;
using WorldGeneration.DataChunks.PerlinNoise.HPerlinNoise;
using WorldGeneration.DataChunks.PureNoise;
using WorldGeneration.DataChunks.StructureNoise.TallGrassStructure;
using WorldGeneration.DataChunks.StructureNoise.TownStructure;
using WorldGeneration.DataChunks.StructureNoise.TreeStructure;
using WorldGeneration.DataChunks.VoronoiNoise;
using WorldGeneration.DataChunks.VoronoiNoise.BiomeVoronoiNoise;
using WorldGeneration.ObjectChunks;
using WorldGeneration.ObjectChunks.ObjectChunkLayers;
using WorldGeneration.ObjectChunks.ObjectStructures;
using WorldGeneration.ObjectChunks.ObjectStructures.TallGrassStructures;
using WorldGeneration.ObjectChunks.ObjectStructures.TownStructures;
using WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures;

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
            BiomeDataAgreggator biomeDataAgreggator = new BiomeDataAgreggator();

            VoronoiDataChunkLayer voronoiDataChunkLayer = new VoronoiDataChunkLayer("biome", 2, 16);
            this.dataChunksMonitor.AddDataLayerToGenerator(voronoiDataChunkLayer);
            biomeDataAgreggator.BiomeLayer = voronoiDataChunkLayer;

            // Region is 1024 cases width
            // high period 2048 cases ? lets try three octaves deep after it
            AltitudeDataAgreggator altitudeDataAgreggator = new AltitudeDataAgreggator(32);

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
            // PART DATAS

            // Test
            TestNoiseDataAgreggator testNoiseDataAgreggator = new TestNoiseDataAgreggator();

            //PerlinDataChunkLayer testLayer = new HPerlinDataChunkLayer("test1", 128, 1);
            //testLayer.SampleLevel = 4;
            //this.dataChunksMonitor.AddDataLayerToGenerator(testLayer);
            //testNoiseDataAgreggator.TestLayer = testLayer;

            //this.dataChunksMonitor.AddDataAgreggatorToGenerator("test", testNoiseDataAgreggator);

            // End Test

            AltitudeDataAgreggator altitudeDataAgreggator = new AltitudeDataAgreggator(32);
            WeatherDataAgreggator weatherDataAgreggator = new WeatherDataAgreggator();
            BiomeDataAgreggator biomeDataAgreggator = new BiomeDataAgreggator();
            Offset2DDataAgreggator offset2DDataAgreggator = new Offset2DDataAgreggator();
            RiverDataAgreggator riverDataAgreggator = new RiverDataAgreggator();
            TownDataAgreggator townDataAgreggator = new TownDataAgreggator();
            FloraDataAgreggator floraDataAgreggator = new FloraDataAgreggator();
            TallGrassDataAgreggator tallGrassDataAgreggator = new TallGrassDataAgreggator();
            TreeDataAgreggator treeDataAgreggator = new TreeDataAgreggator();

            PureNoiseDataChunkLayer pureNoiseDataChunkLayer = new PureNoiseDataChunkLayer("pureNoise", 32);
            this.dataChunksMonitor.AddDataLayerToGenerator(pureNoiseDataChunkLayer);
            floraDataAgreggator.PureNoiseLayer = pureNoiseDataChunkLayer;
            treeDataAgreggator.PureNoiseLayer = pureNoiseDataChunkLayer;

            // Part Biomes

            BiomeDSDataChunkLayer biomeDSDataChunkLayer = new BiomeDSDataChunkLayer("biomeOffset", 5, 2);
            biomeDSDataChunkLayer.SampleLevel = 2;
            this.dataChunksMonitor.AddDataLayerToGenerator(biomeDSDataChunkLayer);
            offset2DDataAgreggator.OffsetLayer = biomeDSDataChunkLayer;

            PerlinDataChunkLayer offsetPerlinDataChunkLayer = new PerlinDataChunkLayer("offsetX", 32, 6);
            this.dataChunksMonitor.AddDataLayerToGenerator(offsetPerlinDataChunkLayer);
            offset2DDataAgreggator.SmoothOffsetLayerX = offsetPerlinDataChunkLayer;

            offsetPerlinDataChunkLayer = new PerlinDataChunkLayer("offsetY", 32, 6);
            this.dataChunksMonitor.AddDataLayerToGenerator(offsetPerlinDataChunkLayer);
            offset2DDataAgreggator.SmoothOffsetLayerY = offsetPerlinDataChunkLayer;

            HPerlinDataChunkLayer hPerlinDataChunkLayer = new HPerlinDataChunkLayer("temperature", 512, 1);
            hPerlinDataChunkLayer.Margin = 32;
            hPerlinDataChunkLayer.SampleLevel = 4;
            this.dataChunksMonitor.AddDataLayerToGenerator(hPerlinDataChunkLayer);
            weatherDataAgreggator.TemperatureLayer = hPerlinDataChunkLayer;

            hPerlinDataChunkLayer = new HPerlinDataChunkLayer("humidity", 512, 1);
            hPerlinDataChunkLayer.Margin = 32;
            hPerlinDataChunkLayer.SampleLevel = 4;
            this.dataChunksMonitor.AddDataLayerToGenerator(hPerlinDataChunkLayer);
            weatherDataAgreggator.HumidityLayer = hPerlinDataChunkLayer;

            BiomeVoronoiDataChunkLayer biomeVoronoiDataChunkLayer = new BiomeVoronoiDataChunkLayer("biome", 1, 32);
            this.dataChunksMonitor.AddDataLayerToGenerator(biomeVoronoiDataChunkLayer);
            biomeDataAgreggator.BiomeLayer = biomeVoronoiDataChunkLayer;
            riverDataAgreggator.RiverLayer = biomeVoronoiDataChunkLayer;

            testNoiseDataAgreggator.TestLayer = biomeVoronoiDataChunkLayer;

            // Part Altitude

            // Region is 1024 cases width
            // high period 2048 cases ? lets try three octaves deep after it
            PerlinDataChunkLayer perlinDataChunkLayer = new PerlinDataChunkLayer("landscape", 1024, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(1, perlinDataChunkLayer);

            perlinDataChunkLayer = new PerlinDataChunkLayer("landscape2Level2", 512, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(0.5f, perlinDataChunkLayer);

            perlinDataChunkLayer = new PerlinDataChunkLayer("landscape3Level3", 256, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(0.25f, perlinDataChunkLayer);

            perlinDataChunkLayer = new PerlinDataChunkLayer("landscape4Level4", 128, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(0.12f, perlinDataChunkLayer);

            perlinDataChunkLayer = new PerlinDataChunkLayer("landscape5Level5", 64, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(0.4f, perlinDataChunkLayer);

            perlinDataChunkLayer = new PerlinDataChunkLayer("landscape6Level6", 32, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(0.2f, perlinDataChunkLayer);

            perlinDataChunkLayer = new PerlinDataChunkLayer("landscape7Level7", 16, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(0.1f, perlinDataChunkLayer);

            perlinDataChunkLayer = new PerlinDataChunkLayer("landscape8Level8", 8, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddAltitudeLayer(0.05f, perlinDataChunkLayer);


            perlinDataChunkLayer = new PerlinDataChunkLayer("landscapeWaterLevel", 20, 1);
            this.dataChunksMonitor.AddDataLayerToGenerator(perlinDataChunkLayer);
            altitudeDataAgreggator.AddSeaLayer(0.08f, perlinDataChunkLayer);
            altitudeDataAgreggator.BiomeDataAgreggator = biomeDataAgreggator;

            // Part Towns
            TownDataStructureChunkLayer townStructureDataChunkLayer = new TownDataStructureChunkLayer("town", 1024);
            townStructureDataChunkLayer.NbMinDataStructure = 25;
            townStructureDataChunkLayer.NbMaxDataStructure = 50;
            townStructureDataChunkLayer.StructDimension = new IntRect(32, 32, 48, 48);
            this.dataChunksMonitor.AddDataLayerToGenerator(townStructureDataChunkLayer);
            townDataAgreggator.AddDataStructureChunkLayer(townStructureDataChunkLayer);

            // Part flora & rocks
            hPerlinDataChunkLayer = new HPerlinDataChunkLayer("flora", 256, 1);
            hPerlinDataChunkLayer.Margin = 32;
            hPerlinDataChunkLayer.SampleLevel = 4;
            this.dataChunksMonitor.AddDataLayerToGenerator(hPerlinDataChunkLayer);
            floraDataAgreggator.FloraLayer = hPerlinDataChunkLayer;
            treeDataAgreggator.ForestLayer = hPerlinDataChunkLayer;

            // Part Trees
            TreeDataStructureChunkLayer treeStructureDataChunkLayer = new TreeDataStructureChunkLayer("tree", 256);
            treeStructureDataChunkLayer.NbMinDataStructure = 10000;
            treeStructureDataChunkLayer.NbMaxDataStructure = 12000;
            treeStructureDataChunkLayer.StructDimension = new IntRect(3, 3, 3, 3);
            this.dataChunksMonitor.AddDataLayerToGenerator(treeStructureDataChunkLayer);
            treeDataAgreggator.AddDataStructureChunkLayer(treeStructureDataChunkLayer);

            // Part Tall Grass
            TallGrassDataStructureChunkLayer tallGrassStructureDataChunkLayer = new TallGrassDataStructureChunkLayer("tallGrass", 256);
            tallGrassStructureDataChunkLayer.NbMinDataStructure = 10;
            tallGrassStructureDataChunkLayer.NbMaxDataStructure = 25;
            tallGrassStructureDataChunkLayer.StructDimension = new IntRect(20, 20, 40, 40);
            this.dataChunksMonitor.AddDataLayerToGenerator(tallGrassStructureDataChunkLayer);
            tallGrassDataAgreggator.AddDataStructureChunkLayer(tallGrassStructureDataChunkLayer);

            tallGrassStructureDataChunkLayer = new TallGrassDataStructureChunkLayer("secondTallGrass", 128);
            tallGrassStructureDataChunkLayer.NbMinDataStructure = 15;
            tallGrassStructureDataChunkLayer.NbMaxDataStructure = 30;
            tallGrassStructureDataChunkLayer.StructDimension = new IntRect(10, 10, 20, 20);
            this.dataChunksMonitor.AddDataLayerToGenerator(tallGrassStructureDataChunkLayer);
            tallGrassDataAgreggator.AddDataStructureChunkLayer(tallGrassStructureDataChunkLayer);


            // Register data aggregators

            this.dataChunksMonitor.AddAltitudeAgreggatorToGenerator("altitude", altitudeDataAgreggator);
            this.dataChunksMonitor.AddDataAgreggatorToGenerator("biome", biomeDataAgreggator);
            this.dataChunksMonitor.AddDataAgreggatorToGenerator("weather", weatherDataAgreggator);
            this.dataChunksMonitor.AddDataAgreggatorToGenerator("2DOffset", offset2DDataAgreggator);
            this.dataChunksMonitor.AddDataAgreggatorToGenerator("river", riverDataAgreggator);
            this.dataChunksMonitor.AddDataAgreggatorToGenerator("town", townDataAgreggator);
            this.dataChunksMonitor.AddDataAgreggatorToGenerator("flora", floraDataAgreggator);
            this.dataChunksMonitor.AddDataAgreggatorToGenerator("tallGrass", tallGrassDataAgreggator);
            this.dataChunksMonitor.AddDataAgreggatorToGenerator("tree", treeDataAgreggator);

            this.dataChunksMonitor.AddDataAgreggatorToGenerator("test", testNoiseDataAgreggator);

            // PART OBJECTS

            // Part Structure Templates
            this.objectChunkMonitor.AddObjectStructureTemplatesToGenerator(new TownObjectStructureTemplate());
            this.objectChunkMonitor.AddObjectStructureTemplatesToGenerator(new TreeObjectStructureTemplate());
            this.objectChunkMonitor.AddObjectStructureTemplatesToGenerator(new NarrowTreeObjectStructureTemplate());
            this.objectChunkMonitor.AddObjectStructureTemplatesToGenerator(new TallGrassObjectStructureTemplate());

            // Part chunk layers
            BiomeObjectChunkLayer biomeObjectChunkLayer = new BiomeObjectChunkLayer("biomeLayer");
            this.objectChunkMonitor.AddObjectLayerToGenerator(biomeObjectChunkLayer);

            AltitudeObjectChunkLayer altitudeObjectChunkLayer = new AltitudeObjectChunkLayer("altitudeLayer");
            this.objectChunkMonitor.AddObjectLayerToGenerator(altitudeObjectChunkLayer);


            BiomeTransitionObjectChunkLayer biomeTransitionObjectChunkLayer = new BiomeTransitionObjectChunkLayer("biomeTransitionLayer");
            this.objectChunkMonitor.AddObjectLayerToGenerator(biomeTransitionObjectChunkLayer);

            AltitudeTransitionObjectChunkLayer altitudeTransitionObjectChunkLayer = new AltitudeTransitionObjectChunkLayer("altitudeTransitionLayer");
            this.objectChunkMonitor.AddObjectLayerToGenerator(altitudeTransitionObjectChunkLayer);


            WaterObjectChunkLayer waterObjectChunkLayer = new WaterObjectChunkLayer("waterLayer");
            this.objectChunkMonitor.AddObjectLayerToGenerator(waterObjectChunkLayer);

            WaterTransitionObjectChunkLayer waterTransitionObjectChunkLayer = new WaterTransitionObjectChunkLayer("waterTransitionLayer");
            this.objectChunkMonitor.AddObjectLayerToGenerator(waterTransitionObjectChunkLayer);

            TownObjectChunkLayer townObjectChunkLayer = new TownObjectChunkLayer("townLayer");
            this.objectChunkMonitor.AddObjectLayerToGenerator(townObjectChunkLayer);

            // TODO : replace to rock chunk layer
            FloraCObjectChunkLayer floraCObjectChunkLayer = new FloraCObjectChunkLayer("floraCLayer");
            this.objectChunkMonitor.AddObjectLayerToGenerator(floraCObjectChunkLayer);

            TreeObjectChunkLayer structureTreeObjectChunkLayer = new TreeObjectChunkLayer("structureTreeLayer");
            this.objectChunkMonitor.AddObjectLayerToGenerator(structureTreeObjectChunkLayer);

            TallGrassObjectChunkLayer structureTallGrassObjectChunkLayer = new TallGrassObjectChunkLayer("structureTallGrassLayer");
            this.objectChunkMonitor.AddObjectLayerToGenerator(structureTallGrassObjectChunkLayer);

            FloraNCObjectChunkLayer floraNCObjectChunkLayer = new FloraNCObjectChunkLayer("floraNCLayer");
            this.objectChunkMonitor.AddObjectLayerToGenerator(floraNCObjectChunkLayer);
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
                    // Extend newWorldArea to fit margin exigences from the objectChunkMonitor
                    newWorldArea = this.ExtendAreaToGenerate(newWorldArea);

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

        private IntRect ExtendAreaToGenerate(IntRect areaToExtend)
        {
            return new IntRect(
                areaToExtend.Left - this.objectChunkMonitor.MaxObjectChunkLayerMargin, 
                areaToExtend.Top - this.objectChunkMonitor.MaxObjectChunkLayerMargin, 
                areaToExtend.Width + 2 * this.objectChunkMonitor.MaxObjectChunkLayerMargin, 
                areaToExtend.Height + 2 * this.objectChunkMonitor.MaxObjectChunkLayerMargin);
        }

        public void Dispose()
        {
            this.isThreadRunning = false;
        }
    }
}
