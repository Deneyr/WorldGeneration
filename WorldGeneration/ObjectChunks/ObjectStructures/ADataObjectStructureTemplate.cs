﻿using SFML.Graphics;
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
    internal abstract class ADataObjectStructureTemplate : IStructureTemplate
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
                IObjectStructure objectStructure = this.CreateObjectStructureFrom(random, dataStructure.StructureWorldPosition, worldAltitude);

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
                            this.UpdateObjectCase(random, zObjectCase, worldAltitude, objectStructure, (dataStructure as ADataStructure).DataStructureCases[marginHeight + i, marginWidth + j]);
                        }
                    }
                }

                return objectStructure;
            }
            return null;
        }

        public virtual bool IsGenerationValidAtWorldPosition(IDataStructure dataStructure, int worldAltitude, IObjectChunk objectChunk)
        {
            Vector2i worldPosition = dataStructure.StructureWorldPosition;

            int structureHeight = dataStructure.StructureBoundingBox.Height;
            int structureWidth = dataStructure.StructureBoundingBox.Width;

            int StartWorldX = worldPosition.X + structureWidth;
            int StartWorldY = worldPosition.Y + structureHeight;

            for (int i = 0; i < structureHeight; i++)
            {
                int y = StartWorldY + i;
                for (int j = 0; j < structureWidth; j++)
                {
                    int x = StartWorldX + j;

                    IZObjectCase zObjectCase = ChunkHelper.GetCaseAtWorldCoordinates(objectChunk, x, y) as IZObjectCase;

                    if (this.ValidateZObjectCase(zObjectCase, worldAltitude, i, j) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected virtual bool ValidateZObjectCase(IZObjectCase zObjectCase, int worldAltitude, int baseLocalI, int baseLocalJ)
        {
            return true;
        }

        protected virtual IObjectStructure CreateObjectStructureFrom(Random random, Vector2i worldPosition, int worldAltitude)
        {
            return new CaseObjectStructure(this.TemplateUID, random.Next(), worldPosition, worldAltitude);
        }

        protected abstract void UpdateObjectCase(Random random, IZObjectCase zObjectCase, int worldAltitude, IObjectStructure parentObjectStructure, IDataStructureCase dataStructureCase);
    }
}
