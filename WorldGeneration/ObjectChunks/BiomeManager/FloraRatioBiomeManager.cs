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

        // Part trees

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
            //if (altitude > this.seaAltitude)
            //{
            //    if (altitude < this.seaAltitude + 1)
            //    {
            //        return 0.1f;
            //    }
            //    else if(altitude < this.seaAltitude + 3)
            //    {
            //        return 0.05f;
            //    }
            //}
            return 0.7f;
        }

        private float GetTreeRatioFromDesert(int altitude)
        {
            //if (altitude > this.seaAltitude)
            //{
            //    return 0.0006f;
            //}
            return 0.15f;
        }

        private float GetTreeRatioFromRainForest(int altitude)
        {
            //if (altitude > this.seaAltitude)
            //{
            //    if (altitude < this.seaAltitude + 2)
            //    {
            //        return 0.4f;
            //    }
            //    else if (altitude < this.seaAltitude + 4)
            //    {
            //        return 0.2f;
            //    }
            //}
            return 0.1f;
        }

        private float GetTreeRatioFromSavanna(int altitude)
        {
            //if (altitude > this.seaAltitude + 1)
            //{
            //    return 0.01f;
            //}
            return 0.6f;
        }

        private float GetTreeRatioFromSeasonalForest(int altitude)
        {
            //if (altitude > this.seaAltitude)
            //{
            //    if (altitude < this.seaAltitude + 3)
            //    {
            //        return 0.3f;
            //    }
            //    else if (altitude < this.seaAltitude + 5)
            //    {
            //        return 0.1f;
            //    }
            //}
            return 0.75f;
        }

        private float GetTreeRatioFromTemperateForest(int altitude)
        {
            //if (altitude > this.seaAltitude)
            //{
            //    if (altitude < this.seaAltitude + 3)
            //    {
            //        return 0.3f;
            //    }
            //}
            return 0.8f;
        }

        private float GetTreeRatioFromTemperateRainForest(int altitude)
        {
            //if (altitude > this.seaAltitude)
            //{
            //    if (altitude < this.seaAltitude + 3)
            //    {
            //        return 0.4f;
            //    }
            //    else if (altitude < this.seaAltitude + 5)
            //    {
            //        return 0.2f;
            //    }
            //}
            return 0.1f;
        }

        private float GetTreeRatioFromTropicalWoodland(int altitude)
        {
            //if(altitude > this.seaAltitude + 1)
            //{
            //    if (altitude < this.seaAltitude + 3)
            //    {
            //        return 0.5f;
            //    }
            //    else if (altitude < this.seaAltitude + 6)
            //    {
            //        return 0.4f;
            //    }
            //}
            return 0.65f;
        }

        private float GetTreeRatioFromTundra(int altitude)
        {
            //if (altitude > this.seaAltitude + 1)
            //{
            //    if (altitude < this.seaAltitude + 4)
            //    {
            //        return 0.05f;
            //    }
            //}
            return 0.2f;
        }

        // Part vegetation

        public float GetVegetationRatioFromBiomeAltitude(BiomeType biome, int altitude)
        {
            switch (biome)
            {
                case BiomeType.BOREAL_FOREST:
                    return this.GetVegetationRatioFromBorealForest(altitude);
                case BiomeType.DESERT:
                    return this.GetVegetationRatioFromDesert(altitude);
                case BiomeType.RAINFOREST:
                    return this.GetVegetationRatioFromRainForest(altitude);
                case BiomeType.SAVANNA:
                    return this.GetVegetationRatioFromSavanna(altitude);
                case BiomeType.SEASONAL_FOREST:
                    return this.GetVegetationRatioFromSeasonalForest(altitude);
                case BiomeType.TEMPERATE_FOREST:
                    return this.GetVegetationRatioFromTemperateForest(altitude);
                case BiomeType.TEMPERATE_RAINFOREST:
                    return this.GetVegetationRatioFromTemperateRainForest(altitude);
                case BiomeType.TROPICAL_WOODLAND:
                    return this.GetVegetationRatioFromTropicalWoodland(altitude);
                case BiomeType.TUNDRA:
                    return this.GetVegetationRatioFromTundra(altitude);
            }
            return 0;
        }

        private float GetVegetationRatioFromBorealForest(int altitude)
        {
            if (altitude > this.seaAltitude)
            {
                if (altitude < this.seaAltitude + 3)
                {
                    return 0.1f;
                }
                else if (altitude < this.seaAltitude + 5)
                {
                    return 0.02f;
                }
            }
            return 0;
        }

        private float GetVegetationRatioFromDesert(int altitude)
        {
            return 0;
        }

        private float GetVegetationRatioFromRainForest(int altitude)
        {
            if (altitude > this.seaAltitude)
            {
                if (altitude < this.seaAltitude + 3)
                {
                    return 0.2f;
                }
                else if (altitude < this.seaAltitude + 5)
                {
                    return 0.04f;
                }
            }
            return 0;
        }

        private float GetVegetationRatioFromSavanna(int altitude)
        {
            if (altitude > this.seaAltitude)
            {
                if (altitude < this.seaAltitude + 3)
                {
                    return 0.3f;
                }
                else if (altitude < this.seaAltitude + 6)
                {
                    return 0.2f;
                }
            }
            return 0;
        }

        private float GetVegetationRatioFromSeasonalForest(int altitude)
        {
            if (altitude > this.seaAltitude)
            {
                if (altitude < this.seaAltitude + 3)
                {
                    return 0.1f;
                }
                else if (altitude < this.seaAltitude + 5)
                {
                    return 0.02f;
                }
            }
            return 0;
        }

        private float GetVegetationRatioFromTemperateForest(int altitude)
        {
            if (altitude > this.seaAltitude)
            {
                if (altitude < this.seaAltitude + 3)
                {
                    return 0.1f;
                }
                else if (altitude < this.seaAltitude + 5)
                {
                    return 0.02f;
                }
            }
            return 0;
        }

        private float GetVegetationRatioFromTemperateRainForest(int altitude)
        {
            if (altitude > this.seaAltitude)
            {
                if (altitude < this.seaAltitude + 3)
                {
                    return 0.2f;
                }
                else if (altitude < this.seaAltitude + 5)
                {
                    return 0.04f;
                }
            }
            return 0;
        }

        private float GetVegetationRatioFromTropicalWoodland(int altitude)
        {
            if (altitude > this.seaAltitude)
            {
                if (altitude < this.seaAltitude + 3)
                {
                    return 0.4f;
                }
                else if (altitude < this.seaAltitude + 5)
                {
                    return 0.3f;
                }
            }
            return 0;
        }

        private float GetVegetationRatioFromTundra(int altitude)
        {
            return 0;
        }

        // Part rock

        public float GetRockRatioFromBiomeAltitude(BiomeType biome, int altitude)
        {
            if (altitude > this.seaAltitude + 4)
            {
                if (altitude < this.seaAltitude + 5)
                {
                    return 0.05f;
                }
                else if (altitude < this.seaAltitude + 8)
                {
                    return 0.1f;
                }
                else if (altitude < this.seaAltitude + 11)
                {
                    return 0.05f;
                }
            }
            return 0;
        }
    }
}
