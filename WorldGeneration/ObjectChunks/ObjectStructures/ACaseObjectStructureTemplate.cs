using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;

namespace WorldGeneration.ObjectChunks.ObjectStructures
{
    internal abstract class ACaseObjectStructureTemplate : ADataObjectStructureTemplate
    {
        protected int[,,] enumValueStructure;

        public Vector2i LocalBoundingBox
        {
            get;
            private set;
        }

        public IntRect BaseLocalBoundingBox
        {
            get;
            private set;
        }

        public int MaxLocalElevation
        {
            get;
            private set;
        }

        public ACaseObjectStructureTemplate(string templateUID, Vector2i localBoundingBox, int maxLocalElevation, IntRect baseLocalBoundingBox):
            base(templateUID)
        {
            this.LocalBoundingBox = localBoundingBox;
            this.MaxLocalElevation = maxLocalElevation;

            this.BaseLocalBoundingBox = baseLocalBoundingBox;
        }

        protected override bool ValidateZObjectCase(ObjectChunkLayersMonitor objectChunksMonitor, IZObjectCase zObjectCase, int worldAltitude, int baseLocalI, int baseLocalJ)
        {
            return worldAltitude + this.MaxLocalElevation - 1 < zObjectCase.NbAltitudeLevel;
        }

        protected override void UpdateZObjectCase(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IZObjectCase zObjectCase, IDataStructure dataStructure, int worldAltitude, IObjectStructure parentObjectStructure, int i, int j)
        {
            if (zObjectCase != null)
            {
                for (int k = 0; k < this.MaxLocalElevation; k++)
                {
                    int z = worldAltitude + k;
                    int enumValue = this.enumValueStructure[k, i, j];
                    if (enumValue != -1 && z < zObjectCase.NbAltitudeLevel)
                    {
                        IObjectCase objectCase = zObjectCase[z];

                        if(objectCase == null)
                        {
                            objectCase = this.CreateObjectCase(objectChunksMonitor, random, zObjectCase, z);
                        }

                        this.UpdateObjectCase(objectChunksMonitor, random, objectCase, dataStructure, parentObjectStructure, enumValue);
                    }
                }
            }
        }

        protected virtual IObjectCase CreateObjectCase(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IZObjectCase zObjectCase, int worldAltitude)
        {
            ObjectCase objectCase = new ObjectCase(zObjectCase.Position, worldAltitude);
            zObjectCase.SetCaseAt(objectCase);

            return objectCase;
        }

        protected abstract void UpdateObjectCase(ObjectChunkLayersMonitor objectChunksMonitor, Random random, IObjectCase objectCase, IDataStructure dataStructure, IObjectStructure parentObjectStructure, int enumValue);
    }
}
