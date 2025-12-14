using Microsoft.Xna.Framework.Graphics;
using SFML.System;
using System.Collections.Generic;
using WorldGeneration.ObjectChunks;

namespace PokeU.View
{
    public interface IObject2DFactory
    {
        IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Vector2i position);

        Dictionary<string, Texture2D> Resources
        {
            get;
        }

        IObjectChunk CurrentObjectChunk
        {
            get;
            set;
        }

        Texture2D GetTextureByIndex(int index);

        void OnTextureLoaded(string path, Texture2D texture);

        void OnTextureUnloaded(string path);
    }
}
