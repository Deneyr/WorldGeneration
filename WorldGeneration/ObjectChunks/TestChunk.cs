﻿//using SFML.Graphics;
//using SFML.System;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WorldGeneration.ChunksMonitoring;
//using WorldGeneration.DataChunks.DataAgreggator;
//using WorldGeneration.DataChunks.PerlinNoise;
//using WorldGeneration.DataChunks.VoronoiNoise;
//using WorldGeneration.ObjectChunks.BiomeManager;

//namespace WorldGeneration.ObjectChunks
//{
//    public class TestChunk: IObjectChunk
//    {
//        protected ICase[,] casesArray;

//        public Vector2i Position
//        {
//            get;
//            private set;
//        }

//        public int NbCaseSide
//        {
//            get;
//            private set;
//        }

//        public IZObjectCase[,] ZCasesArray => throw new NotImplementedException();

//        //protected List<Vector2i> notGeneratedCases;

//        public TestChunk(Vector2i position, int nbCaseSide, int seaLevel)
//        {
//            this.Position = position;
//            this.NbCaseSide = nbCaseSide;

//            this.casesArray = new ICase[this.NbCaseSide, this.NbCaseSide];
//            for (int i = 0; i < this.NbCaseSide; i++)
//            {
//                for (int j = 0; j < this.NbCaseSide; j++)
//                {
//                    this.casesArray[i, j] = null;
//                }
//            }
//        }

//        public bool GenerateChunk(ObjectChunkLayersMonitor dataChunksMonitor, IObjectChunkLayer parentLayer)
//        {
//            int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed - "TestChunkLayerGenerator".GetHashCode());
//            Random random = new Random(chunkSeed);

//            for (int i = 0; i < this.NbCaseSide; i++)
//            {
//                for (int j = 0; j < this.NbCaseSide; j++)
//                {
//                    Vector2i worldPosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position.X, this.Position.Y, j, i));
//                    this.casesArray[i, j] = this.GenerateCaseAtWorldCoordinates(dataChunksMonitor, worldPosition, random);
//                }
//            }

//            return true;
//        }

//        public ICase GenerateCaseAtWorldCoordinates(ObjectChunkLayersMonitor objectChunksMonitor, Vector2i position, Random random)
//        {
//            //VoronoiDataCase voroDataCase = objectChunksMonitor.DataChunkMonitor.DataChunksLayers["biome"].GetCaseAtWorldCoordinates(position.X, position.Y) as VoronoiDataCase;

//            //PerlinDataCase dataCase = dataChunksMonitor.DataChunkMonitor.DataChunksLayers["landscape"].GetCaseAtWorldCoordinates(position.X, position.Y) as PerlinDataCase;
//            //PerlinDataCase dataCase2 = dataChunksMonitor.DataChunkMonitor.DataChunksLayers["landscapeLevel2"].GetCaseAtWorldCoordinates(position.X, position.Y) as PerlinDataCase;
//            //PerlinDataCase dataCase3 = dataChunksMonitor.DataChunkMonitor.DataChunksLayers["landscapeLevel3"].GetCaseAtWorldCoordinates(position.X, position.Y) as PerlinDataCase;
//            //PerlinDataCase dataCase4 = dataChunksMonitor.DataChunkMonitor.DataChunksLayers["landscapeLevel4"].GetCaseAtWorldCoordinates(position.X, position.Y) as PerlinDataCase;

//            TestCase generatedCase = new TestCase(position.X, position.Y);
//            //generatedCase.AltitudeValue = dataCase.Value + dataCase2.Value * 0.5f + dataCase3.Value * 0.25f + dataCase4.Value * 0.15f;

//            generatedCase.AltitudeValue = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["altitude"] as AltitudeDataAgreggator).GetAltitudeAtWorldCoordinates(position.X, position.Y, out bool isUnderSea);
//            float borderValue = 0;
//            generatedCase.BiomeValue = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["biome"] as BiomeDataAgreggator).GetBiomeAtWorldCoordinates(position.X, position.Y, out borderValue);
//            generatedCase.IsUnderSea = isUnderSea;

//            if (generatedCase.AltitudeValue > 14 && generatedCase.AltitudeValue < 22)
//            {
//                generatedCase.RiverValue = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["river"] as RiverDataAgreggator).GetRiverValueAtWorldCoordinates(position.X, position.Y);
//                generatedCase.AltitudeValue -= (int) Math.Ceiling(generatedCase.RiverValue * 4);
//            }
//            else
//            {
//                generatedCase.RiverValue = 0;
//            }

//            if (generatedCase.IsUnderSea == false && generatedCase.RiverValue == 0)
//            {
//                FloraRatioBiomeManager floraRatioManager = objectChunksMonitor.DataChunkMonitor.FloraRatioManager;

//                generatedCase.IsThereTree = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["flora"] as FloraDataAgreggator).IsThereTreeAtWorldCoordinate(position.X, position.Y, floraRatioManager.GetTreeRatioFromBiomeAltitude(generatedCase.BiomeValue, generatedCase.AltitudeValue));
//                generatedCase.IsThereRock = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["flora"] as FloraDataAgreggator).IsThereRockAtWorldCoordinate(position.X, position.Y, floraRatioManager.GetRockRatioFromBiomeAltitude(generatedCase.BiomeValue, generatedCase.AltitudeValue));
//                generatedCase.IsThereFlower = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["flora"] as FloraDataAgreggator).IsThereFlowerAtWorldCoordinate(position.X, position.Y, floraRatioManager.GetVegetationRatioFromBiomeAltitude(generatedCase.BiomeValue, generatedCase.AltitudeValue));
//                generatedCase.IsThereTallGrass = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["tallGrass"] as TallGrassDataAgreggator).IsThereTallGrassAtWorldCoordinates(position.X, position.Y);
//            }

//            return generatedCase;
//        }

//        protected virtual int GenerateChunkSeed(int seed)
//        {
//            return this.Position.X * this.Position.Y - seed - this.NbCaseSide - this.Position.X + this.Position.Y * this.Position.Y;
//        }

//        public ICase GetCaseAtLocal(int x, int y)
//        {
//            return this.casesArray[y, x];
//        }
//    }
//}
