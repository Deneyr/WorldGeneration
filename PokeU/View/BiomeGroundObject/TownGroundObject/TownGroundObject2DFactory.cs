using PokeU.View.GroundObject;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;
using WorldGeneration.ObjectChunks.ObjectLands.TownGroundObject;

namespace PokeU.View.BiomeGroundObject.TownGroundObject
{
    public class TownGroundObject2DFactory : AGroundObject2DFactory
    {
        protected override void InitializeFactory()
        {
            // GROUND = 0,
            // SEA_DEPTH = 1,
            // SAND = 2,
            // GRASS = 3,
            // MONTAIN = 4,
            // SNOW = 5,
            this.texturesPath.Add(@"Autotiles\Brick path2.png");

            this.texturesPath.Add(@"Autotiles\cliff.png");

            base.InitializeFactory();
        }

        public override Texture GetTextureByLandType(LandType landType)
        {
            //switch (landType)
            //{
            //    case LandType.GROUND:
            //        return this.Resources[this.texturesPath.ElementAt(0)];
            //    case LandType.SEA_DEPTH:
            //        return this.Resources[this.texturesPath.ElementAt(0)];
            //    case LandType.SAND:
            //        return this.Resources[this.texturesPath.ElementAt(1)];
            //    case LandType.GRASS:
            //        return this.Resources[this.texturesPath.ElementAt(1)];
            //    case LandType.MOUNTAIN:
            //        return this.Resources[this.texturesPath.ElementAt(2)];
            //    case LandType.SNOW:
            //        return this.Resources[this.texturesPath.ElementAt(1)];
            //}
            return this.Resources[this.texturesPath.ElementAt(0)];
        }

        public override Texture GetWallTexture()
        {
            return this.Resources[this.texturesPath.ElementAt(1)];
        }


        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Vector2i position)
        {
            ATownGroundLandObject townLandObject = obj as ATownGroundLandObject;

            if (townLandObject != null)
            {
                return new TownGroundObject2D(this, townLandObject, position, this.IsWall);
            }
            return null;
        }
    }
}