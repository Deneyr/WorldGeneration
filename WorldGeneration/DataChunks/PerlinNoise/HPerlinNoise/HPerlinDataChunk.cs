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

        //public static float[] meanNormalLaw = new float[255];
        //public static int nbSample = 0;

        private static readonly int DENSITY_SIZE = 255;

        private static float[] meanDistributive = new float[255]
        {
            0,
            0,
            0,
            0,

            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,

            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,

            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,

            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,


            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,

            0,
0.0000075f,
0.0003889007f,
0.0014240787f,
0.0028252488f,
0.00450853f,
0.006360257f,
0.0081283f,
0.0105290851f,
0.0125143453f,
0.01488729f,
0.0174825266f,
0.0203498621f,
0.0235789772f,
0.027970925f,
0.03259143f,
0.037211787f,
0.0420057029f,
0.04692882f,
0.0518219844f,
0.0571637079f,
0.06263256f,
0.0680251f,
0.073700875f,
0.0802096054f,
0.08602081f,
0.0923654f,
0.09873263f,
0.105065808f,
0.111201562f,
0.117779717f,
0.124327846f,
0.130905941f,
0.1377311f,
0.1442835f,
0.15169692f,
0.158469722f,
0.165902883f,
0.173961282f,
0.1820682f,
0.190459356f,
0.2005806f,
0.2106141f,
0.220095843f,
0.230069384f,
0.241009578f,
0.252598941f,
0.263926983f,
0.27555573f,
0.287605971f,
0.29937088f,
0.3108703f,
0.322807759f,
0.334408581f,
0.358033538f,
0.358033538f,
0.369676739f,
0.3815587f,
0.393694758f,
0.40597564f,
0.417141378f,
0.429276526f,
0.441685528f,
0.4545639f,
0.4664833f,
0.479103118f,
0.492431253f,
0.5053985f,
0.518583f,
0.5318315f,
0.5444093f,
0.5577531f,
0.571218133f,
0.5844662f,
0.597919941f,
0.6107846f,
0.6072864f,
0.6204187f,
0.6337105f,
0.6517105f,
0.6680741364f,
0.6830741364f,
0.6969202902f,
0.7097774331f,
0.7217774331f,
0.7330274331f,
0.7436156684f,
0.7536156684f,
0.7630893526f,
0.7720893526f,
0.7806607811f,
0.7888425993f,
0.7966686863f,
0.8041686863f,
0.8113686863f,
0.8182917632f,
0.8249584299f,
0.8313870013f,
0.8375938979f,
0.8435938979f,
0.8494003495f,
0.8550253495f,
0.8604798949f,
0.8657740126f,
0.8709168697f,
0.8759168697f,
0.8807817346f,
0.8855185767f,
0.8901339613f,
0.8946339613f,
0.8990242052f,
0.9033099195f,
0.907495966f,
0.9115868751f,
0.9155868751f,
0.9194999186f,
0.9233297058f,
0.9270797058f,
0.9307531752f,
0.9343531752f,
0.9378825869f,
0.9413441254f,
0.9447403518f,
0.9480736852f,
0.9513464124f,
0.9545606981f,
0.9577185929f,
0.9608220412f,
0.9638728886f,
0.9668728886f,
0.9698237083f,
0.9727269341f,
0.975584077f,
0.978396577f,
0.9811658077f,
0.9814658077f,
0.9814858077f,
0.981558077f,
0.9816658077f,
0.9817658077f,
0.9818658077f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,

0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,

0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,

0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,

0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,

0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
0.99f,
        };

        //static HPerlinDataChunk()
        //{
        //    for (int i = 0; i < 255; i++)
        //    {
        //        meanDistributive[i] = 0;
        //    }
        //}

        //private void UpdateMeanDistribution(float[] newSample)
        //{
        //    for (int i = 0; i < 255; i++)
        //    {
        //        meanNormalLaw[i] *= nbSample;
        //    }

        //    nbSample++;

        //    for (int i = 0; i < 255; i++)
        //    {
        //        meanNormalLaw[i] += newSample[i];
        //        meanNormalLaw[i] /= nbSample;
        //    }
        //}

        public HPerlinDataChunk(Vector2i position, int nbCaseSide, int noiseFrequency, int sampleLevel) 
            : base(position, nbCaseSide, noiseFrequency, sampleLevel)
        {
        }

        public override void GenerateChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            base.GenerateChunk(dataChunksMonitor, parentLayer);

            //int densitySize = 255;
            //float[] densityFunction = new float[densitySize];
            ////float[] newDensityFunction = new float[densitySize];
            //float[] distributionFunction = new float[densitySize];

            //for (int i = 0; i < densitySize; i++)
            //{
            //    densityFunction[i] = 0;
            //}
            //int minIndex = int.MaxValue;
            //int maxIndex = int.MinValue;
            //for (int i = 0; i < this.realNbCaseSide; i++)
            //{
            //    for (int j = 0; j < this.realNbCaseSide; j++)
            //    {
            //        int value = (int)((((this.casesArray[i, j] as PerlinDataCase).Value + 1) / 2) * densitySize);
            //        densityFunction[value]++;

            //        if(minIndex > value)
            //        {
            //            minIndex = value;
            //        }

            //        if (maxIndex < value)
            //        {
            //            maxIndex = value;
            //        }
            //    }
            //}

            //float nbValue = 1 + this.realNbCaseSide * this.realNbCaseSide;
            //int spectrumValue = maxIndex - minIndex + 1;
            ////float ratioValue = ((float) (spectrumValue)) / densitySize;

            //float cumulativeValue = 0;
            //for (int i = minIndex; i < minIndex + spectrumValue; i++)
            //{
            //    distributionFunction[i] = densityFunction[i] / nbValue + cumulativeValue;
            //    cumulativeValue = distributionFunction[i];
            //}

            //float[] normalLaw = new float[255];
            //for (int i = 0; i < densitySize; i++)
            //{
            //    normalLaw[i] = 0;
            //}


            for (int i = 0; i < this.realNbCaseSide; i++)
            {
                for (int j = 0; j < this.realNbCaseSide; j++)
                {
                    PerlinDataCase perlinCase = this.casesArray[i, j] as PerlinDataCase;
                    float scaledValue = (perlinCase.Value + 1) / 2;
                    int value = (int)(scaledValue * DENSITY_SIZE);

                    //int newValue = (int)(meanDistributive[value] * 255);

                    perlinCase.Value = meanDistributive[value] * 0.8f + scaledValue * 0.2f;

                    //normalLaw[newValue]++;
                }
            }

            //this.UpdateMeanDistribution(normalLaw);
        }
    }
}
