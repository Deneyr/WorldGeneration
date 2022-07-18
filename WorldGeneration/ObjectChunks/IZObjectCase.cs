using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.ObjectChunks
{
    public interface IZObjectCase : ICase
    {
        BiomeType ObjectBiome
        {
            get;
            set;
        }

        IObjectCase this[int z]
        {
            get;
        }

        int NbAltitudeLevel
        {
            get;
        }

        int GroundAltitude
        {
            get;
        }

        void SetGroundCaseAt(IObjectCase objectCase);

        void SetCaseAt(IObjectCase objectCase);
    }
}
