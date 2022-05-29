using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.ObjectChunks.ObjectStructures
{
    internal abstract class ACaseObjectStructureTemplate : IStructureTemplate
    {
        protected int[,,] enumValueStructure;

        public string TemplateUID
        {
            get;
            private set;
        }

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

        public ACaseObjectStructureTemplate(string templateUID, Vector2i localBoundingBox, int maxLocalElevation, IntRect baseLocalBoundingBox)
        {
            this.TemplateUID = templateUID;

            this.LocalBoundingBox = localBoundingBox;
            this.MaxLocalElevation = maxLocalElevation;

            this.BaseLocalBoundingBox = baseLocalBoundingBox;

            //this.enumValueStructure = new int[this.LocalBoundingBox.Y, this.LocalBoundingBox.X, this.MaxLocalElevation];
            //for(int i = 0; i < this.LocalBoundingBox.Y; i++)
            //{
            //    for (int j = 0; j < this.LocalBoundingBox.X; j++)
            //    {
            //        for (int k = 0; k < this.MaxLocalElevation; k++)
            //        {
            //            this.enumValueStructure[i, j, k] = -1;
            //        }
            //    }
            //}
        }

        public IObjectStructure GenerateStructureAtWorldPosition(Random random, Vector2i worldPosition, int worldAltitude, IObjectChunk objectChunk)
        {
            IObjectStructure objectStructure = this.CreateObjectStructureFrom(random, worldPosition, worldAltitude);

            for (int i = 0; i < this.LocalBoundingBox.Y; i++)
            {
                int y = worldPosition.Y + i;
                for (int j = 0; j < this.LocalBoundingBox.X; j++)
                {
                    int x = worldPosition.X + j;

                    IZObjectCase zObjectCase = ChunkHelper.GetCaseAtWorldCoordinates(objectChunk, x, y) as IZObjectCase;
                    if (zObjectCase != null)
                    {
                        for (int k = 0; k < this.MaxLocalElevation; k++)
                        {
                            int z = worldAltitude + k;
                            if (this.enumValueStructure[k, i, j] != -1 && z < zObjectCase.NbAltitudeLevel)
                            {
                                IObjectCase objectCase = zObjectCase[z];

                                this.UpdateObjectCase(random, objectCase, objectStructure, this.enumValueStructure[k, i, j]);
                            }
                        }
                    }
                }
            }

            return objectStructure;
        }

        public virtual bool IsGenerationValidAtWorldPosition(Vector2i worldPosition, int worldAltitude, IObjectChunk objectChunk)
        {
            int StartWorldX = worldPosition.X + this.BaseLocalBoundingBox.Left;
            int StartWorldY = worldPosition.Y + this.BaseLocalBoundingBox.Top;

            for (int i = 0; i < this.BaseLocalBoundingBox.Height; i++)
            {
                int y = StartWorldY + i;
                for (int j = 0; j < this.BaseLocalBoundingBox.Width; j++)
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
            return worldAltitude + this.MaxLocalElevation - 1 < zObjectCase.NbAltitudeLevel;
        }

        protected virtual IObjectStructure CreateObjectStructureFrom(Random random, Vector2i worldPosition, int worldAltitude)
        {
            return new CaseObjectStructure(this.TemplateUID, random.Next(), worldPosition, worldAltitude);
        }

        protected abstract void UpdateObjectCase(Random random, IObjectCase objectCase, IObjectStructure parentObjectStructure, int enumValue);
    }
}
