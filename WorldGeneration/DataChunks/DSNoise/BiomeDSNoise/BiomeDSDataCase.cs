using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.DSNoise.BiomeDSNoise
{
    internal class BiomeDSDataCase : ICase
    {
        public Vector2i Position
        {
            get;
            private set;
        }

        public float[] Value
        {
            get;
            set;
        }

        public int CurrentBiome
        {
            get;
            private set;
        }

        public float TotalValueSum
        {
            get;
            private set;
        }

        public BiomeDSDataCase(int nbBiome, int x, int y)
        {
            this.Position = new Vector2i(x, y);
            this.Value = new float[nbBiome];

            for (int i = 0; i < nbBiome; i++)
            {
                this.Value[i] = 0;
            }
            this.CurrentBiome = 0;
            this.TotalValueSum = 1;
        }

        public void UpdateCurrentBiome()
        {
            int nbBiome = this.Value.GetLength(0);
            int indexMax = 0;
            float valueMax = float.MinValue;
            this.TotalValueSum = 0;
            for (int i = 0; i < nbBiome; i++)
            {
                if(valueMax < this.Value[i])
                {
                    valueMax = this.Value[i];
                    indexMax = i;
                }
                this.TotalValueSum += this.Value[i];
            }

            this.CurrentBiome = indexMax;
        }
    }
}
