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

        private AltitudeObjectChunkLayer altitudeObjectChunkLayer;

        private WaterObjectChunkLayer waterObjectChunkLayer;

        public WaterTransitionObjectChunkLayer(string id)
            : base(id)
        {
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            this.altitudeDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["altitude"] as AltitudeDataAgreggator);

            this.altitudeObjectChunkLayer = (objectChunksMonitor.ObjectChunksLayers["altitudeLayer"] as AltitudeObjectChunkLayer);
            this.waterObjectChunkLayer = (objectChunksMonitor.ObjectChunksLayers["waterLayer"] as WaterObjectChunkLayer);

            base.ComputeObjectChunk(objectChunksMonitor, objectChunk);
        }

        protected override void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            List<LandTransition> landTransitions = this.waterObjectChunkLayer.GetLandTransitionAtLocal(localPosition.X, localPosition.Y);

            if (landTransitions.Any())
            {
                int waterAltitude = this.waterObjectChunkLayer.GetMaxAreaBufferValueAtLocal(localPosition.X, localPosition.Y);
                int groundAltitude = this.altitudeObjectChunkLayer.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

                if (groundAltitude <= waterAltitude)
                {
                    IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

                    //LandCase landCase = (zObjectCase[zObjectCase.GroundAltitude] as ObjectCase).Land;
                    //if (zObjectCase.GroundAltitude == this.altitudeDataAgreggator.SeaLevel)
                    //{
                    //    landCase.LandWater = new WaterLandObject(random.Next());
                    //    landCase.LandWater.LandTransition = landTransitions.First();
                    //}

                    ObjectCase objectCase = zObjectCase[groundAltitude] as ObjectCase;

                    if (objectCase.Land.LandWater == null && objectCase.Land.IsValid)
                    {
                        objectCase.Land.LandWater = new WaterLandObject(random.Next());
                        objectCase.Land.LandWater.LandTransition = landTransitions.First();
                    }

                    if (objectCase.Land.LandWater == null && objectCase.Land.IsValid)
                    {

                        //int currentAltitude = groundAltitude;
                        //int diffWaterAltitude = waterAltitude - groundAltitude + 1;

                        //for (int i = 0; i < diffWaterAltitude; i++)
                        //{
                        //    ObjectCase objectCase = zObjectCase[currentAltitude] as ObjectCase;

                        //    if (objectCase == null)
                        //    {
                        //        objectCase = new ObjectCase(zObjectCase.Position, currentAltitude);
                        //        zObjectCase.SetCaseAt(objectCase);
                        //    }

                        //    if (objectCase.Land.LandWater == null && objectCase.Land.IsValid)
                        //    {
                        //        objectCase.Land.LandWater = new WaterLandObject(random.Next());
                        //        if (objectCase.Land.LandWall != null)
                        //        {
                        //            objectCase.Land.LandWater.LandTransition = LandTransitionHelper.ReverseLandTransition(objectCase.Land.LandWall.LandTransition);
                        //        }
                        //        else
                        //        {
                        //            objectCase.Land.LandWater.LandTransition = landTransitions.First();
                        //        }
                        //    }

                        //    currentAltitude++;
                        //}
                    }
                }
            }
        }
    }
}
