using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DataAgreggator;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class BiomeObjectChunkLayer : AObjectChunkLayer
    {
        public BiomeObjectChunkLayer(string id)
            : base(id)
        {
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            int chunkSeed = this.GenerateChunkSeed(objectChunk, objectChunksMonitor.WorldSeed);
            Random random = new Random(chunkSeed);

            BiomeDataAgreggator biomeDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["biome"] as BiomeDataAgreggator);

            for (int i = 0; i < objectChunk.NbCaseSide; i++)
            {
                for (int j = 0; j < objectChunk.NbCaseSide; j++)
                {
                    IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(j, i) as IZObjectCase;

                    zObjectCase.ObjectBiome = biomeDataAgreggator.GetBiomeAtWorldCoordinates(zObjectCase.Position.X, zObjectCase.Position.Y, out float borderValue);
                }
            }
        }
    }
}
