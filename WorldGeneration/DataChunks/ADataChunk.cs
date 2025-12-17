using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.Maths.RandomHelpers;
using WorldGeneration.WorldGenerating;

namespace WorldGeneration.DataChunks
{
    internal abstract class ADataChunk : IDataChunk
    {
        protected int realNbCaseSide;

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

        public int SampleLevel
        {
            get;
            set;
        }

        //protected List<Vector2i> notGeneratedCases;

        public ADataChunk(Vector2i position, int nbCaseSide, int sampleLevel)
        {
            this.Position = position;
            this.NbCaseSide = nbCaseSide;

            this.SampleLevel = sampleLevel;
            this.realNbCaseSide = (int) Math.Ceiling(((float) nbCaseSide) / this.SampleLevel);
            //this.notGeneratedCases = null;

            this.casesArray = new ICase[this.realNbCaseSide, this.realNbCaseSide];
            for (int i = 0; i < this.realNbCaseSide; i++)
            {
                for (int j = 0; j < this.realNbCaseSide; j++)
                {
                    this.casesArray[i, j] = null;
                }
            }
        }

        public abstract void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer);

        public virtual void GenerateChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            ulong chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed, parentLayer.HashedId);
            WGRandom random = new WGRandom(chunkSeed);

            //this.notGeneratedCases = new List<Vector2i>();

            for (int i = 0; i < this.realNbCaseSide; i++)
            {
                for (int j = 0; j < this.realNbCaseSide; j++)
                {
                    ICase generatedCase = this.GenerateCase(dataChunksMonitor, parentLayer, j, i, random);

                    this.casesArray[i, j] = generatedCase;
                }
            }
        }

        protected abstract ICase GenerateCase(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer, int x, int y, WGRandom random);

        protected virtual ulong GenerateChunkSeed(ulong worldSeed, ulong layerSeed)
        {
            ulong h = worldSeed;
            h ^= HashHelpers.Mix(layerSeed);
            h ^= HashHelpers.Mix((ulong)(long)this.Position.X);
            h ^= HashHelpers.Mix((ulong)(long)this.Position.Y);
            return HashHelpers.Mix(h);
            //worldSeed ^= (ulong)(this.Position.X) * 0x9E3779B97F4A7C15UL;
            //worldSeed ^= (ulong)(this.Position.Y) * 0xC2B2AE3D27D4EB4FUL;
            //worldSeed ^= (ulong)(layerSeed) * 0x165667B19E3779F9UL;
            //return HashHelpers.Mix(worldSeed);
            //return this.Position.X * this.Position.Y * seed + seed + this.NbCaseSide + this.Position.X + this.Position.Y * this.Position.Y;
        }

        public ICase GetCaseAtLocal(int x, int y)
        {
            return this.casesArray[y / this.SampleLevel, x / this.SampleLevel];
        }
    }
}
