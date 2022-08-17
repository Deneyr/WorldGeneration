using PokeU.View.GroundObject;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.GroundObject;

namespace PokeU.View.BiomeGroundObject
{
    public class BorealForestGroundObject2DFactory : AGroundObject2DFactory
    {
        protected override void InitializeFactory()
        {
            // GROUND = 0,
            // SEA_DEPTH = 1,
            // SAND = 2,
            // GRASS = 3,
            // MONTAIN = 4,
            // SNOW = 5,
            this.texturesPath.Add(@"Autotiles\Red cave floor2.png");
            this.texturesPath.Add(@"Autotiles\snow.png");
            this.texturesPath.Add(@"Autotiles\mountainGround2.png");

            this.texturesPath.Add(@"Autotiles\cliff.png");

            base.InitializeFactory();
        }

        public override Texture GetTextureByLandType(LandType landType)
        {
            switch (landType)
            {
                case LandType.GROUND:
                    return this.Resources[this.texturesPath.ElementAt(0)];
                case LandType.SEA_DEPTH:
                    return this.Resources[this.texturesPath.ElementAt(0)];
                case LandType.SAND:
                    return this.Resources[this.texturesPath.ElementAt(1)];
                case LandType.GRASS:
                    return this.Resources[this.texturesPath.ElementAt(1)];
                case LandType.MONTAIN:
                    return this.Resources[this.texturesPath.ElementAt(2)];
                case LandType.SNOW:
                    return this.Resources[this.texturesPath.ElementAt(1)];
            }
            return null;
        }

        public override Texture GetWallTexture()
        {
            return this.Resources[this.texturesPath.ElementAt(3)];
        }


        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Vector2i position)
        {
            GroundLandObject groundLandObject = obj as GroundLandObject;

            if (groundLandObject != null)
            {
                return new BorealForestGroundObject2D(this, groundLandObject, position, this.IsWall);
            }
            return null;
        }
    }
}
