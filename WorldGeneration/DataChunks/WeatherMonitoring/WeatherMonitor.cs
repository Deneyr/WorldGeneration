using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.DataChunks.WeatherMonitoring
{
    public class WeatherMonitor
    {
        private Dictionary<Color, BiomeType> colorToBiome;

        private BiomeType[,] biomeMatrix;

        public WeatherMonitor(string weatherImagePath)
        {
            this.colorToBiome = new Dictionary<Color, BiomeType>();
            this.colorToBiome.Add(new Color(0x23725eff), BiomeType.BOREAL_FOREST);
            this.colorToBiome.Add(new Color(0xbefff2ff), BiomeType.TUNDRA);
            this.colorToBiome.Add(new Color(0x223d35ff), BiomeType.TEMPERATE_RAINFOREST);
            this.colorToBiome.Add(new Color(0x44b36aff), BiomeType.TEMPERATE_FOREST);
            this.colorToBiome.Add(new Color(0xb8c862ff), BiomeType.SAVANNA);
            this.colorToBiome.Add(new Color(0xffffb2ff), BiomeType.DESERT);
            this.colorToBiome.Add(new Color(0x214d29ff), BiomeType.RAINFOREST);
            this.colorToBiome.Add(new Color(0x6a9026ff), BiomeType.SEASONAL_FOREST);
            this.colorToBiome.Add(new Color(0xbca135ff), BiomeType.TROPICAL_WOODLAND);

            this.ConstructColorToBiome(weatherImagePath);
        }

        private void ConstructColorToBiome(string weatherImagePath)
        {
            using(Image weatherImage = new Image(weatherImagePath))
            {
                this.biomeMatrix = new BiomeType[weatherImage.Size.X, weatherImage.Size.Y];

                for(uint i = 0; i < weatherImage.Size.Y; i++)
                {
                    for (uint j = 0; j < weatherImage.Size.X; j++)
                    {
                        this.biomeMatrix[i, j] = this.colorToBiome[weatherImage.GetPixel(i, j)];
                    }
                }
            }
        }

        public BiomeType GetBiomeAt(float x, float y)
        {
            int i = (int) y * this.biomeMatrix.GetLength(0);
            int j = (int) x * this.biomeMatrix.GetLength(1);

            return this.biomeMatrix[i, j];
        }
    }
}
