//using Microsoft.Xna.Framework.Graphics;
//using PokeU.View;
//using SFML.Graphics;
//using SFML.System;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WorldGeneration.ObjectChunks;
//using WorldGeneration.ObjectChunks.ObjectLands;

//namespace testMonoGame.View
//{
//    internal class LandZCases2D : AObject2D
//    {
//        private List<LandCase2D> landCase2Ds;

//        public int MaxAltitude
//        {
//            get;
//            protected set;
//        }

//        public LandZCases2D(LandWorld2D landWorld2D, IObjectChunk parentObjectChunk, ZObjectCase zObjectCase, Vector2i position)
//        {
//            this.landCase2Ds = new List<LandCase2D>();
//            this.MaxAltitude = zObjectCase.NbAltitudeLevel;

//            LandCase2DFactory caseObject2DFactory = LandWorld2D.MappingObjectModelView[typeof(ObjectCase)] as LandCase2DFactory;

//            for(int z = 0; z < zObjectCase.NbAltitudeLevel; z++)
//            {
//                ObjectCase objectCase = zObjectCase[z] as ObjectCase;
//                if (objectCase != null)
//                {
//                    LandCase landCase = objectCase.Land;

//                    if (this.IsLandCaseValid(landCase, z, zObjectCase))
//                    {
//                        caseObject2DFactory.CurrentObjectChunk = parentObjectChunk;
//                        LandCase2D newLandCase2D = caseObject2DFactory.CreateObject2D(landWorld2D, objectCase, zObjectCase.Position) as LandCase2D;

//                        if (z < this.MaxAltitude - 1 && zObjectCase[z + 1] != null)
//                        {
//                            LandCase landCaseUp = (zObjectCase[z + 1] as ObjectCase).Land;
//                            newLandCase2D.UpdateOverLandCase(landCaseUp);
//                        }
//                        if (z > 0 && zObjectCase[z - 1] != null)
//                        {
//                            LandCase landCaseDown = (zObjectCase[z - 1] as ObjectCase).Land;
//                            newLandCase2D.UpdateUnderLandCase(landCaseDown);
//                        }

//                        newLandCase2D.SetLandCaseRatio(z, LandWorld2D.LOADED_ALTITUDE_RANGE);

//                        this.landCase2Ds.Add(newLandCase2D);
//                    }
//                }
//            }

//            this.Position = new Vector2f(position.X, position.Y);
//        }

//        private bool IsLandCaseValid(LandCase landCase, int caseAltitude, ZObjectCase zObjectCase)
//        {
//            if(caseAltitude < zObjectCase.GroundAltitude
//                && landCase.IsOnlyWater)
//            {
//                return false;
//            }

//            return true;
//        }

//        public void UpdateCurrentAltitude(int currentAltitude)
//        {
//            foreach (LandCase2D landCase2D in this.landCase2Ds)
//            {
//                landCase2D.SetLandCaseRatio(currentAltitude, LandWorld2D.LOADED_ALTITUDE_RANGE);
//            }
//        }

//        public override void DrawIn(SpriteBatch spriteBatch, ref FloatRect boundsView)
//        {
//            foreach (LandCase2D landCase2D in this.landCase2Ds)
//            {
//                landCase2D.DrawIn(spriteBatch, ref boundsView);
//            }
//        }

//        public override void Dispose()
//        {
//            foreach(LandCase2D landCase2D in this.landCase2Ds)
//            {
//                landCase2D.Dispose();
//            }

//            this.landCase2Ds.Clear();
//        }
//    }
//}
