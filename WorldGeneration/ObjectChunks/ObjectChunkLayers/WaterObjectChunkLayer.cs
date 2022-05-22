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
    internal class WaterObjectChunkLayer : A2PassObjectChunkLayer
    {
        private AltitudeDataAgreggator altitudeDataAgreggator;

        private AltitudeObjectChunkLayer altitudeObjectChunkLayer;

        public override bool GenerateAllLevels
        {
            get
            {
                return false;
            }
        }

        public WaterObjectChunkLayer(string id)
            : base(id)
        {
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            this.altitudeDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["altitude"] as AltitudeDataAgreggator);
            this.altitudeObjectChunkLayer = (objectChunksMonitor.ObjectChunksLayers["altitudeLayer"] as AltitudeObjectChunkLayer);


            base.ComputeObjectChunk(objectChunksMonitor, objectChunk);
        }


        protected override void ComputeBufferArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            //int waterAltitude = this.altitudeObjectChunkLayer.GetWaterLevelAreaBufferValueAtLocal(localPosition.X, localPosition.Y);
            //int groundAltitude = this.altitudeObjectChunkLayer.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

            //this.AreaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin] = waterAltitude >= groundAltitude ? 1 : 0;

            //bool isThereWater = this.altitudeObjectChunkLayer.GetWaterAreaBufferValueAtLocal(localPosition.X, localPosition.Y);
            //int initialAltitudeLevel = this.altitudeObjectChunkLayer.GetInitialAltitudeAreaBufferValueAtLocal(localPosition.X, localPosition.Y);
            //IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;
            //ObjectCase objectCase = zObjectCase[zObjectCase.GroundAltitude] as ObjectCase;

            //int refAltitudeLevel = Math.Max(this.altitudeDataAgreggator.SeaLevel, initialAltitudeLevel);
            //int levelsToFills = Math.Max(0, refAltitudeLevel - zObjectCase.GroundAltitude);
            //int result = 0;
            //if (isThereWater && levelsToFills >= 0 && objectCase.Land.LandWall == null)
            //{
            //    result = 1;
            //}

            //this.AreaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin] = result;
        }

        //protected override void ComputeSecondBufferArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        //{

        //}

        protected override void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            //bool isThereWater = this.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y) == 1 ? true : false;

            //if (isThereWater)
            //{
            int groundAltitude = this.altitudeObjectChunkLayer.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);
            int waterAltitude = this.altitudeObjectChunkLayer.GetWaterLevelAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

            if (waterAltitude >= 0)
            {
                IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

                int currentAltitude = groundAltitude;
                int diffWaterAltitude = waterAltitude - groundAltitude + 1;

                for (int i = 0; i < diffWaterAltitude; i++)
                {
                    ObjectCase objectCase = zObjectCase[currentAltitude] as ObjectCase;

                    if (objectCase == null)
                    {
                        objectCase = new ObjectCase(zObjectCase.Position, currentAltitude);
                        zObjectCase.SetCaseAt(objectCase);
                    }

                    objectCase.Land.LandWater = new WaterLandObject(random.Next());
                    if (objectCase.Land.LandWall != null)
                    {
                        objectCase.Land.LandWater.LandTransition = LandTransitionHelper.ReverseLandTransition(objectCase.Land.LandWall.LandTransition);
                    }

                    currentAltitude++;
                }
            }
            //}

            //IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

            //int computedAltitude = LandCreationHelper.NeedToFillAt(this.AreaBuffer, localPosition.Y, localPosition.X, this.ObjectChunkMargin);

            //ObjectCase objectCase = new ObjectCase(zObjectCase.Position, computedAltitude);

            //LandType landType = this.GetAltitudeLandType(zObjectCase.ObjectBiome, objectCase.Altitude);
            //GroundLandObject groundLandObject = BiomeObjectChunkLayer.CreateGroundLandObject(zObjectCase.ObjectBiome, landType);
            //objectCase.Land.AddLandGround(groundLandObject);

            //zObjectCase.SetCaseAt(objectCase);
        }

        //public override void InitObjectChunkLayer(int nbCaseSide)
        //{
        //    base.InitObjectChunkLayer(nbCaseSide);

        //    int caseSideExtended = nbCaseSide + this.ObjectChunkMargin * 2;
        //    this.InitialAltitudeAreaBuffer = new int[caseSideExtended, caseSideExtended];
        //    this.WaterAreaBuffer = new bool[caseSideExtended, caseSideExtended];
        //}
    }
}
