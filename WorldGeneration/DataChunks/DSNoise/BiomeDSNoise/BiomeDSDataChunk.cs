using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.DataChunks.DSNoise.BiomeDSNoise
{
    internal class BiomeDSDataChunk : DSDataChunk
    {
        public int NbBiome
        {
            get;
            private set;
        }

        public BiomeDSDataChunk(Vector2i position, int nbCaseSide, int nbBiome) 
            : base(position, nbCaseSide)
        {
            this.NbBiome = nbBiome;
        }

        public override void PrepareChunk(DataChunkLayersMonitor dataChunksMonitor, IDataChunkLayer parentLayer)
        {
            int chunkSeed = this.GenerateChunkSeed(dataChunksMonitor.WorldSeed + parentLayer.Id.GetHashCode());
            Random random = new Random(chunkSeed);

            BiomeDSDataCase generatedCase = new BiomeDSDataCase(this.NbBiome, 0, 0);

            //generatedCase.Value[random.Next(0, this.NbBiome)] = 0.5f;

            for (int i = 0; i < this.NbBiome; i++)
            {
                generatedCase.Value[i] = (float)(random.NextDouble() - 0.5f);
            }

            //generatedCase.UpdateCurrentBiome();

            this.CasesArray[0, 0] = generatedCase;
        }

        protected override ICase GenerateCaseFrom(IDataChunkLayer parentLayer, int x, int y, ICase topLeftCase, ICase topRightCase, ICase botLeftCase, ICase botRightCase, int valueGenerated)
        {
            Random random = new Random(valueGenerated);

            BiomeDSDataCase generatedCase = new BiomeDSDataCase(this.NbBiome, 0, 0);

            float ratioValueGenerated;

            for (int i = 0; i < this.NbBiome; i++)
            {
                ratioValueGenerated = (float)((random.NextDouble() * 2) - 1);

                generatedCase.Value[i] = ((topLeftCase as BiomeDSDataCase).Value[i] + (topRightCase as BiomeDSDataCase).Value[i] + (botLeftCase as BiomeDSDataCase).Value[i] + (botRightCase as BiomeDSDataCase).Value[i]) / 4
                + ratioValueGenerated * this.GetCurrentAddingRatio((parentLayer as DSDataChunkLayer).CurrentNbStep);
            }

            //generatedCase.UpdateCurrentBiome();

            return generatedCase;
        }
    }
}
