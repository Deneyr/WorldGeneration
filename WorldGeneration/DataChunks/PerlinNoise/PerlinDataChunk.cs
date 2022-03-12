using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.WorldGenerating;
using WorldGeneration.Maths;

namespace WorldGeneration.DataChunks.PerlinNoise
{
    internal class PerlinDataChunk : ADataChunk
    {
        protected int nbSummitCase;

        public Vector2f[,] SummitArray
        {
            get;
            private set;
        }

        public int NoiseFrequency
        {
            get;
            private set;
        }

        //private int[] gaussian = new int[100];

        //public override void GenerateChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        //{
        //    base.GenerateChunk(dataChunksMonitor, parentLayer);

        //    for (int i = 0; i < 100; i++)
        //    {
        //        gaussian[i] = 0;
        //    }

        //    for (int i = 0; i < this.NbCaseSide; i++)
        //    {
        //        for (int j = 0; j < this.NbCaseSide; j++)
        //        {
        //            if((this.CasesArray[i, j] as PerlinDataCase).Value > 0.65)
        //            {
        //                Console.WriteLine();
        //            }

        //            int value = (int) ((((this.CasesArray[i, j] as PerlinDataCase).Value + 1) / 2) * 100);

        //            gaussian[value]++;
        //        }
        //    }
        //}

        public PerlinDataChunk(Vector2i position, int nbCaseSide, int noiseFrequency): 
            base(position, nbCaseSide)
        {
            this.NoiseFrequency = noiseFrequency;

            this.nbSummitCase = nbCaseSide / noiseFrequency;
            this.SummitArray = new Vector2f[this.nbSummitCase, this.nbSummitCase];
        }

        public override void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed + parentLayer.Id.GetHashCode());
            Random random = new Random(chunkSeed);
            int nbSummitCase = this.SummitArray.GetLength(0);
            for (int i = 0; i < this.nbSummitCase; i++)
            {
                for (int j = 0; j < this.nbSummitCase; j++)
                {
                    this.SummitArray[i, j] = this.GenerateSummitVector(dataChunksMonitor, parentLayer, j, i, random);
                }
            }
        }

        protected Vector2f GenerateSummitVector(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer, int x, int y, Random random)
        {
            double angle = random.NextDouble() * 2 * Math.PI;

            return new Vector2f((float) Math.Cos(angle), (float) Math.Sin(angle));
        }

        protected override ICase GenerateCase(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer, int x, int y, Random random)
        {
            Vector2f topLeftVector = this.GetTopLeftVector(parentLayer, x, y);

            Vector2f botLeftVector = this.GetBotLeftVector(parentLayer, x, y, out bool isThereVector);

            Vector2f topRightVector = this.GetTopRightVector(parentLayer, x, y, out bool isThereTopRightVector);
            isThereVector &= isThereTopRightVector;

            Vector2f botRightVector = this.GetBotRightVector(parentLayer, x, y, out bool isThereBotRightVector);
            isThereVector &= isThereBotRightVector;

            if (isThereVector)
            {
                PerlinDataCase caseGenerated = new PerlinDataCase(x, y);

                float realX = ((x + 0.5f) % this.NoiseFrequency) / this.NoiseFrequency;
                float realY = ((y + 0.5f) % this.NoiseFrequency) / this.NoiseFrequency;

                float topLeftValue = topLeftVector.Dot(new Vector2f(realX, realY));
                float botLeftValue = botLeftVector.Dot(new Vector2f(realX, realY - 1));
                float topRightValue = topRightVector.Dot(new Vector2f(realX - 1, realY));
                float botRightValue = botRightVector.Dot(new Vector2f(realX - 1, realY - 1));

                float fadeX = Fade(realX);
                float fadeY = Fade(realY);

                float topValue = topLeftValue * (1 - fadeX) + topRightValue * fadeX;
                float botValue = botLeftValue * (1 - fadeX) + botRightValue * fadeX;

                caseGenerated.Value = topValue * (1 - fadeY) + botValue * fadeY;

                return caseGenerated;
            }
            return null;
        }

        private Vector2f GetTopLeftVector(IDataChunkLayer parentLayer, int x, int y)
        {
            return this.SummitArray[(int)(y / this.NoiseFrequency), (int)(x / this.NoiseFrequency)];
        }

        private Vector2f GetBotLeftVector(IDataChunkLayer parentLayer, int x, int y, out bool isThereVector)
        {
            isThereVector = true;
            int frequencyNoiseY = (int)(y / this.NoiseFrequency) + 1;

            if(frequencyNoiseY >= this.nbSummitCase)
            {
                ChunkContainer nextChunkContainer = (parentLayer as AExtendedDataChunkLayer).ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X, this.Position.Y + 1);

                if(nextChunkContainer != null && nextChunkContainer.ContainedChunk != null)
                {
                    PerlinDataChunk perlinDataChunk = nextChunkContainer.ContainedChunk as PerlinDataChunk;

                    return perlinDataChunk.SummitArray[0, (int)(x / this.NoiseFrequency)];
                }

                isThereVector = false;
                return new Vector2f();
            }

            return this.SummitArray[frequencyNoiseY, (int)(x / this.NoiseFrequency)];
        }

        private Vector2f GetTopRightVector(IDataChunkLayer parentLayer, int x, int y, out bool isThereVector)
        {
            isThereVector = true;
            int frequencyNoiseX = (int)(x / this.NoiseFrequency) + 1;

            if (frequencyNoiseX >= this.nbSummitCase)
            {
                ChunkContainer nextChunkContainer = (parentLayer as AExtendedDataChunkLayer).ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X + 1, this.Position.Y);

                if (nextChunkContainer != null && nextChunkContainer.ContainedChunk != null)
                {
                    PerlinDataChunk perlinDataChunk = nextChunkContainer.ContainedChunk as PerlinDataChunk;

                    return perlinDataChunk.SummitArray[(int)(y / this.NoiseFrequency), 0];
                }

                isThereVector = false;
                return new Vector2f();
            }

            return this.SummitArray[(int)(y / this.NoiseFrequency), frequencyNoiseX];
        }

        private Vector2f GetBotRightVector(IDataChunkLayer parentLayer, int x, int y, out bool isThereVector)
        {
            isThereVector = true;
            int frequencyNoiseX = (int)(x / this.NoiseFrequency) + 1;
            int frequencyNoiseY = (int)(y / this.NoiseFrequency) + 1;

            int offsetX = 0;
            if (frequencyNoiseX >= this.nbSummitCase)
            {
                frequencyNoiseX = 0;
                offsetX = 1;
            }

            int offsetY = 0;
            if (frequencyNoiseY >= this.nbSummitCase)
            {
                frequencyNoiseY = 0;
                offsetY = 1;
            }

            ChunkContainer nextChunkContainer = (parentLayer as AExtendedDataChunkLayer).ExtendedChunksMonitor.GetChunkContainerAt(this.Position.X + offsetX, this.Position.Y + offsetY);

            if (nextChunkContainer != null && nextChunkContainer.ContainedChunk != null)
            {
                PerlinDataChunk perlinDataChunk = nextChunkContainer.ContainedChunk as PerlinDataChunk;

                return perlinDataChunk.SummitArray[frequencyNoiseY, frequencyNoiseX];
            }

            isThereVector = false;
            return new Vector2f();
        }

        public static float Fade(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);         // 6t^5 - 15t^4 + 10t^3
            //return (3 - 2 * t) * t * t;
        }
    }
}
