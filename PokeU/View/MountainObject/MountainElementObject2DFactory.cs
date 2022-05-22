//using PokeU.Model;
//using PokeU.Model.MountainObject;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PokeU.View.MountainObject
//{
//    public class MountainElementObject2DFactory : AObject2DFactory
//    {
//        protected override void InitializeFactory()
//        {
//            this.texturesPath.Add(@"Autotiles\elementsMountain.png");

//            base.InitializeFactory();
//        }

//        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, IObject obj)
//        {
//            MountainElementLandObject mountainElementLandObject = obj as MountainElementLandObject;

//            if (mountainElementLandObject != null)
//            {
//                return new MountainElementObject2D(this, mountainElementLandObject);
//            }
//            return null;
//        }
//    }
//}
