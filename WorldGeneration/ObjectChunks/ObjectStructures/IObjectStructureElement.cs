using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks.ObjectStructures
{
    public interface IObjectStructureElement
    {
        string ParentStructureUID
        {
            get;
        }

        //Vector2i LocalStructurePosition
        //{
        //    get;
        //}

        //int LocalStructureAltitude
        //{
        //    get;
        //}
    }
}
