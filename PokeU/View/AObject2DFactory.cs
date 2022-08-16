using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks;

namespace PokeU.View
{
    public abstract class AObject2DFactory: IObject2DFactory
    {
        private static readonly Texture BLANK_TEXTURE;

        private Dictionary<string, Texture> resources;

        protected HashSet<string> texturesPath;

        public IObjectChunk CurrentObjectChunk
        {
            get;
            set;
        }

        static AObject2DFactory()
        {
            BLANK_TEXTURE = new Texture((uint)MainWindow.MODEL_TO_VIEW, (uint)MainWindow.MODEL_TO_VIEW);
        }

        public AObject2DFactory()
        {
            this.texturesPath = new HashSet<string>();

            this.InitializeFactory();
        }

        protected virtual void InitializeFactory()
        {
            this.resources = new Dictionary<string, Texture>();
            Texture blankTexture = BLANK_TEXTURE;
            foreach (string texturesPath in this.texturesPath)
            {
                this.resources.Add(texturesPath, blankTexture);
            }
        }

        public abstract IObject2D CreateObject2D(LandWorld2D landWorld2D, object obj, Vector2i position);

        public Dictionary<string, Texture> Resources
        {
            get
            {
                return this.resources;
            }
        }

        public Texture GetTextureByIndex(int index)
        {
            return this.Resources[this.texturesPath.ElementAt(index)];
        }

        public void OnTextureLoaded(string path, Texture texture)
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
                this.Resources[path] = BLANK_TEXTURE;
            }
        }
    }
}
