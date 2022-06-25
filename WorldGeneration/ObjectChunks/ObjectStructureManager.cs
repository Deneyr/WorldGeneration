using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectStructures;

namespace WorldGeneration.ObjectChunks
{
    internal class ObjectStructureManager
    {
        private Dictionary<string, IObjectStructure> idToObjectStructures;

        public ObjectStructureManager()
        {
            this.idToObjectStructures = new Dictionary<string, IObjectStructure>();
        }

        public void RegisterObjectStructure(IObjectStructure objectStructureToRegister)
        {
            if(this.idToObjectStructures.TryGetValue(objectStructureToRegister.UID, out IObjectStructure objectStructure))
            {

            }
            else
            {
                this.idToObjectStructures[objectStructureToRegister.UID] = objectStructureToRegister;
            }
        }

        public IObjectStructure GetObjectStructure(string uid)
        {
            if (this.idToObjectStructures.TryGetValue(uid, out IObjectStructure objectStructure))
            {
                return objectStructure;
            }
            return null;
        }

    }
}
