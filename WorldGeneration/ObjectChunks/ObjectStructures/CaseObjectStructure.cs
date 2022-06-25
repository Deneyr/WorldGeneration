using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace WorldGeneration.ObjectChunks.ObjectStructures
{
    public class CaseObjectStructure : IObjectStructure
    {
        public string UID
        {
            get;
            private set;
        }

        public string TemplateUID
        {
            get;
            private set;
        }

        public int ObjectStructureId
        {
            get;
            private set;
        }

        public Vector2i WorldPosition
        {
            get;
            private set;
        }

        public int WorldAltitude
        {
            get;
            private set;
        }

        public CaseObjectStructure(string templateUID, string structureUid, int objectStructureId, Vector2i worldPosition, int worldAltitude)
        {
            this.TemplateUID = templateUID;
            this.UID = structureUid;

            this.ObjectStructureId = objectStructureId;

            this.WorldPosition = worldPosition;
            this.WorldAltitude = worldAltitude;
        }

    }
}
