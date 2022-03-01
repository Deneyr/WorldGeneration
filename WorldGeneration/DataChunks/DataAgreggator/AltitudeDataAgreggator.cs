using System;
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

        public AltitudeDataAgreggator(int nbAltitudeLevel)
        {
            this.NbAltitudeLevel = nbAltitudeLevel;
            this.AltitudeLayers = new List<Tuple<float, IDataChunkLayer>>();
        }

        public int GetAltitudeAtWorldCoordinates(int x, int y)
        {
            //PerlinDataCase dataCase = this.DataChunkLayersMonitor.DataChunksLayers["landscape"].GetCaseAtWorldCoordinates(x, y) as PerlinDataCase;
            //PerlinDataCase dataCase2 = this.DataChunkLayersMonitor.DataChunksLayers["landscapeLevel2"].GetCaseAtWorldCoordinates(x, y) as PerlinDataCase;
            //PerlinDataCase dataCase3 = this.DataChunkLayersMonitor.DataChunksLayers["landscapeLevel3"].GetCaseAtWorldCoordinates(x, y) as PerlinDataCase;
            //PerlinDataCase dataCase4 = this.DataChunkLayersMonitor.DataChunksLayers["landscapeLevel4"].GetCaseAtWorldCoordinates(x, y) as PerlinDataCase;

            //float altitudeValue = dataCase.Value + dataCase2.Value * 0.5f + dataCase3.Value * 0.25f + dataCase4.Value * 0.15f;
            float altitudeValue = 0;

            foreach(Tuple<float, IDataChunkLayer> altitudeLayer in this.AltitudeLayers)
            {
                PerlinDataCase dataCase = altitudeLayer.Item2.GetCaseAtWorldCoordinates(x, y) as PerlinDataCase;
                altitudeValue += altitudeLayer.Item1 * dataCase.Value;
            }

            altitudeValue = (altitudeValue + 1) / 2;

            return (int) (altitudeValue * this.NbAltitudeLevel);

            //// TEST
            //Random random = new Random();
            //return random.Next(0, this.NbAltitudeLevel);
        }

        internal void AddAltitudeLayer(float weight, IDataChunkLayer altitudeLayer)
        {
            this.AltitudeLayers.Add(new Tuple<float, IDataChunkLayer>(weight, altitudeLayer));
        }

    }
}
