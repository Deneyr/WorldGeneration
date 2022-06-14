using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;
using WorldGeneration.ObjectChunks.ObjectLands.ElementObject;

namespace WorldGeneration.ObjectChunks.ObjectStructures.TallGrassStructures
{
    internal class TallGrassStructureTemplate : ADataObjectStructureTemplate
    {
        public TallGrassStructureTemplate() 
            : base("TallGrassStructure")
        {
        }

        protected override void UpdateObjectCase(Random random, IZObjectCase zObjectCase, int worldAltitude, IObjectStructure parentObjectStructure, IDataStructureCase dataStructureCase)
        {
            if (zObjectCase.GroundAltitude >= 0)
            {
                ObjectCase currentObjectCase = zObjectCase[zObjectCase.GroundAltitude] as ObjectCase;

                if (dataStructureCase != null && currentObjectCase.Land.LandWater == null)
                {
                    currentObjectCase.Land.LandOverGround = new TallGrassElementLandObject(random.Next());
                }
            }
        }

        protected override IObjectStructure CreateObjectStructureFrom(Random random, Vector2i worldPosition, int worldAltitude)
        {
            return new TallGrassStructure(this.TemplateUID, random.Next(), worldPosition, worldAltitude);
        }
    }
}
