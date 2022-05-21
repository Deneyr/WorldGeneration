using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DataAgreggator;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.WaterObject;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class WaterTransitionObjectChunkLayer : AObjectChunkLayer
    {
        private AltitudeDataAgreggator altitudeDataAgreggator;

        private WaterObjectChunkLayer waterObjectChunkLayer;


        public WaterTransitionObjectChunkLayer(string id)
            : base(id)
        {
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            this.altitudeDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["altitude"] as AltitudeDataAgreggator);

            this.waterObjectChunkLayer = (objectChunksMonitor.ObjectChunksLayers["waterLayer"] as WaterObjectChunkLayer);

            base.ComputeObjectChunk(objectChunksMonitor, objectChunk);
        }

        protected override void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            List<LandTransition> landTransitions = this.waterObjectChunkLayer.GetLandTransitionAtLocal(localPosition.X, localPosition.Y);

            if (landTransitions.Any())
            {
                IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

                LandCase landCase = (zObjectCase[zObjectCase.GroundAltitude] as ObjectCase).Land;
                if(zObjectCase.GroundAltitude == this.altitudeDataAgreggator.SeaLevel)
                {
                    landCase.LandWater = new WaterLandObject();
                    landCase.LandWater.LandTransition = landTransitions.First();
                }
            }
        }
    }
}
