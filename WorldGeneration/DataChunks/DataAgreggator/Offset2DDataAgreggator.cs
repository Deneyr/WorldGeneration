using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DSNoise.BiomeDSNoise;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class Offset2DDataAgreggator : IDataAgreggator
    {
        internal IDataChunkLayer OffsetLayer
        {
            get;
            set;
        }

        public Offset2DDataAgreggator()
        {
        }

        public Vector2f GetOffsetAtWorldCoordinates(int x, int y)
        {
            float[] offsetVector = (this.OffsetLayer.GetCaseAtWorldCoordinates(x, y) as BiomeDSDataCase).Value;
            //return (int) ((this.BiomeLayer.GetCaseAtWorldCoordinates(x, y) as DSDataCase).Value * 255);
            //return (this.BiomeLayer.GetCaseAtWorldCoordinates(x, y) as BiomeDSDataCase).CurrentBiome;
            return new Vector2f(offsetVector[0], offsetVector[1]);
            // TEST
            //return 0;
        }
    }
}
