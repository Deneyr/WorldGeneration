using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace WorldGeneration.ModelChunk
{
    public class ZModelCases : IZModelCases
    {
        private IModelCase[] zModelCases;

        public IModelCase this[int z]
        {
            get
            {
                return this.zModelCases[z];
            }
        }

        public Vector2i Position
        {
            get;
        }

        public ZModelCases(int nbZCases)
        {
            this.zModelCases = new IModelCase[nbZCases];
        }
    }
}
