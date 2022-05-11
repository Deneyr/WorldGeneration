using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.DataAgreggator;

namespace WorldGeneration.ObjectChunks.ObjectChunkLayers
{
    internal class AltitudeObjectChunkLayer : AObjectChunkLayer
    {
        public AltitudeObjectChunkLayer(string id) 
            : base(id)
        {
        }

        public override void ComputeObjectChunk(ObjectChunkLayersMonitor objectChunksMonitor, IObjectChunk objectChunk)
        {
            int chunkSeed = this.GenerateChunkSeed(objectChunk, objectChunksMonitor.WorldSeed);
            Random random = new Random(chunkSeed);

            AltitudeDataAgreggator altitudeDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["altitude"] as AltitudeDataAgreggator);
            RiverDataAgreggator riverDataAgreggator = (objectChunksMonitor.DataChunkMonitor.DataAgreggators["river"] as RiverDataAgreggator);

            int seaAltitude = altitudeDataAgreggator.SeaLevel;

            for (int i = 0; i < objectChunk.NbCaseSide; i++)
            {
                for (int j = 0; j < objectChunk.NbCaseSide; j++)
                {
                    IZObjectCase zObjectCase = objectChunk.GetCaseAtLocal(j, i) as IZObjectCase;

                    int altitude = altitudeDataAgreggator.GetAltitudeAtWorldCoordinates(zObjectCase.Position.X, zObjectCase.Position.Y, out bool isUnderSea);

                    int riverDepth = 0;
                    if (altitude < 22)
                    {
                        float riverRatio = riverDataAgreggator.GetRiverValueAtWorldCoordinates(zObjectCase.Position.X, zObjectCase.Position.Y);
                        riverDepth = (int)Math.Ceiling(riverRatio * 4);
                        riverDepth = Math.Min(Math.Max(altitude - seaAltitude + 1, 0), riverDepth);
                    }

                    altitude = altitude - riverDepth;

                    ObjectCase objectCase = new ObjectCase(zObjectCase.Position, altitude);
                    zObjectCase.SetCaseAt(objectCase, altitude);

                    if (isUnderSea)
                    {
                        for(int a = altitude; a <= seaAltitude; a++)
                        {
                            if(zObjectCase[a] == null)
                            {
                                objectCase = new ObjectCase(zObjectCase.Position, a);
                                zObjectCase.SetCaseAt(objectCase, a);
                            }

                            (zObjectCase[a] as ObjectCase).IsUnderSea = true;
                        }
                    }
                    else if(riverDepth > 0)
                    {
                        if(altitude == seaAltitude)
                        {
                            riverDepth = 1;
                        }

                        for (int a = 0; a <= riverDepth; a++)
                        {
                            int newAltitude = altitude + a;
                            if (zObjectCase[newAltitude] == null)
                            {
                                objectCase = new ObjectCase(zObjectCase.Position, newAltitude);
                                zObjectCase.SetCaseAt(objectCase, newAltitude);
                            }

                            (zObjectCase[newAltitude] as ObjectCase).IsUnderSea = true;
                        }
                    }
                }
            }
        }
    }
}
