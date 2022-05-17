using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class BiomeTransitionObjectChunkLayer : AObjectChunkLayer
    {
        BiomeObjectChunkLayer biomeObjectChunkLayer;

        public BiomeTransitionObjectChunkLayer(string id)
            : base(id)
        {
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            this.biomeObjectChunkLayer = (objectChunksMonitor.ObjectChunksLayers["biomeLayer"] as BiomeObjectChunkLayer);

            base.ComputeObjectChunk(objectChunksMonitor, objectChunk);
        }

        protected override void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

            List<LandTransition> landTransitions = this.biomeObjectChunkLayer.GetLandTransitionAtLocal(localPosition.X, localPosition.Y);
            LandCase landCase = (zObjectCase[zObjectCase.GroundAltitude] as ObjectCase).Land;
            BiomeType secondBiomeType = (BiomeType)this.biomeObjectChunkLayer.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

            if (landTransitions.Any())
            {
                GroundLandObject groundLandObject = landCase.LandGroundList.First() as GroundLandObject;

                GroundLandObject newLandObject = BiomeObjectChunkLayer.CreateGroundLandObject(secondBiomeType, groundLandObject.Type);
                newLandObject.LandTransition = landTransitions.First();

                landCase.LandGroundList.Add(newLandObject);
            }
        }
    }
}
