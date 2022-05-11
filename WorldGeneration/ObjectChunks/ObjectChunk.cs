using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using WorldGeneration.ChunksMonitoring;

namespace WorldGeneration.ObjectChunks
{
    public class ObjectChunk : AObjectChunk
    {
        public ObjectChunk(Vector2i position, int nbCaseSide, int nbAltitudeLevel) 
            : base(position, nbCaseSide)
        {
            this.InitCaseArray(nbAltitudeLevel);
        }

        private void InitCaseArray(int nbAltitudeLevel)
        {
            this.ZCasesArray = new IZObjectCase[this.NbCaseSide, this.NbCaseSide];
            for (int i = 0; i < this.NbCaseSide; i++)
            {
                for (int j = 0; j < this.NbCaseSide; j++)
                {
                    Vector2i worldPosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new SFML.Graphics.IntRect(this.Position.X, this.Position.Y, j, i));

                    this.ZCasesArray[i, j] = new ZObjectCase(worldPosition, nbAltitudeLevel);
                }
            }
        }
    }
}
