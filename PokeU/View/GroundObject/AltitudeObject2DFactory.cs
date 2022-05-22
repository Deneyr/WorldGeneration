//using PokeU.Model;
//using PokeU.Model.GroundObject;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PokeU.View.GroundObject
//{
//    public class AltitudeObject2DFactory : AObject2DFactory
//    {
//        protected override void InitializeFactory()
//        {
//            this.texturesPath.Add(@"Autotiles\cliff.png");

//            base.InitializeFactory();
//        }

//        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, IObject obj)
//        {
//            AltitudeLandObject altitudeLandObject = obj as AltitudeLandObject;

//            if (altitudeLandObject != null)
//            {
//                return new AltitudeObject2D(this, altitudeLandObject);
//            }
//            return null;
//        }
//    }
//}
