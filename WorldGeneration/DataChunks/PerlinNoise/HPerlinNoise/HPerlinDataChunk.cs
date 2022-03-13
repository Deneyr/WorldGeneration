using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace WorldGeneration.DataChunks.PerlinNoise.HPerlinNoise
{
    internal class HPerlinDataChunk: PerlinDataChunk
    {
        public HPerlinDataChunk(Vector2i position, int nbCaseSide, int noiseFrequency, int sampleLevel) 
            : base(position, nbCaseSide, noiseFrequency, sampleLevel)
        {
        }

        public override void GenerateChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            base.GenerateChunk(dataChunksMonitor, parentLayer);

            int densitySize = 255;
            float[] densityFunction = new float[densitySize];
            //float[] newDensityFunction = new float[densitySize];
            float[] distributionFunction = new float[densitySize];

            for (int i = 0; i < densitySize; i++)
            {
                densityFunction[i] = 0;
            }
            int minIndex = int.MaxValue;
            int maxIndex = int.MinValue;
            for (int i = 0; i < this.realNbCaseSide; i++)
            {
                for (int j = 0; j < this.realNbCaseSide; j++)
                {
                    int value = (int)((((this.casesArray[i, j] as PerlinDataCase).Value + 1) / 2) * densitySize);
                    densityFunction[value]++;

                    if(minIndex > value)
                    {
                        minIndex = value;
                    }

                    if (maxIndex < value)
                    {
                        maxIndex = value;
                    }
                }
            }

            float nbValue = 1 + this.realNbCaseSide * this.realNbCaseSide;
            int spectrumValue = maxIndex - minIndex + 1;
            //float ratioValue = ((float) (spectrumValue)) / densitySize;

            float cumulativeValue = 0;
            for (int i = minIndex; i < minIndex + spectrumValue; i++)
            {
                distributionFunction[i] = densityFunction[i] / nbValue + cumulativeValue;
                cumulativeValue = distributionFunction[i];
            }

            for (int i = 0; i < this.realNbCaseSide; i++)
            {
                for (int j = 0; j < this.realNbCaseSide; j++)
                {
                    PerlinDataCase perlinCase = this.casesArray[i, j] as PerlinDataCase;
                    float scaledValue = (perlinCase.Value + 1) / 2;
                    int value = (int)(scaledValue * densitySize);
                    perlinCase.Value = scaledValue * 0.3f + distributionFunction[value] * 0.7f;
                }
            }
        }
    }
}
