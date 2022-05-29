using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks.ObjectStructures
{
    public interface IObjectStructure
    {
        string UID
        {
            get;
        }

        string TemplateUID
        {
            get;
        }

        int ObjectStructureId
        {
            get;
        }

        Vector2i WorldPosition
        {
            get;
        }

        int WorldAltitude
        {
            get;
        }
    }
}
