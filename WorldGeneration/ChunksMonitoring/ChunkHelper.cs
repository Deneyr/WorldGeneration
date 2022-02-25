using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ChunksMonitoring
{
    public static class ChunkHelper
    {
        public static IntRect GetChunkPositionFromWorldPosition(int nbCaseSide, Vector2i worldPosition)
        {
            Vector2i chunkPosition = new Vector2i((int)Math.Floor(worldPosition.X / ((float)nbCaseSide)), (int)Math.Floor(worldPosition.Y / ((float)nbCaseSide)));
            Vector2i positionInChunk = worldPosition - (chunkPosition * nbCaseSide);

            return new IntRect(chunkPosition, positionInChunk);
        }

        public static Vector2i GetWorldPositionFromChunkPosition(int nbCaseSide, IntRect chunkPosition)
        {
            return (new Vector2i(chunkPosition.Left, chunkPosition.Top)) * nbCaseSide + (new Vector2i(chunkPosition.Width, chunkPosition.Height));
        }

        public static ICase GetCaseAtLocalCoordinate(IChunk chunk, int x, int y)
        {
            if (x >= 0
                && x < chunk.NbCaseSide
                && y >= 0
                && y < chunk.NbCaseSide)
            {
                return chunk.CasesArray[y, x];
            }
            return null;
        }

        public static ICase GetCaseAtWorldCoordinate(IChunk chunk, int x, int y)
        {
            IntRect chunkPosition = GetChunkPositionFromWorldPosition(chunk.NbCaseSide, new Vector2i(x, y));

            if (chunkPosition.Left == chunk.Position.X
                && chunkPosition.Top == chunk.Position.Y)
            {
                return chunk.CasesArray[chunkPosition.Width, chunkPosition.Height];
            }
            return null;
        }

        public static IntRect GetWorldAreaFromChunkArea(int nbCaseSide, IntRect chunkArea)
        {
            return new IntRect(
                chunkArea.Left * nbCaseSide,
                chunkArea.Top * nbCaseSide,
                chunkArea.Width * nbCaseSide,
                chunkArea.Height * nbCaseSide
                );
        }

        public static IntRect GetChunkAreaFromWorldArea(int nbCaseSide, IntRect worldArea)
        {
            IntRect chunkArea = new IntRect(
                (int)Math.Floor(worldArea.Left / ((float)nbCaseSide)),
                (int)Math.Floor(worldArea.Top / ((float)nbCaseSide)),
                (int)Math.Ceiling((worldArea.Left + worldArea.Width) / ((float)nbCaseSide)),
                (int)Math.Ceiling((worldArea.Top + worldArea.Height) / ((float)nbCaseSide))
                );

            chunkArea.Width -= chunkArea.Left;
            chunkArea.Height -= chunkArea.Top;

            return chunkArea;
        }
    }
}
