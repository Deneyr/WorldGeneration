using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PokeU.View.ResourcesManager
{
    public class TextureManager
    {
        private AtlasManager atlasManager;

        private Dictionary<string, (Texture2D, Rectangle)> texturesDictionary;

        public event Action<string, (Texture2D, Rectangle)> TextureLoaded;

        public event Action<string> TextureUnloaded;

        public Game MainGame
        {
            get;
            set;
        }

        public TextureManager(GraphicsDevice graphicsDevice)
        {
            this.atlasManager = new AtlasManager(graphicsDevice, 2048, 2048);

            this.texturesDictionary = new Dictionary<string, (Texture2D, Rectangle)>();
        }

        public (Texture2D, Rectangle) GetTexture(string path)
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
                        this.atlasManager.AddTexture(path, texture);
                        (Texture2D, Rectangle) loadedTexture = this.atlasManager.GetTextureRegion(path);

                        this.texturesDictionary.Add(path, loadedTexture);

                        this.NotifyTextureLoaded(path, loadedTexture);
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
                    //this.texturesDictionary[path].Dispose();

                    this.atlasManager.Remove(path);

                    this.texturesDictionary.Remove(path);

                    this.NotifyTextureUnloaded(path);
                }
            }
        }

        private void NotifyTextureLoaded(string path, (Texture2D, Rectangle) textureLoaded)
        {
            if(this.TextureLoaded != null)
            {
                this.TextureLoaded(path, textureLoaded);
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
