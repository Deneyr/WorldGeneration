﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.DataChunks.PerlinNoise;

namespace WorldGeneration.DataChunks.DataAgreggator
{
    internal class AltitudeDataAgreggator: IDataAgreggator
    {
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

        public AltitudeDataAgreggator(int nbAltitudeLevel)
        {
            this.NbAltitudeLevel = nbAltitudeLevel;
            this.AltitudeLayers = new List<Tuple<float, IDataChunkLayer>>();
            this.SeaAltitudeLayer = new Tuple<float, IDataChunkLayer>(0, null);
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

            altitudeValue = this.computeAltitude(altitudeValue);

            int altitudeLevel = (int)(altitudeValue * this.NbAltitudeLevel);

            //if(altitudeLevel <= (this.NbAltitudeLevel / 2)
            //    && firstAltitudeValue < 0)
            //{
            //    isUnderSea = true;
            //}

            if(altitudeValue < 0.5)
            {
                if (this.SeaAltitudeLayer.Item2 != null)
                {
                    float seaAltitudeValue = altitudeValue + (this.SeaAltitudeLayer.Item2.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase).Value * this.SeaAltitudeLayer.Item1;

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

            return altitudeLevel;

            //// TEST
            //Random random = new Random();
            //return random.Next(0, this.NbAltitudeLevel);
        }

        private float computeAltitude(float altitudeValue)
        {
            //if(altitudeValue < 0.55)
            //{
            //    return altitudeValue;
            //}
            //else
            //{
            //    return (altitudeValue - 0.55f) * (altitudeValue - 0.55f) * 0.45f + 0.55f;
            //}
            //double a = altitudeValue - 0.5;
            //return (float)(Math.Tan(a) + 0.5);

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
                return 1.25f * (altitudeValue - 0.33f) + 1.167f;
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
