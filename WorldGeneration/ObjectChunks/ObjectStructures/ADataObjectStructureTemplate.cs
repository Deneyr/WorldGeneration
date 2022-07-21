using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.StructureNoise.DataStructure;

namespace WorldGeneration.ObjectChunks.ObjectStructures
{
    internal abstract class ADataObjectStructureTemplate : IObjectStructureTemplate
    {
        public string TemplateUID
        {
            get;
            private set;
        }

        public ADataObjectStructureTemplate(string templateUID)
        {
            this.TemplateUID = templateUID;
        }

        public virtual IObjectStructure GenerateStructureAtWorldPosition(Random random, IDataStructure dataStructure, int worldAltitude, IObjectChunk objectChunk)
        {
            //Vector2i worldPosition = dataStructure.StructureWorldPosition;

            //int structureHeight = dataStructure.StructureBoundingBox.Height;
            //int structureWidth = dataStructure.StructureBoundingBox.Width;

            IntRect structureWorldArea = dataStructure.StructureWorldBoundingBox;
            IntRect chunkWorldArea = ChunkHelper.GetWorldAreaFromChunkArea(objectChunk.NbCaseSide, new IntRect(objectChunk.Position.X, objectChunk.Position.Y, 1, 1));

            if (chunkWorldArea.Intersects(structureWorldArea, out IntRect overlapingArea))
            {
                string structureUid = this.ConstructeObjectStructureUID(dataStructure.StructureWorldPosition, worldAltitude);
                IObjectStructure objectStructure = this.CreateObjectStructureFrom(random, structureUid, dataStructure, worldAltitude);
                objectChunk.RegisterObjectStructure(objectStructure);

                int marginHeight = overlapingArea.Top - structureWorldArea.Top;
                int marginWidth = overlapingArea.Left - structureWorldArea.Left;

                for (int i = 0; i < overlapingArea.Height; i++)
                {
                    int y = overlapingArea.Top + i;
                    for (int j = 0; j < overlapingArea.Width; j++)
                    {
                        int x = overlapingArea.Left + j;

                        IZObjectCase zObjectCase = ChunkHelper.GetCaseAtWorldCoordinates(objectChunk, x, y) as IZObjectCase;
                        if (zObjectCase != null)
                        {
                            this.UpdateZObjectCase(random, zObjectCase, dataStructure, worldAltitude, objectStructure, marginHeight + i, marginWidth + j); //(dataStructure as ADataStructure).DataStructureCases[marginHeight + i, marginWidth + j]);
                        }
                    }
                }

                return objectStructure;
            }
            return null;
        }

        public virtual bool IsGenerationValidAtWorldPosition(IDataStructure dataStructure, int worldAltitude, IObjectChunk objectChunk)
        {
            IntRect structureWorldArea = dataStructure.StructureWorldBoundingBox;
            IntRect chunkWorldArea = ChunkHelper.GetWorldAreaFromChunkArea(objectChunk.NbCaseSide, new IntRect(objectChunk.Position.X, objectChunk.Position.Y, 1, 1));

            if (chunkWorldArea.Intersects(structureWorldArea, out IntRect overlapingArea))
            {
                int marginHeight = overlapingArea.Top - structureWorldArea.Top;
                int marginWidth = overlapingArea.Left - structureWorldArea.Left;

                for (int i = 0; i < overlapingArea.Height; i++)
                {
                    int y = overlapingArea.Top + i;
                    for (int j = 0; j < overlapingArea.Width; j++)
                    {
                        int x = overlapingArea.Left + j;

                        IZObjectCase zObjectCase = ChunkHelper.GetCaseAtWorldCoordinates(objectChunk, x, y) as IZObjectCase;

                        if (this.ValidateZObjectCase(zObjectCase, worldAltitude, marginHeight + i, marginWidth + j) == false)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        protected virtual bool ValidateZObjectCase(IZObjectCase zObjectCase, int worldAltitude, int baseLocalI, int baseLocalJ)
        {
            return true;
        }

        protected virtual IObjectStructure CreateObjectStructureFrom(Random random, string uid, IDataStructure dataStructure, int worldAltitude)
        {
            return new CaseObjectStructure(this.TemplateUID, uid, random.Next(), dataStructure.StructureWorldPosition, worldAltitude);
        }

        protected string ConstructeObjectStructureUID(Vector2i worldPosition, int worldAltitude)
        {
            return String.Concat(this.TemplateUID, "_", worldPosition.X, "-", worldPosition.Y, "-", worldAltitude);
        }

        protected abstract void UpdateZObjectCase(Random random, IZObjectCase zObjectCase, IDataStructure dataStructure, int worldAltitude, IObjectStructure parentObjectStructure, int i, int j);
    }
}
