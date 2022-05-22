//using PokeU.Model;
//using PokeU.Model.MountainObject;
//using PokeU.View.GroundObject;
//using SFML.Graphics;
//using SFML.System;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PokeU.View.MountainObject
//{
//    public class MountainObject2D : GroundObject2D
//    {
//        public MountainObject2D(IObject2DFactory factory, MountainLandObject landObject) :
//            base()
//        {
//            Texture texture = factory.GetTextureByIndex((int)landObject.LandMountainType);

//            if (landObject.LandTransition == LandTransition.NONE)
//            {
//                Random random = new Random(landObject.Position.X - landObject.Position.Y * landObject.Position.Y);

//                switch (random.Next(0, 4))
//                {
//                    case 0:
//                        this.ObjectSprite = new Sprite(texture, new IntRect(2 * MainWindow.MODEL_TO_VIEW, 4 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW));
//                        break;
//                    case 1:
//                        this.ObjectSprite = new Sprite(texture, new IntRect(0 * MainWindow.MODEL_TO_VIEW, 0 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW));
//                        break;
//                    case 2:
//                        this.ObjectSprite = new Sprite(texture, new IntRect(2 * MainWindow.MODEL_TO_VIEW, 0 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW));
//                        break;
//                    case 3:
//                        this.ObjectSprite = new Sprite(texture, new IntRect(4 * MainWindow.MODEL_TO_VIEW, 0 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW));
//                        break;
//                }
//            }
//            else
//            {
//                this.ObjectSprite = new Sprite(texture, this.GetTransitionTextureCoord(landObject.LandTransition));
//            }

//            this.ObjectSprite.Scale = new Vector2f(0.5f, 0.5f);

//            this.Position = new Vector2f(landObject.Position.X, landObject.Position.Y);
//        }
//    }
//}
