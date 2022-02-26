using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace WorldGeneration.ObjectChunks
{
    public class ZObjectCases : IZObjectCases
    {
        private IObjectCase[] zModelCases;

        public IObjectCase this[int z]
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

        public ZObjectCases(int nbZCases)
        {
            this.zModelCases = new IObjectCase[nbZCases];
        }
    }
}
