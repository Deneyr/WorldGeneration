using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.ObjectChunks.BiomeManager
{
    internal class TallGrassBiomeManager
    {
        private int seaAltitude;

        public TallGrassBiomeManager(int seaAltitude)
        {
            this.seaAltitude = seaAltitude;
        }

        // Part trees

        public float GetTallGrassRatioFromBiomeAltitude(BiomeType biome, int altitude)
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
            return 0.7f;
        }

        private float GetTreeRatioFromDesert(int altitude)
        {
            return 0.1f;
        }

        private float GetTreeRatioFromRainForest(int altitude)
        {
            return 0.8f;
        }

        private float GetTreeRatioFromSavanna(int altitude)
        {
            return 0.5f;
        }

        private float GetTreeRatioFromSeasonalForest(int altitude)
        {
            return 0.7f;
        }

        private float GetTreeRatioFromTemperateForest(int altitude)
        {
            return 0.7f;
        }

        private float GetTreeRatioFromTemperateRainForest(int altitude)
        {
            return 0.8f;
        }

        private float GetTreeRatioFromTropicalWoodland(int altitude)
        {
            return 0.8f;
        }

        private float GetTreeRatioFromTundra(int altitude)
        {
            return 0.4f;
        }
    }
}
