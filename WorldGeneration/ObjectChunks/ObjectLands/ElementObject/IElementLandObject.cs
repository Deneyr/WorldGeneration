using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks.ObjectLands.ElementObject
{
    public interface IElementLandObject
    {
        int LandElementObjectId
        {
            get;
        }

        IElementLandObject Clone();
    }
}
