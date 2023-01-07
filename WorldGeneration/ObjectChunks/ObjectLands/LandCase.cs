using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectLands
{
    public class LandCase
    {
        private List<ILandGround> landGroundOverWallList;

        private ILandWater landWater;

        private ILandOverGround landOverWall;

        private ILandWall landWall;

        private ILandOverGround landOverGround;

        private List<ILandGround> landGroundList;

        public bool IsValid
        {
            get
            {
                if(this.landWater != null 
                    || this.landWall != null)
                {
                    return true;
                }

                if(this.landGroundList.Count > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool IsOnlyWater
        {
            get
            {
                if(this.landWall != null
                    || this.landGroundList.Count > 0
                    || this.landGroundOverWallList.Count > 0)
                {
                    return false;
                }

                return this.landWater != null;
            }
        }

        public List<ILandGround> LandGroundOverWallList
        {
            get
            {
                return this.landGroundOverWallList;
            }
            internal set
            {
                this.landGroundOverWallList = value;
            }
        }

        public ILandWater LandWater
        {
            get
            {
                return this.landWater;
            }
            set
            {
                this.landWater = value;
            }
        }

        public ILandOverGround LandOverWall
        {
            get
            {
                return this.landOverWall;
            }
            set
            {
                this.landOverWall = value;
            }
        }

        public ILandWall LandWall
        {
            get
            {
                return this.landWall;
            }
            set
            {
                if (value == null || value is ILandWall)
                {
                    this.landWall = value;
                }
            }
        }

        public ILandOverGround LandOverGround
        {
            get
            {
                return this.landOverGround;
            }
            set
            {
                this.landOverGround = value;
            }
        }

        public List<ILandGround> LandGroundList
        {
            get
            {
                return this.landGroundList;
            }
            internal set
            {
                this.landGroundList = value;
            }
        }

        public LandCase()
        {
            this.landGroundOverWallList = new List<ILandGround>();

            this.landWater = null;

            this.landOverWall = null;

            this.landWall = null;

            this.landOverGround = null;

            this.landGroundList = new List<ILandGround>();
        }

        public void AddLandGroundOverWall(ILandGround landGround)
        {
            if (landGround != null)
            {
                if (landGround.LandTransition == LandTransition.NONE)
                {
                    this.landGroundOverWallList.Clear();
                }

                this.landGroundOverWallList.Add(landGround);
            }
        }

        public void AddLandGround(ILandGround landGround)
        {
            if (landGround != null)
            {
                if(landGround.LandTransition == LandTransition.NONE)
                {
                    this.landGroundList.Clear();
                }

                this.landGroundList.Add(landGround);
            }
        }

        public List<ILandObject> GetLandObjects()
        {
            List<ILandObject> landObjectsList = new List<ILandObject>();

            foreach (ILandObject landGroundOverWallObject in this.landGroundOverWallList)
            {
                landObjectsList.Add(landGroundOverWallObject);
            }

            if (this.LandWater != null)
            {
                landObjectsList.Add(this.LandWater);
            }

            if (this.LandOverGround != null)
            {
                landObjectsList.Add(this.LandOverGround);
            }

            if (this.LandWall != null)
            {
                landObjectsList.Add(this.LandWall);
            }

            if (this.LandOverWall != null)
            {
                landObjectsList.Add(this.LandOverWall);
            }

            foreach (ILandObject landGroundObject in this.landGroundList)
            {
                landObjectsList.Add(landGroundObject);
            }

            return landObjectsList;
        }
    }
}
