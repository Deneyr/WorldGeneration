using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.DataChunks.WeatherMonitoring
{
    internal static class AltitudeBiomeMonitor
    {
        public static float FilterAltitudeFromBiome(BiomeType biomeType, float altitude)
        {
            float altitude2 = altitude * altitude;
            float altitude3 = altitude2 * altitude;
            float altitude4 = altitude3 * altitude;
            switch (biomeType)
            {
                case BiomeType.DESERT:
                    return 0.1f * altitude4 - 0.2f * altitude3 - 0.1f * altitude2 + 0.4f * altitude;
                case BiomeType.SAVANNA:
                    return 0.08f * altitude4 - 0.1f * altitude3 - 0.1f * altitude2 + 0.2f * altitude;
                case BiomeType.TROPICAL_WOODLAND:
                    return -0.9f * altitude4 + 2.2f * altitude3 - 2.2f * altitude2 + altitude;
                case BiomeType.TUNDRA:
                    return 1.6f * altitude4 - 2.3f * altitude3 - altitude2 + 2.7f * altitude;
                case BiomeType.SEASONAL_FOREST:
                    return 2.8f * altitude4 - 5.77f * altitude3 + 2.9f * altitude2 + 0.4f * altitude;
                case BiomeType.RAINFOREST:
                    return 0.7f * altitude4 - 3.1f * altitude3 + 3.3f * altitude2 + 0.1f * altitude;
                case BiomeType.TEMPERATE_FOREST:
                    return 1.55f * altitude4 - 3.3f * altitude3 + 1.4f * altitude2 + 0.7f * altitude;
                case BiomeType.TEMPERATE_RAINFOREST:
                    return 2.1f * altitude4 - 4.2f * altitude3 + 1.9f * altitude2 + 0.4f * altitude;
                case BiomeType.BOREAL_FOREST:
                    return 0.3f * altitude4 - 0.65f * altitude3 + 0.4f * altitude2;
            }
            return altitude;
        }
    }
}
