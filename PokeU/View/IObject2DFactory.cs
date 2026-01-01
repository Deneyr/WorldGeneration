using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SFML.System;
using System.Collections.Generic;
using WorldGeneration.ObjectChunks;

namespace PokeU.View
{
    public interface IObject2DFactory
    {
        IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Point position);

        Dictionary<string, (Texture2D, Rectangle)> Resources
        {
            get;
        }

        IObjectChunk CurrentObjectChunk
        {
            get;
            set;
        }

        (Texture2D, Rectangle) GetTextureByIndex(int index);

        void OnTextureLoaded(string path, (Texture2D, Rectangle) texture);

        void OnTextureUnloaded(string path);
    }
}
