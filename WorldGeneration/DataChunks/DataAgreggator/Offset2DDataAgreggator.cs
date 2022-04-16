using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.DSNoise.BiomeDSNoise;
using WorldGeneration.DataChunks.PerlinNoise;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class Offset2DDataAgreggator : IDataAgreggator
    {
        internal IDataChunkLayer OffsetLayer
        {
            get;
            set;
        }

        internal IDataChunkLayer SmoothOffsetLayerX
        {
            get;
            set;
        }

        internal IDataChunkLayer SmoothOffsetLayerY
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

        public Vector2f GetSmoothOffsetAtWorldCoordinates(int x, int y)
        {
            float offsetVector1 = (this.SmoothOffsetLayerX.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase).Value;
            float offsetVector2 = (this.SmoothOffsetLayerY.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase).Value;

            offsetVector1 = offsetVector1 * 2 - 1;
            offsetVector2 = offsetVector2 * 2 - 1;
            //return (int) ((this.BiomeLayer.GetCaseAtWorldCoordinates(x, y) as DSDataCase).Value * 255);
            //return (this.BiomeLayer.GetCaseAtWorldCoordinates(x, y) as BiomeDSDataCase).CurrentBiome;
            return new Vector2f(offsetVector1, offsetVector2);
            // TEST
            //return 0;
        }
    }
}
