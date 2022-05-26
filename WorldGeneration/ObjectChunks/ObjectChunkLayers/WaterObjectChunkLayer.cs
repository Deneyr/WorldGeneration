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
                return true;
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
            int waterAltitude = this.altitudeObjectChunkLayer.GetWaterLevelAreaBufferValueAtLocal(localPosition.X, localPosition.Y);
            int groundAltitude = this.altitudeObjectChunkLayer.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

            int i = localPosition.Y + this.ObjectChunkMargin;
            int j = localPosition.X + this.ObjectChunkMargin;

            //this.AreaBuffer[localPosition.Y + this.ObjectChunkMargin, localPosition.X + this.ObjectChunkMargin] = waterAltitude;

            if (waterAltitude >= groundAltitude)
            {
                this.AreaBuffer[i, j] = waterAltitude;
            }
            else
            {
                this.AreaBuffer[i, j] = -1;
            }

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

        protected override void ComputeSecondBufferArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            int i = localPosition.Y + this.ObjectChunkMargin - 1;
            int j = localPosition.X + this.ObjectChunkMargin - 1;

            this.SecondAreaBuffer[i, j] = LandCreationHelper.NeedToFillWaterAt(this.AreaBuffer, localPosition.Y, localPosition.X, this.ObjectChunkMargin);
        }

        //protected override void ComputeSecondBufferArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        //{

        //}

        protected override void ComputeTransitionAreaBuffer(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            int waterLevel = this.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

            int i = localPosition.Y + this.ObjectChunkMargin - 2;
            int j = localPosition.X + this.ObjectChunkMargin - 2;

            this.TransitionAreaBuffer[i, j].Clear();

            if (waterLevel == -1)
            {              
                int[,] subAreaInt = new int[3, 3];
                int groundAltitude = this.altitudeObjectChunkLayer.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

                this.GetComputedWaterMatrix(localPosition.Y, localPosition.X, ref subAreaInt, out int minLocalAltitude, out int maxLocalAltitude);

                if(minLocalAltitude >= groundAltitude)
                {
                    this.GetComputedLandType(ref subAreaInt, maxLocalAltitude, out LandTransition landTransition);

                    if (landTransition != LandTransition.NONE)
                    {
                        this.TransitionAreaBuffer[i, j].Add(landTransition);
                    }
                }
            }


            //int[,] subAreaInt = new int[3, 3];

                //this.GetComputedWaterMatrix(localPosition.Y, localPosition.X, ref subAreaInt, out int minLocalAltitude, out int maxLocalAltitude);
                ////int groundAltitude = this.altitudeObjectChunkLayer.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

                //int i = localPosition.Y + this.ObjectChunkMargin - 2;
                //int j = localPosition.X + this.ObjectChunkMargin - 2;

                //this.MaxValueAreaBuffer[i, j] = -1;

                //if (subAreaInt[1, 1] != maxLocalAltitude)
                //{
                //    if (subAreaInt[1, 1] == -1)
                //    {
                //        subAreaInt[1, 1] = minLocalAltitude - 1;
                //    }

                //    int diffAltitude = maxLocalAltitude - subAreaInt[1, 1];
                //    //this.SecondAreaBuffer[i, j] = maxLocalAltitude;

                //    this.MaxValueAreaBuffer[i, j] = subAreaInt[1, 1] + 1;

                //    this.TransitionAreaBuffer[i, j].Clear();

                //    if (this.GenerateAllLevels == false)
                //    {
                //        diffAltitude = Math.Min(diffAltitude, 1);
                //    }

                //    for (int offset = 0; offset < diffAltitude; offset++)
                //    {
                //        this.GetComputedLandType(ref subAreaInt, maxLocalAltitude, out LandTransition landTransition);

                //        this.TransitionAreaBuffer[i, j].Add(landTransition);

                //        subAreaInt[1, 1]++;
                //    }
                //}
        }

        protected void GetComputedWaterMatrix(int i, int j, ref int[,] subAreaInt, out int minValue, out int maxValue)
        {
            maxValue = int.MinValue;
            minValue = -1;

            int secondObjectChunkMargin = this.ObjectChunkMargin - 1;

            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int altitude = this.SecondAreaBuffer[i + secondObjectChunkMargin + y, j + secondObjectChunkMargin + x];

                    maxValue = Math.Max(maxValue, altitude);

                    if (altitude != -1)
                    {
                        if (minValue == -1)
                        {
                            minValue = altitude;
                        }
                        else
                        {
                            minValue = Math.Min(minValue, altitude);
                        }
                    }

                    subAreaInt[y + 1, x + 1] = altitude;
                }
            }
        }

        protected override void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            //bool isThereWater = this.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y) == 1 ? true : false;

            //if (isThereWater)
            //{
            int waterAltitude = this.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

            if (waterAltitude >= 0)
            {
                int groundAltitude = this.altitudeObjectChunkLayer.GetSecondAreaBufferValueAtLocal(localPosition.X, localPosition.Y);

                IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

                int currentAltitude = groundAltitude;
                int diffWaterAltitude = waterAltitude - groundAltitude + 1;

                //if(diffWaterAltitude <= 0)
                //{
                //    Console.WriteLine();
                //}

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
