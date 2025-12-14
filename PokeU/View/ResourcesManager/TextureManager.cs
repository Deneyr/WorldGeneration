using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PokeU.View.ResourcesManager
{
    public class TextureManager
    {
        private Dictionary<string, Texture2D> texturesDictionary;

        public event Action<string, Texture2D> TextureLoaded;

        public event Action<string> TextureUnloaded;

        public Game MainGame
        {
            get;
            set;
        }

        public TextureManager()
        {
            this.texturesDictionary = new Dictionary<string, Texture2D>();
        }

        public Texture2D GetTexture(string path)
        {
            return this.texturesDictionary[path];
        }

        public void LoadTextures(HashSet<string> texturesToLoad)
        {
            foreach(string path in texturesToLoad)
            {
                if (this.texturesDictionary.ContainsKey(path) == false)
                {
                    Texture2D texture = null;
                    texture = Texture2D.FromFile(this.MainGame.GraphicsDevice, path + ".png");

                    if (texture != null)
                    {
                        this.texturesDictionary.Add(path, texture);

                        this.NotifyTextureLoaded(path, texture);
                    }
                }
            }
        }

        public void UnloadTextures(HashSet<string> texturesToUnload)
        {
            foreach (string path in texturesToUnload)
            {
                if (this.texturesDictionary.ContainsKey(path))
                {
                    this.texturesDictionary[path].Dispose();

                    this.texturesDictionary.Remove(path);

                    this.NotifyTextureUnloaded(path);
                }
            }
        }

        private void NotifyTextureLoaded(string path, Texture2D texture)
        {
            if(this.TextureLoaded != null)
            {
                this.TextureLoaded(path, texture);
            }
        }

        private void NotifyTextureUnloaded(string path)
        {
            if (this.TextureUnloaded != null)
            {
                this.TextureUnloaded(path);
            }
        }
    }
}
