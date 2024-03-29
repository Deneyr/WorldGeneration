﻿using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.DataChunks.StructureNoise.DataStructure
{
    internal interface IDataStructure
    {
        string ObjectStructureTemplateId
        {
            get;
        }

        int StructureTypeIndex
        {
            get;
        }

        Vector2i StructureWorldPosition
        {
            get;
        }

        Vector2i StructureWorldCenter
        {
            get;
        }

        BiomeType StructureBiome
        {
            get;
        }

        IntRect StructureBoundingBox
        {
            get;
        }

        IntRect StructureWorldBoundingBox
        {
            get;
        }

        IntRect StructureBaseBoundingBox
        {
            get;
        }

        IntRect StructureWorldBaseBoundingBox
        {
            get;
        }

        void GenerateStructure(Random random, IDataStructureTemplate structureTemplate);

        IDataStructureCase GetStructureCaseAtChunkCoordinate(int x, int y);
    }
}
