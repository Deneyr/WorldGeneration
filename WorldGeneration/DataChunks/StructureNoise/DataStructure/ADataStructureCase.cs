using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace WorldGeneration.DataChunks.StructureNoise.DataStructure
{
    internal class ADataStructureCase: IDataStructureCase
    {
        public IDataStructure ParentDataStructure
        {
            private set;
            get;
        }

        public Vector2i Position
        {
            private set;
            get;
        }

        public ADataStructureCase(IDataStructure parentDataStructure, int x, int y)
        {
            this.Position = new Vector2i(x, y);
            this.ParentDataStructure = parentDataStructure;
        }
    }
}
