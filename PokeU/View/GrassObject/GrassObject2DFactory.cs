//using PokeU.Model;
//using PokeU.Model.GrassObject;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PokeU.View.GrassObject
//{
//    public class GrassObject2DFactory : AObject2DFactory
//    {
//        protected override void InitializeFactory()
//        {
//            this.texturesPath.Add(@"Autotiles\grassDry.png");
//            this.texturesPath.Add(@"Autotiles\Snow cave highlight2.png");
//            this.texturesPath.Add(@"Autotiles\grass.png");
//            this.texturesPath.Add(@"Autotiles\grass3.png");

//            base.InitializeFactory();
//        }

//        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, IObject obj)
//        {
//            GrassLandObject grassLandObject = obj as GrassLandObject;

//            if (grassLandObject != null)
//            {
//                return new GrassObject2D(this, grassLandObject);
//            }
//            return null;
//        }
//    }
//}
