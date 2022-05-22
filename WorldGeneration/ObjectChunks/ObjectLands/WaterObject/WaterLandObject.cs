using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectLands.WaterObject
{
    public class WaterLandObject : ALandObject, ILandWater
    {
        public WaterLandObject(int landObjectId) 
            : base(landObjectId)
        {
        }

        public void SetLandTransition(LandTransition landTransition)
        {
            this.LandTransition = landTransition;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            return null;
        }

        public override ILandObject Clone()
        {
            WaterLandObject waterLandObject = new WaterLandObject(this.LandObjectId);
            waterLandObject.SetLandTransition(this.LandTransition);

            return waterLandObject;
        }
    }
}
