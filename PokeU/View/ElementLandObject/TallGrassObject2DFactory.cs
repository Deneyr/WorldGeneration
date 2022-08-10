using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.TallGrass;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.WaterObject;

namespace PokeU.View.ElementLandObject
{
    public class TallGrassObject2DFactory : AObject2DFactory
    {
        public BiomeType BiomeTypeFactory
        {
            get;
            private set;
        }

        public TallGrassObject2DFactory(BiomeType factoryBiomeType)
        {
            this.BiomeTypeFactory = factoryBiomeType;
        }

        protected override void InitializeFactory()
        {
            this.texturesPath.Add(@"Autotiles\tallGrass.png");
            this.texturesPath.Add(@"Autotiles\tallGrassBeach.png");
            this.texturesPath.Add(@"Autotiles\tallGrassDesert.png");
            this.texturesPath.Add(@"Autotiles\tallGrassForest.png");
            this.texturesPath.Add(@"Autotiles\tallGrassSnow.png");

            base.InitializeFactory();
        }

        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Vector2i position)
        {
            ATallGrassElementLandObject tallGrassElementLandObject = obj as ATallGrassElementLandObject;

            if (tallGrassElementLandObject != null)
            {
                return new TallGrassObject2D(this, tallGrassElementLandObject, position);
            }
            return null;
        }

        public Texture GetTextureFromBiomeLandType(LandType landType)
        {
            if(landType == LandType.GRASS)
            {
                switch (this.BiomeTypeFactory)
                {
                    case BiomeType.BOREAL_FOREST:
                        return this.GetTextureByIndex(4);
                    case BiomeType.DESERT:
                        return this.GetTextureByIndex(2);
                    case BiomeType.RAINFOREST:
                        return this.GetTextureByIndex(3);
                    case BiomeType.SAVANNA:
                        return this.GetTextureByIndex(2);
                    case BiomeType.SEASONAL_FOREST:
                        return this.GetTextureByIndex(0);
                    case BiomeType.TEMPERATE_FOREST:
                        return this.GetTextureByIndex(0);
                    case BiomeType.TEMPERATE_RAINFOREST:
                        return this.GetTextureByIndex(3);
                    case BiomeType.TROPICAL_WOODLAND:
                        return this.GetTextureByIndex(0);
                    case BiomeType.TUNDRA:
                        return this.GetTextureByIndex(4);
                }
            }
            else if(landType == LandType.MONTAIN)
            {
                return this.GetTextureByIndex(0);
            }
            else if(landType == LandType.SNOW)
            {
                return this.GetTextureByIndex(4);
            }
            else if(landType == LandType.SAND)
            {
                return this.GetTextureByIndex(1);
            }
            return this.GetTextureByIndex(0);
        }
    }
}
