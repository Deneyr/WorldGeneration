using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.ObjectChunks.ObjectLands;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class AltitudeTransitionObjectChunkLayer : AObjectChunkLayer
    {
        private AltitudeObjectChunkLayer altitudeObjectChunkLayer;

        public AltitudeTransitionObjectChunkLayer(string id) 
            : base(id)
        {
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            this.altitudeObjectChunkLayer = (objectChunksMonitor.ObjectChunksLayers["altitudeLayer"] as AltitudeObjectChunkLayer);

            base.ComputeObjectChunk(objectChunksMonitor, objectChunk);
        }

        protected override void ComputeChunkArea(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectChunk objectChunk, Vector2i localPosition, Vector2i worldPosition)
        {
            IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(localPosition.X, localPosition.Y) as IZObjectCase;

            List<LandTransition> landTransitions = this.altitudeObjectChunkLayer.GetLandTransitionAtLocal(localPosition.X, localPosition.Y);
            LandCase firstLandCase = (zObjectCase[zObjectCase.GroundAltitude] as ObjectCase).Land;
            int currentAltitude = zObjectCase.GroundAltitude + 1;

            foreach(LandTransition landTransition in landTransitions)
            {
                //if(zObjectCase[currentAltitude] == null)
                //{
                ObjectCase objectCase = new ObjectCase(zObjectCase.Position, currentAltitude);

                zObjectCase.SetCaseAt(objectCase);
                //}

                LandCase landCase = (zObjectCase[currentAltitude] as ObjectCase).Land;
                landCase.LandWall = firstLandCase.LandGroundList.First().Clone() as ILandWall;
                landCase.LandWall.LandTransition = landTransition;

                foreach(ILandGround landGround in firstLandCase.LandGroundList)
                {
                    ILandGround newLandGroundOverWall = landGround.Clone(landTransition) as ILandGround;

                    if (newLandGroundOverWall != null)
                    {
                        landCase.LandGroundOverWallList.Add(newLandGroundOverWall);
                    }
                }

                currentAltitude++;
            }
        }
    }
}
