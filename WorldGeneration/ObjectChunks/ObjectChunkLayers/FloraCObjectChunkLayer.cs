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
    internal class FloraCObjectChunkLayer: AObjectChunkLayer
    {
        private FloraDataAgreggator floraDataAgreggator;

        public FloraCObjectChunkLayer(string id)
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
                    objectCase.IsThereTree = this.floraDataAgreggator.IsThereTreeAtWorldCoordinate(zObjectCase.Position.X, zObjectCase.Position.Y, floraRatioManager.GetTreeRatioFromBiomeAltitude(zObjectCase.ObjectBiome, objectCase.Altitude));
                    objectCase.IsThereRock = this.floraDataAgreggator.IsThereRockAtWorldCoordinate(zObjectCase.Position.X, zObjectCase.Position.Y, floraRatioManager.GetRockRatioFromBiomeAltitude(zObjectCase.ObjectBiome, objectCase.Altitude));
                }
            }
        }
    }
}
