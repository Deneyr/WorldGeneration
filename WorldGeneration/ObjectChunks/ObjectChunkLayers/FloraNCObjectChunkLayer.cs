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
        private FloraDataAgreggator floraDataAgreggator;

        public FloraNCObjectChunkLayer(string id)
            : base(id)
        {
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            this.floraDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["flora"] as FloraDataAgreggator);

            base.ComputeObjectChunk(objectChunksMonitor, objectChunk);
        }

        protected override void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            FloraRatioBiomeManager floraRatioManager = objectChunksMonitor.DataChunkMonitor.FloraRatioManager;
            IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

            if (zObjectCase.GroundAltitude >= 0)
            {
                ObjectCase objectCase = zObjectCase[zObjectCase.GroundAltitude] as ObjectCase;

                if (objectCase.IsUnderSea == false)
                {
                    if (objectCase.IsThereTree == false
                        && objectCase.IsThereRock == false
                        && objectCase.IsThereTallGrass == false)
                    {
                        objectCase.IsThereFlower = this.floraDataAgreggator.IsThereFlowerAtWorldCoordinate(zObjectCase.Position.X, zObjectCase.Position.Y, floraRatioManager.GetVegetationRatioFromBiomeAltitude(zObjectCase.ObjectBiome, objectCase.Altitude));
                    }
                }
            }
        }
    }
}
