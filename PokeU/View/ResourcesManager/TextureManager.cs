using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View.ResourcesManager
{
    public class TextureManager
    {
        private Dictionary<string, Texture> texturesDictionary;

        public event Action<string, Texture> TextureLoaded;

        public event Action<string> TextureUnloaded;

        public TextureManager()
        {
            this.texturesDictionary = new Dictionary<string, Texture>();
        }

        public Texture GetTexture(string path)
        {
            return this.texturesDictionary[path];
        }

        public void LoadTextures(HashSet<string> texturesToLoad)
        {
            foreach(string path in texturesToLoad)
            {
                if (this.texturesDictionary.ContainsKey(path) == false)
                {
                    Texture texture = new Texture(path);

                    this.texturesDictionary.Add(path, texture);

                    this.NotifyTextureLoaded(path, texture);
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

        private void NotifyTextureLoaded(string path, Texture texture)
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
