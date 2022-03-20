using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.DataChunks.VoronoiNoise.BiomeVoronoiNoise
{
    internal class BiomeVoronoiDataCase : VoronoiDataCase
    {
        //private Dictionary<int, BiomeData> biomeToWeight;

        //private List<BiomeData> biomeDatas;

        //private float totalWeight;

        //private float ratio;

        public float BorderValue
        {
            get;
            set;
        }

        public BiomeVoronoiDataCase(int x, int y)
            : base(x, y)
        {
            //this.BorderValue = 1;
            //this.totalWeight = 0;
            //this.biomeToWeight = new Dictionary<int, BiomeData>();
            //this.biomeDatas = new List<BiomeData>();
        }

        //public void UpdateBiomeWeight(int biome, float weight)
        //{
        //    this.biomeDatas.Add(new BiomeData(biome, weight));

        //    if(this.biomeToWeight.TryGetValue(biome, out BiomeData biomeData))
        //    {
        //        //biomeData.BiomeWeight += weight;
        //        if (biomeData.BiomeWeight > weight)
        //        {
        //            this.totalWeight += (weight - biomeData.BiomeWeight);
        //            biomeData.BiomeWeight = weight;
        //        }
        //    }
        //    else
        //    {
        //        this.biomeToWeight.Add(biome, new BiomeData(biome, weight));
        //        this.totalWeight += weight;
        //    }
        //}

        //public void FinalCaseUpdate(float borderMax)
        //{
        //    float weight = this.biomeToWeight[this.Value].BiomeWeight;
        //    this.ratio = int.MaxValue;

        //    if (this.biomeToWeight.Count > 1)
        //    {
        //        foreach (KeyValuePair<int, BiomeData> biomesValuePair in this.biomeToWeight)
        //        {
        //            if(biomesValuePair.Key != this.Value)
        //            {
        //                biomesValuePair.Value.BiomeWeight = Math.Min(1, Math.Abs(weight - biomesValuePair.Value.BiomeWeight) / borderMax));
        //            }
        //        }

        //        this.ratio = Math.Min(this.ratio, 1);
        //    }
        //    else
        //    {
        //        this.biomeToWeight.First().Value.BiomeWeight = 1;
        //        this.totalWeight = 1;

        //        this.ratio = 1;
        //    }
        //}

        //private class BiomeData
        //{
        //    public int BiomeType
        //    {
        //        get;
        //        private set;
        //    }

        //    public float BiomeWeight
        //    {
        //        get;
        //        set;
        //    }

        //    public BiomeData(int biomeType, float biomeWeight)
        //    {
        //        this.BiomeType = biomeType;
        //        this.BiomeWeight = biomeWeight;
        //    }
        //}
    }
}
