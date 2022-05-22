//using PokeU.Model;
//using PokeU.Model.GroundObject;
//using SFML.Graphics;
//using SFML.System;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PokeU.View.GroundObject
//{
//    public class AltitudeObject2D : ALandObject2D
//    {
//        public AltitudeObject2D(IObject2DFactory factory, AltitudeLandObject landObject)
//        {
//            Texture texture = factory.GetTextureByIndex(0);

//            this.ObjectSprite = new Sprite(texture, this.GetTransitionTextureCoord(landObject.LandTransition));

//            this.ObjectSprite.Position = this.ObjectSprite.Position;

//            this.ObjectSprite.Scale = new Vector2f(0.5f, 0.5f);

//            this.Position = new Vector2f(landObject.Position.X, landObject.Position.Y);
//        }

//        public override void DrawIn(RenderWindow window, ref FloatRect boundsView)
//        {
//            base.DrawIn(window, ref boundsView);
//        }
//    }
//}
