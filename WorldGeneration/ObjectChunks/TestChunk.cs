using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.DataChunks.PerlinNoise;
using WorldGeneration.DataChunks.VoronoiNoise;

namespace WorldGeneration.ObjectChunks
{
    public class TestChunk: IObjectChunk
    {
        protected ICase[,] casesArray;

        public Vector2i Position
        {
            get;
            private set;
        }

        public int NbCaseSide
        {
            get;
            private set;
        }

        //protected List<Vector2i> notGeneratedCases;

        public TestChunk(Vector2i position, int nbCaseSide)
        {
            this.Position = position;
            this.NbCaseSide = nbCaseSide;

            //this.notGeneratedCases = null;

            this.casesArray = new ICase[this.NbCaseSide, this.NbCaseSide];
            for (int i = 0; i < this.NbCaseSide; i++)
            {
                for (int j = 0; j < this.NbCaseSide; j++)
                {
                    this.casesArray[i, j] = null;
                }
            }
        }

        public bool GenerateChunk(ObjectChunkLayersMonitor dataChunksMonitor, IObjectChunkLayer parentLayer)
        {
            for (int i = 0; i < this.NbCaseSide; i++)
            {
                for (int j = 0; j < this.NbCaseSide; j++)
                {
                    Vector2i worldPosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(this.Position.X, this.Position.Y, j, i));
                    this.casesArray[i, j] = this.GenerateCaseAtWorldCoordinates(dataChunksMonitor, worldPosition);
                }
            }

            return true;
        }

        public ICase GenerateCaseAtWorldCoordinates(ObjectChunkLayersMonitor objectChunksMonitor, Vector2i position)
        {
            //VoronoiDataCase voroDataCase = objectChunksMonitor.DataChunkMonitor.DataChunksLayers["biome"].GetCaseAtWorldCoordinates(position.X, position.Y) as VoronoiDataCase;

            //PerlinDataCase dataCase = dataChunksMonitor.DataChunkMonitor.DataChunksLayers["landscape"].GetCaseAtWorldCoordinates(position.X, position.Y) as PerlinDataCase;
            //PerlinDataCase dataCase2 = dataChunksMonitor.DataChunkMonitor.DataChunksLayers["landscapeLevel2"].GetCaseAtWorldCoordinates(position.X, position.Y) as PerlinDataCase;
            //PerlinDataCase dataCase3 = dataChunksMonitor.DataChunkMonitor.DataChunksLayers["landscapeLevel3"].GetCaseAtWorldCoordinates(position.X, position.Y) as PerlinDataCase;
            //PerlinDataCase dataCase4 = dataChunksMonitor.DataChunkMonitor.DataChunksLayers["landscapeLevel4"].GetCaseAtWorldCoordinates(position.X, position.Y) as PerlinDataCase;

            TestCase generatedCase = new TestCase(position.X, position.Y);
            //generatedCase.AltitudeValue = dataCase.Value + dataCase2.Value * 0.5f + dataCase3.Value * 0.25f + dataCase4.Value * 0.15f;

            generatedCase.AltitudeValue = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["altitude"] as AltitudeDataAgreggator).GetAltitudeAtWorldCoordinates(position.X, position.Y, out bool isUnderSea);
            generatedCase.BiomeValue = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["biome"] as BiomeDataAgreggator).GetBiomeAtWorldCoordinates(position.X, position.Y);
            generatedCase.IsUnderSea = isUnderSea;

            generatedCase.TestValue = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["test"] as TestNoiseDataAgreggator).GetTestValueAtWorldCoordinates(position.X, position.Y);

            return generatedCase;
        }

        public ICase GetCaseAtLocal(int x, int y)
        {
            return this.casesArray[y, x];
        }
    }
}
