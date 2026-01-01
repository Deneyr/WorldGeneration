using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Linq;
using WorldGeneration.ObjectChunks;

namespace PokeU.View
{
    public abstract class AObject2DFactory: IObject2DFactory
    {
        private Dictionary<string, (Texture2D, Rectangle)> resources;

        protected HashSet<string> texturesPath;

        public IObjectChunk CurrentObjectChunk
        {
            get;
            set;
        }

        public AObject2DFactory()
        {
            this.texturesPath = new HashSet<string>();

            this.InitializeFactory();
        }

        protected virtual void InitializeFactory()
        {
            this.resources = new Dictionary<string, (Texture2D, Rectangle)>();
            //Texture2D blankTexture = BLANK_TEXTURE;
            foreach (string texturesPath in this.texturesPath)
            {
                this.resources.Add(texturesPath, (null, Rectangle.Empty));
            }
        }

        public abstract IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Point position);

        public Dictionary<string, (Texture2D, Rectangle)> Resources
        {
            get
            {
                return this.resources;
            }
        }

        public (Texture2D, Rectangle) GetTextureByIndex(int index)
        {
            return this.Resources[this.texturesPath.ElementAt(index)];
        }

        public void OnTextureLoaded(string path, (Texture2D, Rectangle) texture)
        {
            if (this.Resources.ContainsKey(path))
            {
                this.Resources[path] = texture;
            }
        }

        public void OnTextureUnloaded(string path)
        {
            if (this.Resources.ContainsKey(path))
            {
                this.Resources[path] = (null, Rectangle.Empty);
            }
        }
    }
}
