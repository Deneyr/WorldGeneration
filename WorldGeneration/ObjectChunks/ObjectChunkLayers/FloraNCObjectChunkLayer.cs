using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.ObjectChunks.BiomeManager;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class FloraNCObjectChunkLayer: AObjectChunkLayer
    {
        public FloraNCObjectChunkLayer(string id)
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
                            if (objectCase.IsThereTree == false 
                                && objectCase.IsThereRock == false
                                && objectCase.IsThereTallGrass == false)
                            {
                                objectCase.IsThereFlower = floraDataAgreggator.IsThereFlowerAtWorldCoordinate(zObjectCase.Position.X, zObjectCase.Position.Y, floraRatioManager.GetVegetationRatioFromBiomeAltitude(zObjectCase.ObjectBiome, objectCase.Altitude));
                            }
                        }
                    }
                }
            }
        }

        protected override void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            //IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

            //zObjectCase.ObjectBiome = (BiomeType)this.areaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin];
        }
    }
}
