using PokeU.View.GroundObject;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.WeatherMonitoring;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject.Tree;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;
using static WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures.TreeObjectStructure;

namespace PokeU.View.ElementLandObject
{
    public class TreeObject2DFactory : AGroundObject2DFactory
    {
        public BiomeType BiomeTypeFactory
        {
            get;
            private set;
        }

        public TreeObject2DFactory(BiomeType factoryBiomeType)
        {
            this.BiomeTypeFactory = factoryBiomeType;
        }

        protected override void InitializeFactory()
        {
            this.texturesPath.Add(@"Autotiles\tree.png");
            this.texturesPath.Add(@"Autotiles\tree2.png");
            this.texturesPath.Add(@"Autotiles\tree3.png");

            this.texturesPath.Add(@"Autotiles\treeBeach2.png");

            this.texturesPath.Add(@"Autotiles\treeBeach.png");

            this.texturesPath.Add(@"Autotiles\treeHot.png");
            this.texturesPath.Add(@"Autotiles\tree4.png");

            this.texturesPath.Add(@"Autotiles\treeSnow.png");
            this.texturesPath.Add(@"Autotiles\treeSnow2.png");
            this.texturesPath.Add(@"Autotiles\tree8.png");

            this.texturesPath.Add(@"Autotiles\cactus.png");

            base.InitializeFactory();
        }

        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Vector2i position)
        {
            ATreeElementLandObject treeElementLandObject = obj as ATreeElementLandObject;

            if (treeElementLandObject != null)
            {
                return new TreeObject2D(this, treeElementLandObject, position);
            }
            return null;
        }

        public Texture GetTextureFromBiomeLandType(LandType landType, int treeId)
        {
            if (landType == LandType.GRASS
                || landType == LandType.MONTAIN)
            {
                return this.GetTextureByIndex(this.GetIndexFromId(treeId));
            }
            else if (landType == LandType.SNOW)
            {
                return this.GetTextureByIndex(7 + treeId % 2);
            }
            else if (landType == LandType.SAND)
            {
                switch (this.BiomeTypeFactory)
                {
                    case BiomeType.BOREAL_FOREST:
                    case BiomeType.TUNDRA:
                        return this.GetTextureByIndex(9);
                    default:
                        return this.GetTextureByIndex(3);
                }
            }
            return this.GetTextureByIndex(0);
        }

        private int GetIndexFromId(int treeId)
        {
            switch (this.BiomeTypeFactory)
            {
                case BiomeType.DESERT:
                    return 10;
                case BiomeType.SAVANNA:
                    return 5 + treeId % 2;
                case BiomeType.SEASONAL_FOREST:
                case BiomeType.TEMPERATE_FOREST:
                    return treeId % 3;
                case BiomeType.TEMPERATE_RAINFOREST:
                case BiomeType.RAINFOREST:
                    return treeId % 3;
                case BiomeType.TROPICAL_WOODLAND:
                    return 4;
                case BiomeType.BOREAL_FOREST:
                case BiomeType.TUNDRA:
                    return 7 + treeId % 2;
            }
            return treeId % 3;
        }

        public override Texture GetTextureByLandType(LandType landType)
        {
            throw new NotImplementedException();
        }

        public override Texture GetWallTexture()
        {
            throw new NotImplementedException();
        }
    }
}
