﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.ModelChunks
{
    public interface IZModelCases : ICase
    {
        IModelCase this[int z]
        {
            get;
        }
    }
}