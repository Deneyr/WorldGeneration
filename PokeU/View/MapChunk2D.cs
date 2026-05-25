using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;

namespace PokeU.View
{
    public class MapChunk2D : AObject2D, IChunk
    {
        private MapWorld2D parentMapWorld2D;

        private RenderTarget2D renderTarget;

        private Rectangle destinationRectangle;
        private Matrix scaleMatrix;

        private string fileName;

        Vector2i IChunk.Position => throw new NotImplementedException();

        public int NbCaseSide => throw new NotImplementedException();

        public LandChunk2D LandChunk2D
        {
            get;
            set;
        }

        public MapChunk2D(MapWorld2D parentMapWorld2D, Vector2i chunkPosition, int chunkWidth, int chunkHeight)
        {
            this.parentMapWorld2D = parentMapWorld2D;
            this.scaleMatrix = Matrix.CreateScale(this.parentMapWorld2D.ResolutionScale, this.parentMapWorld2D.ResolutionScale, 1f);

            this.TextureRect = new Rectangle(0, 0, chunkWidth, chunkHeight);
            this.Position = new Vector2(chunkPosition.X * chunkWidth, chunkPosition.Y * chunkHeight);
            this.fileName = $"({this.Position.X}_{this.Position.Y}).png";

            this.destinationRectangle = new Rectangle(
                (int)this.Position.X,
                (int)this.Position.Y,
                chunkWidth,
                chunkHeight);
        }

        public void Render()
        {
            if (this.LandChunk2D == null)
            {
                Debug.Fail("Map chunk 2D is trying to render without parent land chunk 2D");
                return;
            }

            if(this.renderTarget == null)
            {
                this.renderTarget = new RenderTarget2D(this.parentMapWorld2D.GraphicsDevice, (int)(this.LandChunk2D.Width * this.parentMapWorld2D.ResolutionScale), (int)(this.LandChunk2D.Height * this.parentMapWorld2D.ResolutionScale));
            }

            this.parentMapWorld2D.GraphicsDevice.SetRenderTarget(this.renderTarget);
            this.parentMapWorld2D.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.PaleVioletRed);

            FloatRect viewBound = this.ViewBound;
            this.parentMapWorld2D.SpriteBatch.Begin(
                blendState: BlendState.NonPremultiplied, 
                samplerState: SamplerState.PointClamp,
                transformMatrix: this.scaleMatrix);

            this.LandChunk2D.RenderIn(new Vector2(), this.parentMapWorld2D.SpriteBatch, ref viewBound);

            this.parentMapWorld2D.SpriteBatch.End();
            this.parentMapWorld2D.GraphicsDevice.SetRenderTarget(null);
        }

        public override void DrawIn(SpriteBatch spriteBatch, ref FloatRect boundsView)
        {
            Texture2D lTextureToDraw = (this.Texture.Item1 ?? this.renderTarget) ?? this.parentMapWorld2D.DefaultTexture;

            if (lTextureToDraw != null)
            {
                spriteBatch.Draw(
                    texture: lTextureToDraw,
                    destinationRectangle: this.destinationRectangle,
                    color: this.Color);
            }
        }

        public bool DoesTextureFileExist(string folderPath)
        {
            return File.Exists($"{folderPath}{Path.DirectorySeparatorChar}{this.fileName}");
        }

        public async Task SaveTextureAsync(string folderPath)
        {
            Microsoft.Xna.Framework.Color[] colors = new Microsoft.Xna.Framework.Color[renderTarget.Width * renderTarget.Height];
            renderTarget.GetData(colors);

            using Texture2D texture = new Texture2D(this.parentMapWorld2D.GraphicsDevice, renderTarget.Width, renderTarget.Height);
            texture.SetData(colors);

            using MemoryStream memoryStream = new MemoryStream();
            texture.SaveAsPng(memoryStream, texture.Width, texture.Height);
            byte[] pngBytes = memoryStream.ToArray();

            await Task.Run(async () =>
            {
                string filePath = $"{folderPath}{Path.DirectorySeparatorChar}{this.fileName}";
                string tempPath = $"{filePath}.tmp";

                await using FileStream fileStream = new FileStream(
                    tempPath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None);

                await fileStream.WriteAsync(pngBytes);
                await fileStream.FlushAsync();
                fileStream.Close();

                File.Move(tempPath, filePath, overwrite: true);
            });
        }

        public async Task LoadTextureAsync(string folderPath)
        {
            string filePath = $"{folderPath}{Path.DirectorySeparatorChar}{this.fileName}";
            if (!File.Exists(filePath))
                return;

            byte[] pngBytes = await Task.Run(async () =>
            {
                await using FileStream fileStream = new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None);

                byte[] buffer = new byte[fileStream.Length];
                await fileStream.ReadAsync(buffer);
                return buffer;
            });

            using MemoryStream memoryStream = new MemoryStream(pngBytes);
            Texture2D lLoadedTexture = Texture2D.FromStream(this.parentMapWorld2D.GraphicsDevice, memoryStream);
            this.Texture = (lLoadedTexture, new Rectangle(0, 0, lLoadedTexture.Width, lLoadedTexture.Height));
        }

        public ICase GetCaseAtLocal(int x, int y)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            if (this.renderTarget != null)
            {
                this.renderTarget.Dispose();
            }

            base.Dispose();
        }
    }
}
