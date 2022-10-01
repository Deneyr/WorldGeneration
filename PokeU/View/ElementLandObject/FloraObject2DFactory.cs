using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Flora;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace PokeU.View.ElementLandObject
{
    public class FloraObject2DFactory : AObject2DFactory
    {
        public BiomeType BiomeTypeFactory
        {
            get;
            private set;
        }

        public FloraObject2DFactory(BiomeType factoryBiomeType)
        {
            this.BiomeTypeFactory = factoryBiomeType;
        }

        protected override void InitializeFactory()
        {
            this.texturesPath.Add(@"Autotiles\elementsGrass.png");
            this.texturesPath.Add(@"Autotiles\elementsGrassLight.png");

            this.texturesPath.Add(@"Autotiles\elementsMountain.png");

            this.texturesPath.Add(@"Autotiles\elementsTropical.png");

            this.texturesPath.Add(@"Autotiles\elementsSavanna.png");

            base.InitializeFactory();
        }

        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Vector2i position)
        {
            AFloraElementLandObject tallGrassElementLandObject = obj as AFloraElementLandObject;

            if (tallGrassElementLandObject != null)
            {
                return new FloraObject2D(this, tallGrassElementLandObject, position);
            }
            return null;
        }

        public Texture GetTextureFromBiomeLandType(LandType landType)
        {
            if (landType == LandType.GRASS)
            {
                switch (this.BiomeTypeFactory)
                {
                    case BiomeType.BOREAL_FOREST:
                        return this.GetTextureByIndex(0);
                    case BiomeType.DESERT:
                        return this.GetTextureByIndex(0);
                    case BiomeType.RAINFOREST:
                        return this.GetTextureByIndex(0);
                    case BiomeType.SAVANNA:
                        return this.GetTextureByIndex(4);
                    case BiomeType.SEASONAL_FOREST:
                        return this.GetTextureByIndex(0);
                    case BiomeType.TEMPERATE_FOREST:
                        return this.GetTextureByIndex(1);
                    case BiomeType.TEMPERATE_RAINFOREST:
                        return this.GetTextureByIndex(1);
                    case BiomeType.TROPICAL_WOODLAND:
                        return this.GetTextureByIndex(3);
                    case BiomeType.TUNDRA:
                        return this.GetTextureByIndex(0);
                }
            }
            else if (landType == LandType.MOUNTAIN)
            {
                return this.GetTextureByIndex(2);
            }
            else if (landType == LandType.SNOW)
            {
                return this.GetTextureByIndex(0);
            }
            else if (landType == LandType.SAND)
            {
                return this.GetTextureByIndex(0);
            }
            return this.GetTextureByIndex(0);
        }
    }
}
