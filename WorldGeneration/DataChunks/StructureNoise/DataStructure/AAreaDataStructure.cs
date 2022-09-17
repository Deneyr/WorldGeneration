using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.DataChunks.StructureNoise.DataStructure
{
    internal abstract class AAreaDataStructure : IDataStructure
    {
        public int StructureTypeIndex
        {
            get;
            internal set;
        }

        public string ObjectStructureTemplateId
        {
            get;
            protected set;
        }

        public IDataStructureCase[,] DataStructureCases
        {
            get;
            internal set;
        }

        public Vector2i StructureWorldPosition
        {
            get;
            private set;
        }

        public Vector2i StructureWorldCenter
        {
            get
            {
                return new Vector2i(this.StructureWorldPosition.X + this.StructureBoundingBox.Width / 2, this.StructureWorldPosition.Y + this.StructureBoundingBox.Height / 2);
            }
        }

        public BiomeType StructureBiome
        {
            get;
            protected set;
        }

        public IntRect StructureBoundingBox
        {
            get;
            protected set;
        }

        public IntRect StructureBaseBoundingBox
        {
            get;
            private set;
        }

        public IntRect StructureWorldBoundingBox
        {
            get
            {
                return new IntRect(
                    this.StructureWorldPosition.X,
                    this.StructureWorldPosition.Y,
                    this.StructureBoundingBox.Width,
                    this.StructureBoundingBox.Height);
            }
        }

        public IntRect StructureWorldBaseBoundingBox
        {
            get
            {
                return new IntRect(
                    this.StructureWorldPosition.X + this.StructureBaseBoundingBox.Left,
                    this.StructureWorldPosition.Y + this.StructureBaseBoundingBox.Top,
                    this.StructureBaseBoundingBox.Width,
                    this.StructureBaseBoundingBox.Height);
            }
        }

        public AAreaDataStructure(Vector2i structureWorldPosition, IntRect structureBoundingBox, IntRect structureBaseBoundingBox)
        {
            this.StructureWorldPosition = structureWorldPosition;
            this.StructureBiome = BiomeType.BOREAL_FOREST;

            this.StructureBoundingBox = structureBoundingBox;
            this.DataStructureCases = new IDataStructureCase[structureBoundingBox.Height, structureBoundingBox.Width];

            this.StructureBaseBoundingBox = structureBaseBoundingBox;
        }

        public abstract void GenerateStructure(Random random, IDataStructureTemplate structureTemplate);

        public IDataStructureCase GetStructureCaseAtChunkCoordinate(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
