//using PokeU.Model.GrassObject;
//using PokeU.View.GroundObject;
//using SFML.Graphics;
//using SFML.System;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PokeU.View.GrassObject
//{
//    public class GrassElementObject2D : ALandObject2D
//    {
//        public GrassElementObject2D(IObject2DFactory factory, GrassElementLandObject landObject) :
//            base()
//        {
//            Texture texture = factory.GetTextureByIndex((int)landObject.LandGrassType);

//            // Random random = new Random(landObject.Position.X - landObject.Position.Y * landObject.Position.Y);

//            int elementIndex = landObject.ElementIndex;
//            if(landObject.LandGrassType == GrassType.DRY)
//            {
//                elementIndex = 0; 
//            }

//            this.ObjectSprite = new Sprite(texture, new IntRect(elementIndex * 2 * MainWindow.MODEL_TO_VIEW, 0 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW));

//            this.ObjectSprite.Scale = new Vector2f(0.5f, 0.5f);

//            this.Position = new Vector2f(landObject.Position.X, landObject.Position.Y);
//        }
//    }
//}
