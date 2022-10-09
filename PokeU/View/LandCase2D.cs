using PokeU.View.GroundObject;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks;
using WorldGeneration.ObjectChunks.ObjectLands;

namespace PokeU.View
{
    public class LandCase2D : AObject2D
    {
        private List<ILandObject2D> landGroundOverWallList;

        private ILandObject2D landWater;

        private ILandObject2D landOverWall;

        private ILandObject2D landWall;

        private ILandObject2D landOverGround;

        private List<ILandObject2D> landGroundList;

        private LandCaseData underLandCaseData;
        private LandCaseData overLandCaseData;

        public LandCase2D(LandWorld2D landWorld2D, IObjectChunk parentObjectChunk, LandCase landCase, Vector2i position)
        {
            this.landGroundOverWallList = new List<ILandObject2D>();

            this.landWater = null;

            this.landOverWall = null;

            this.landWall = null;

            this.landOverGround = null;

            this.landGroundList = new List<ILandObject2D>();

            this.underLandCaseData = new LandCaseData(false, false);
            this.overLandCaseData = new LandCaseData(false, false);

            AGroundObject2DFactory groundObject2DFactory;

            foreach (ILandObject landGroundObject in landCase.LandGroundList)
            {
                groundObject2DFactory = LandWorld2D.MappingObjectModelView[landGroundObject.GetType()] as AGroundObject2DFactory;
                groundObject2DFactory.IsWall = false;

                groundObject2DFactory.CurrentObjectChunk = parentObjectChunk;

                ILandObject2D landObject2D = groundObject2DFactory.CreateObject2D(landWorld2D, landGroundObject, position) as ILandObject2D;

                this.landGroundList.Add(landObject2D);
            }

            if (landCase.LandOverGround != null)
            {
                IObject2DFactory object2DFactory = LandWorld2D.MappingObjectModelView[landCase.LandOverGround.GetType()];

                object2DFactory.CurrentObjectChunk = parentObjectChunk;

                ILandObject2D landObject2D = object2DFactory.CreateObject2D(landWorld2D, landCase.LandOverGround, position) as ILandObject2D;

                this.landOverGround = landObject2D;
            }

            if (landCase.LandWall != null)
            {
                foreach (ILandObject landGroundOverWallObject in landCase.LandGroundOverWallList)
                {
                    groundObject2DFactory = LandWorld2D.MappingObjectModelView[landGroundOverWallObject.GetType()] as AGroundObject2DFactory;
                    groundObject2DFactory.IsWall = false;

                    groundObject2DFactory.CurrentObjectChunk = parentObjectChunk;

                    ILandObject2D landObject2D = groundObject2DFactory.CreateObject2D(landWorld2D, landGroundOverWallObject, position) as ILandObject2D;

                    this.landGroundOverWallList.Add(landObject2D);
                }
            }

            if (landCase.LandWater != null)
            {
                IObject2DFactory object2DFactory = LandWorld2D.MappingObjectModelView[landCase.LandWater.GetType()];

                object2DFactory.CurrentObjectChunk = parentObjectChunk;

                ILandObject2D landObject2D = object2DFactory.CreateObject2D(landWorld2D, landCase.LandWater, position) as ILandObject2D;

                this.landWater = landObject2D;
            }

            if (landCase.LandWall != null)
            {
                groundObject2DFactory = LandWorld2D.MappingObjectModelView[landCase.LandWall.GetType()] as AGroundObject2DFactory;
                groundObject2DFactory.IsWall = true;

                groundObject2DFactory.CurrentObjectChunk = parentObjectChunk;

                ILandObject2D landObject2D = groundObject2DFactory.CreateObject2D(landWorld2D, landCase.LandWall, position) as ILandObject2D;

                this.landWall = landObject2D;
            }

            if (landCase.LandOverWall != null)
            {
                IObject2DFactory object2DFactory = LandWorld2D.MappingObjectModelView[landCase.LandOverWall.GetType()];

                object2DFactory.CurrentObjectChunk = parentObjectChunk;

                ILandObject2D landObject2D = object2DFactory.CreateObject2D(landWorld2D, landCase.LandOverWall, position) as ILandObject2D;

                this.landOverWall = landObject2D;
            }
        }

