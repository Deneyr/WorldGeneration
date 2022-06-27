using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.ObjectChunks.ObjectStructures.TallGrassStructures
{
    public class TallGrassObjectStructure: CaseObjectStructure
    {
        public BiomeType BiomeType
        {
            get;
            internal set;
        }

        public TallGrassObjectStructure(string templateUID, string structureUid, int objectStructureId, Vector2i worldPosition, int worldAltitude)
            : base(templateUID, structureUid, objectStructureId, worldPosition, worldAltitude)
        {
        }
    }
}
