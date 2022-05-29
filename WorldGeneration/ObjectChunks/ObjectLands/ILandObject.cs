using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks.ObjectLands
{
    public interface ILandObject
    {
        int LandObjectId
        {
            get;
        }

        LandTransition LandTransition
        {
            get;
            set;
        }

        ILandObject Clone(LandTransition wallLandTransition);

        ILandObject Clone();
    }
}
