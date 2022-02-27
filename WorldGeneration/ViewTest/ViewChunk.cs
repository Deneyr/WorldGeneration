using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.PerlinNoise;

namespace WorldGeneration.ViewTest
{
    public class ViewChunk : IChunk
    {
        private RectangleShape[,] caseArray;

        public int NbCaseSide
        {
            get;
            private set;
        }

        public Vector2i Position
        {
            get;
            private set;
        }

        public ICase[,] CasesArray
        {
            get;
        }

        public ViewChunk(IChunk chunk)
        {
            this.NbCaseSide = chunk.CasesArray.GetLength(0);
            this.Position = chunk.Position;

            this.caseArray = new RectangleShape[this.NbCaseSide, this.NbCaseSide];

            for (int i = 0; i < this.NbCaseSide; i++)
            {
                for (int j = 0; j < this.NbCaseSide; j++)
                {
                    PerlinDataCase perlinCase = ChunkHelper.GetCaseAtLocalCoordinates(chunk, j, i) as PerlinDataCase;

                    RectangleShape rectangle = new RectangleShape(new Vector2f(16, 16));
                    byte colorValue = (byte) ((perlinCase.Value + 1) / 2 * 255);
                    if (colorValue < 140)
                    {
                        colorValue = 0;
                    }
                    rectangle.FillColor = new Color(colorValue, colorValue, colorValue);

                    Vector2i modelPosition = ChunkHelper.GetWorldPositionFromChunkPosition(this.NbCaseSide, new IntRect(chunk.Position.X, chunk.Position.Y, j, i));
                    rectangle.Position = new Vector2f(modelPosition.X * ViewMonitor.MODEL_TO_VIEW, modelPosition.Y * ViewMonitor.MODEL_TO_VIEW);

                    this.caseArray[i, j] = rectangle;
                }
            }
        }

        public void DrawIn(RenderWindow window, Time deltaTime)
        {
            foreach(RectangleShape rectangleShape in this.caseArray)
            {
                window.Draw(rectangleShape);
            }
        }
    }
}
