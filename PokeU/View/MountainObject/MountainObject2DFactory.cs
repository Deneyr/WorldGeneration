//using PokeU.Model;
//using PokeU.Model.MountainObject;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PokeU.View.MountainObject
//{
//    public class MountainObject2DFactory : AObject2DFactory
//    {
//        protected override void InitializeFactory()
//        {
//            this.texturesPath.Add(@"Autotiles\mountainGround.png");
//            this.texturesPath.Add(@"Autotiles\mountainGround2.png");

//            base.InitializeFactory();
//        }

//        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, IObject obj)
//        {
//            MountainLandObject mountainLandObject = obj as MountainLandObject;

//            if (mountainLandObject != null)
//            {
//                return new MountainObject2D(this, mountainLandObject);
//            }
//            return null;
//        }
//    }
//}