        public void UpdateUnderLandCase(LandCase underLandCase)
        {
            if (underLandCase != null)
            {
                this.underLandCaseData = new LandCaseData(underLandCase.LandWall != null, underLandCase.LandWater != null); // && underLandCase.LandWater.LandTransition == LandTransition.NONE);
            }
        }

        public void UpdateOverLandCase(LandCase overLandeCase)
        {
            if (overLandeCase != null)
            {
                this.overLandCaseData = new LandCaseData(overLandeCase.LandWall != null, overLandeCase.LandWater != null); // && overLandeCase.LandWater.LandTransition == LandTransition.NONE);
            }
        }

        public void SetLandCaseRatio(int level, int maxLevel)
        {
            this.RatioAltitude = Math.Min(1, Math.Max(-1, ((float) level) / maxLevel));
            float ratioUp = Math.Min(1, Math.Max(-1, ((float)level + 1) / maxLevel));

            foreach (ILandObject2D landGroundObject in this.landGroundList)
            {
                landGroundObject.RatioAltitude = this.RatioAltitude;
            }

            if (this.landOverGround != null)
            {
                this.landOverGround.RatioAltitude = this.RatioAltitude;
            }

            foreach (ILandObject2D landGroundOverWallObject in this.landGroundOverWallList)
            {
                landGroundOverWallObject.RatioAltitude = ratioUp;
            }

            if (this.landWater != null)
            {
                this.landWater.RatioAltitude = this.RatioAltitude;
            }

            if (this.landWall != null)
            {
                this.landWall.RatioAltitude = this.RatioAltitude;
            }

            if (this.landOverWall != null)
            {
                this.landOverWall.RatioAltitude = ratioUp;
            }
        }

        public bool IsValid
        {
            get
            {
                if (this.landWater != null
                    || this.landWall != null)
                {
                    return true;
                }

                if (this.landGroundList.Count > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public override void DrawIn(RenderWindow window, ref FloatRect boundsView)
        {
            if (this.IsValid)
            {
                if (this.underLandCaseData.IsThereWall == false)
                {
                    foreach (ILandObject2D landGroundObject in this.landGroundList)
                    {
                        //landGroundObject.RatioAltitude = this.RatioAltitude;

                        landGroundObject.DrawIn(window, ref boundsView);
                    }
                }

                if (this.landOverGround != null)
                {
                    this.landOverGround.DrawIn(window, ref boundsView);
                }

                if (this.landWall != null)
                {
                    foreach (ILandObject2D landGroundOverWallObject in this.landGroundOverWallList)
                    {
                        landGroundOverWallObject.DrawIn(window, ref boundsView);
                    }
                }

                //if (this.overLandCaseData.IsThereWater == false || this.RatioAltitude == 0)
                //{
                //    if (this.landWater != null)
                //    {
                //        this.landWater.DrawIn(window, ref boundsView);
                //    }
                //}

                if (this.landWall != null)
                {
                    this.landWall.DrawIn(window, ref boundsView);
                }

                if (this.overLandCaseData.IsThereWater == false)
                {
                    if (this.landWater != null)
                    {
                        this.landWater.DrawIn(window, ref boundsView);
                    }
                }

                if (this.landOverWall != null)
                {
                    this.landOverWall.DrawIn(window, ref boundsView);
                }
            }
        }

        public override void Dispose()
        {
            foreach (ILandObject2D landGroundObject in this.landGroundList)
            {
                landGroundObject.Dispose();
            }

            if (this.landOverGround != null)
            {
                this.landOverGround.Dispose();
            }

            foreach (ILandObject2D landGroundOverWallObject in this.landGroundOverWallList)
            {
                landGroundOverWallObject.Dispose();
            }

            if (this.landWater != null)
            {
                this.landWater.Dispose();
            }

            if (this.landWall != null)
            {
                this.landWall.Dispose();
            }

            if (this.landOverWall != null)
            {
                this.landOverWall.Dispose();
            }

            base.Dispose();
        }


        public struct LandCaseData
        {
            public LandCaseData(bool isThereWall, bool isThereWater)
            {
                this.IsThereWall = isThereWall;

                this.IsThereWater = isThereWater;
            }

            public bool IsThereWall
            {
                get;
                private set;
            }

            public bool IsThereWater
            {
                get;
                private set;
            }
        }
    }
}
