//using PokeU.Model;
//using PokeU.Model.GroundObject;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PokeU.View.GroundObject
//{
//    public class GroundElementObject2DFactory : AObject2DFactory
//    {
//        protected override void InitializeFactory()
//        {
//            this.texturesPath.Add(@"Autotiles\elementsSand.png");

//            base.InitializeFactory();
//        }

//        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, IObject obj)
//        {
//            GroundElementLandObject groundElementLandObject = obj as GroundElementLandObject;

//            if (groundElementLandObject != null)
//            {
//                return new GroundElementObject2D(this, groundElementLandObject);
//            }
//            return null;
//        }
//    }
//}
