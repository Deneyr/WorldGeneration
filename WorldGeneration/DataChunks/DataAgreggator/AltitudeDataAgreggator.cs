using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.PerlinNoise;
using WorldGeneration.DataChunks.WeatherMonitoring;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class AltitudeDataAgreggator: IDataAgreggator
    {
        private AltitudeBiomeMonitor altitudeBiomeMonitor;

        public int NbAltitudeLevel
        {
            get;
            private set;
        }

        internal List<Tuple<float, IDataChunkLayer>> AltitudeLayers
        {
            get;
            private set;
        }

        internal Tuple<float, IDataChunkLayer> SeaAltitudeLayer
        {
            get;
            private set;
        }

        internal BiomeDataAgreggator BiomeDataAgreggator
        {
            get;
            set;
        }

        public AltitudeDataAgreggator(int nbAltitudeLevel)
        {
            this.NbAltitudeLevel = nbAltitudeLevel;
            this.AltitudeLayers = new List<Tuple<float, IDataChunkLayer>>();
            this.SeaAltitudeLayer = new Tuple<float, IDataChunkLayer>(0, null);

            this.altitudeBiomeMonitor = new AltitudeBiomeMonitor();
        }

        public int GetAltitudeAtWorldCoordinates(int x, int y, out bool isUnderSea)
        {
            isUnderSea = false;
            //PerlinDataCase dataCase = this.DataChunkLayersMonitor.DataChunksLayers["landscape"].GetCaseAtWorldCoordinates(x, y) as PerlinDataCase;
            //PerlinDataCase dataCase2 = this.DataChunkLayersMonitor.DataChunksLayers["landscapeLevel2"].GetCaseAtWorldCoordinates(x, y) as PerlinDataCase;
            //PerlinDataCase dataCase3 = this.DataChunkLayersMonitor.DataChunksLayers["landscapeLevel3"].GetCaseAtWorldCoordinates(x, y) as PerlinDataCase;
            //PerlinDataCase dataCase4 = this.DataChunkLayersMonitor.DataChunksLayers["landscapeLevel4"].GetCaseAtWorldCoordinates(x, y) as PerlinDataCase;

            //float altitudeValue = dataCase.Value + dataCase2.Value * 0.5f + dataCase3.Value * 0.25f + dataCase4.Value * 0.15f;
            float altitudeValue = 0;
            //float firstAltitudeValue = 0;
            //float maxRatio = float.MinValue;
            foreach (Tuple<float, IDataChunkLayer> altitudeLayer in this.AltitudeLayers)
            {
                PerlinDataCase dataCase = altitudeLayer.Item2.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase;
                float layerValue = altitudeLayer.Item1 * dataCase.Value;
                altitudeValue += layerValue;

                //if(altitudeLayer.Item1 > maxRatio)
                //{
                //    maxRatio = altitudeLayer.Item1;
                //    firstAltitudeValue = layerValue;
                //}
            }

            altitudeValue = (altitudeValue + 1) / 2;

            //altitudeValue = this.computeAltitude(altitudeValue);

            int altitudeLevel = (int)(altitudeValue * this.NbAltitudeLevel);

            //if(altitudeLevel <= (this.NbAltitudeLevel / 2)
            //    && firstAltitudeValue < 0)
            //{
            //    isUnderSea = true;
            //}

            int midAltitudeLevel = this.NbAltitudeLevel / 2;
            float seaValue = (this.SeaAltitudeLayer.Item2.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase).Value;
            if (altitudeLevel < midAltitudeLevel)
            {
                if (altitudeLevel == midAltitudeLevel - 1
                    && this.SeaAltitudeLayer.Item2 != null)
                {
                    float seaAltitudeValue = altitudeValue + seaValue * this.SeaAltitudeLayer.Item1;

                    if (seaAltitudeValue < 0.5)
                    {
                        isUnderSea = true;
                    }
                }
                else
                {
                    isUnderSea = true;
                }
            }
            //else if (altitudeLevel >= midAltitudeLevel)
            //{
            //    if (seaValue > 0)
            //    {
            //        //float squareAltitudeLevel = midAltitudeLevel / 2;
            //        //float squareAltitudeValue = squareAltitudeLevel / this.NbAltitudeLevel;
            //        float ratioAltitude = Math.Max(0, (0.2f - Math.Abs(altitudeValue - 0.7f)) / 0.2f);

            //        ratioAltitude = ratioAltitude * ratioAltitude;

            //        float value = ((float)Math.Sqrt(seaValue)) * 0.5f * ratioAltitude;

            //        altitudeValue += value;
            //    }
            //}

            altitudeValue = Math.Min(1, altitudeValue);

            if(altitudeValue >= 0.5f)
            {
                BiomeType biomeType = this.BiomeDataAgreggator.GetBiomeAtWorldCoordinates(x, y, out float borderValue);
                float newAltitudeValue = this.altitudeBiomeMonitor.FilterAltitudeFromBiome(biomeType, (altitudeValue - 0.5f) * 2f) / 2f + 0.5f;

                altitudeValue = altitudeValue * (1 - borderValue) + borderValue * newAltitudeValue;
            }

            altitudeLevel = (int)(altitudeValue * this.NbAltitudeLevel);

            return altitudeLevel;

            //// TEST
            //Random random = new Random();
            //return random.Next(0, this.NbAltitudeLevel);
        }

        private float computeAltitude(float altitudeValue)
        {
            if(altitudeValue < 0.33f)
            {
                return 1.25f * altitudeValue;
            }
            else if(altitudeValue < 0.66f)
            {
                return 0.5f * (altitudeValue - 0.5f) + 0.5f;
            }
            else
            {
                return 1.25f * (altitudeValue - 0.333f) + 0.167f;
            }
        }

        internal void AddAltitudeLayer(float weight, IDataChunkLayer altitudeLayer)
        {
            this.AltitudeLayers.Add(new Tuple<float, IDataChunkLayer>(weight, altitudeLayer));
        }

        internal void AddSeaLayer(float weight, IDataChunkLayer altitudeLayer)
        {
            this.SeaAltitudeLayer = new Tuple<float, IDataChunkLayer>(weight, altitudeLayer);
        }

    }
}
