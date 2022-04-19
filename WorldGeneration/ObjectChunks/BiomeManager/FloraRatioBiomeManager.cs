using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.ObjectChunks.BiomeManager
{
    internal class FloraRatioBiomeManager
    {
        private int seaAltitude;

        public FloraRatioBiomeManager(int seaAltitude)
        {
            this.seaAltitude = seaAltitude;
        }

        public float GetTreeRatioFromBiomeAltitude(BiomeType biome, int altitude)
        {
            switch (biome)
            {
                case BiomeType.BOREAL_FOREST:
                    return this.GetTreeRatioFromBorealForest(altitude);
                case BiomeType.DESERT:
                    return this.GetTreeRatioFromDesert(altitude);
                case BiomeType.RAINFOREST:
                    return this.GetTreeRatioFromRainForest(altitude);
                case BiomeType.SAVANNA:
                    return this.GetTreeRatioFromSavanna(altitude);
                case BiomeType.SEASONAL_FOREST:
                    return this.GetTreeRatioFromSeasonalForest(altitude);
                case BiomeType.TEMPERATE_FOREST:
                    return this.GetTreeRatioFromTemperateForest(altitude);
                case BiomeType.TEMPERATE_RAINFOREST:
                    return this.GetTreeRatioFromTemperateRainForest(altitude);
                case BiomeType.TROPICAL_WOODLAND:
                    return this.GetTreeRatioFromTropicalWoodland(altitude);
                case BiomeType.TUNDRA:
                    return this.GetTreeRatioFromTundra(altitude);
            }
            return 0;
        }

        private float GetTreeRatioFromBorealForest(int altitude)
        {
            if (altitude > this.seaAltitude + 1)
            {
                if (altitude < this.seaAltitude + 2)
                {
                    return 0.1f;
                }
                else if(altitude < this.seaAltitude + 4)
                {
                    return 0.05f;
                }
            }
            return 0;
        }

        private float GetTreeRatioFromDesert(int altitude)
        {
            if (altitude > this.seaAltitude)
            {
                return 0.0001f;
            }
            return 0;
        }

        private float GetTreeRatioFromRainForest(int altitude)
        {
            if (altitude > this.seaAltitude + 1)
            {
                if (altitude < this.seaAltitude + 2)
                {
                    return 0.4f;
                }
                else if (altitude < this.seaAltitude + 4)
                {
                    return 0.2f;
                }
            }
            return 0;
        }

        private float GetTreeRatioFromSavanna(int altitude)
        {
            if (altitude > this.seaAltitude + 1)
            {
                return 0.0005f;
            }
            return 0;
        }

        private float GetTreeRatioFromSeasonalForest(int altitude)
        {
            if (altitude > this.seaAltitude + 1)
            {
                if (altitude < this.seaAltitude + 2)
                {
                    return 0.3f;
                }
                else if (altitude < this.seaAltitude + 4)
                {
                    return 0.1f;
                }
            }
            return 0;
        }

        private float GetTreeRatioFromTemperateForest(int altitude)
        {
            if (altitude > this.seaAltitude + 1)
            {
                if (altitude < this.seaAltitude + 3)
                {
                    return 0.3f;
                }
            }
            return 0;
        }

        private float GetTreeRatioFromTemperateRainForest(int altitude)
        {
            if (altitude > this.seaAltitude + 1)
            {
                if (altitude < this.seaAltitude + 2)
                {
                    return 0.4f;
                }
                else if (altitude < this.seaAltitude + 4)
                {
                    return 0.2f;
                }
            }
            return 0;
        }

        private float GetTreeRatioFromTropicalWoodland(int altitude)
        {
            if(altitude > this.seaAltitude + 1)
            {
                if (altitude < this.seaAltitude + 3)
                {
                    return 0.5f;
                }
                else if (altitude < this.seaAltitude + 5)
                {
                    return 0.3f;
                }
            }
            return 0;
        }

        private float GetTreeRatioFromTundra(int altitude)
        {
            if (altitude > this.seaAltitude + 1)
            {
                if (altitude < this.seaAltitude + 3)
                {
                    return 0.05f;
                }
            }
            return 0;
        }
    }
}
