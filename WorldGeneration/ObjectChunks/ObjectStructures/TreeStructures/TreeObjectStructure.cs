using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.ObjectChunks.ObjectStructures.TreeStructures
{
    public class TreeObjectStructure : CaseObjectStructure
    {
        public BiomeType BiomeType
        {
            get;
            internal set;
        }

        public TreeObjectStructure(string templateUID, string structureUid, int objectStructureId, Vector2i worldPosition, int worldAltitude) 
            : base(templateUID, structureUid, objectStructureId, worldPosition, worldAltitude)
        {
        }

        public enum TreePart
        {
            TOP_LEFT = 0,
            TOP_MID = 1,
            TOP_RIGHT = 2,

            MID_LEFT = 3,
            MID_MID = 4,
            MID_RIGHT = 4,

            BOT_LEFT = 5,
            BOT_MID = 6,
            BOT_RIGHT = 7
        }
    }
}
