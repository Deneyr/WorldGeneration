//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PokeU.View.GrassObject
//{
//    public class GrassElementObject2DFactory: AObject2DFactory
//    {
//        protected override void InitializeFactory()
//        {
//            this.texturesPath.Add(@"Autotiles\elementsGrassDry.png");
//            this.texturesPath.Add(@"Autotiles\elementsGrassLight.png");
//            this.texturesPath.Add(@"Autotiles\elementsGrass.png");
//            this.texturesPath.Add(@"Autotiles\elementsGrassForest.png");

//            base.InitializeFactory();
//        }

//        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, IObject obj)
//        {
//            GrassElementLandObject grassElementLandObject = obj as GrassElementLandObject;

//            if (grassElementLandObject != null)
//            {
//                return new GrassElementObject2D(this, grassElementLandObject);
//            }
//            return null;
//        }
//    }
//}
