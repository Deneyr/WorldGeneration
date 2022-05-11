using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.ObjectChunks.BiomeManager;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class FloraCObjectChunkLayer: AObjectChunkLayer
    {
        public FloraCObjectChunkLayer(string id)
            : base(id)
        {
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            int chunkSeed = this.GenerateChunkSeed(objectChunk, objectChunksMonitor.WorldSeed);
            Random random = new Random(chunkSeed);

            FloraDataAgreggator floraDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["flora"] as FloraDataAgreggator);
            FloraRatioBiomeManager floraRatioManager = objectChunksMonitor.DataChunkMonitor.FloraRatioManager;

            for (int i = 0; i < objectChunk.NbCaseSide; i++)
            {
                for (int j = 0; j < objectChunk.NbCaseSide; j++)
                {
                    IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(j, i) as IZObjectCase;

                    if (zObjectCase.GroundAltitude >= 0)
                    {
                        ObjectCase objectCase = zObjectCase[zObjectCase.GroundAltitude] as ObjectCase;

                        if (objectCase.IsUnderSea == false)
                        {
                            objectCase.IsThereTree = floraDataAgreggator.IsThereTreeAtWorldCoordinate(zObjectCase.Position.X, zObjectCase.Position.Y, floraRatioManager.GetTreeRatioFromBiomeAltitude(zObjectCase.ObjectBiome, objectCase.Altitude));
                            objectCase.IsThereRock = floraDataAgreggator.IsThereRockAtWorldCoordinate(zObjectCase.Position.X, zObjectCase.Position.Y, floraRatioManager.GetRockRatioFromBiomeAltitude(zObjectCase.ObjectBiome, objectCase.Altitude));
                        }
                    }
                }
            }
        }
    }
}
