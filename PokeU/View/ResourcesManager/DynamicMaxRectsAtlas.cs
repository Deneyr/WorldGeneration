using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PokeU.View.ResourcesManager
{
    public class DynamicMaxRectsAtlas : IDisposable
    {
        private readonly Texture2D _texture;
        private readonly Dictionary<string, Rectangle> _regions;
        private readonly List<Rectangle> _freeRects;

        public Texture2D Texture => _texture;
        public bool IsEmpty => _regions.Count == 0;

        public DynamicMaxRectsAtlas(GraphicsDevice graphicsDevice, int width, int height)
        {
            _texture = new Texture2D(graphicsDevice, width, height);
            _regions = new Dictionary<string, Rectangle>();
            _freeRects = new List<Rectangle>
        {
            new Rectangle(0, 0, width, height)
        };

            ClearAtlas();
        }

        private void ClearAtlas()
        {
            var data = new Color[_texture.Width * _texture.Height];
            Array.Fill(data, Color.Transparent);
            _texture.SetData(data);
        }

        #region Add / Remove

        public bool TryAdd(string key, Texture2D source, out Rectangle region)
        {
            region = Rectangle.Empty;

            if (_regions.ContainsKey(key))
                return false;

            if (!FindPosition(source.Width, source.Height, out region))
                return false;

            CopyTexture(source, region);
            _regions[key] = region;

            SplitFreeRects(region);
            PruneFreeRects();

            return true;
        }

        public bool Remove(string key)
        {
            if (!_regions.TryGetValue(key, out var region))
                return false;

            _regions.Remove(key);

            // Réinjecte l'espace comme libre
            _freeRects.Add(region);
            PruneFreeRects();

            ClearRegion(region);
            return true;
        }

        #endregion

        public Rectangle GetRegion(string key)
        {
            if (!_regions.TryGetValue(key, out var rect))
                throw new KeyNotFoundException(key);

            return rect;
        }

        #region MaxRects

        private bool FindPosition(int w, int h, out Rectangle best)
        {
            best = Rectangle.Empty;
            int bestScore = int.MaxValue;

            foreach (var free in _freeRects)
            {
                if (w <= free.Width && h <= free.Height)
                {
                    int score = free.Width * free.Height - w * h;
                    if (score < bestScore)
                    {
                        best = new Rectangle(free.X, free.Y, w, h);
                        bestScore = score;
                    }
                }
            }

            return best != Rectangle.Empty;
        }

        private void SplitFreeRects(Rectangle used)
        {
            for (int i = _freeRects.Count - 1; i >= 0; i--)
            {
                var free = _freeRects[i];
                if (!free.Intersects(used))
                    continue;

                _freeRects.RemoveAt(i);

                if (used.X > free.X)
                    _freeRects.Add(new Rectangle(
                        free.X, free.Y,
                        used.X - free.X, free.Height));

                if (used.Right < free.Right)
                    _freeRects.Add(new Rectangle(
                        used.Right, free.Y,
                        free.Right - used.Right, free.Height));

                if (used.Y > free.Y)
                    _freeRects.Add(new Rectangle(
                        free.X, free.Y,
                        free.Width, used.Y - free.Y));

                if (used.Bottom < free.Bottom)
                    _freeRects.Add(new Rectangle(
                        free.X, used.Bottom,
                        free.Width, free.Bottom - used.Bottom));
            }
        }

        private void PruneFreeRects()
        {
            for (int i = 0; i < _freeRects.Count; i++)
            {
                for (int j = i + 1; j < _freeRects.Count; j++)
                {
                    if (Contains(_freeRects[i], _freeRects[j]))
                    {
                        _freeRects.RemoveAt(j--);
                    }
                    else if (Contains(_freeRects[j], _freeRects[i]))
                    {
                        _freeRects.RemoveAt(i--);
                        break;
                    }
                }
            }
        }

        private static bool Contains(Rectangle a, Rectangle b)
        {
            return a.X <= b.X && a.Y <= b.Y &&
                   a.Right >= b.Right && a.Bottom >= b.Bottom;
        }

        #endregion

        #region GPU

        private void CopyTexture(Texture2D source, Rectangle destination)
        {
            var data = new Color[source.Width * source.Height];
            source.GetData(data);
            _texture.SetData(0, destination, data, 0, data.Length);
        }

        private void ClearRegion(Rectangle region)
        {
            var data = new Color[region.Width * region.Height];
            Array.Fill(data, Color.Transparent);
            _texture.SetData(0, region, data, 0, data.Length);
        }

        #endregion

        public void Dispose()
        {
            _texture.Dispose();
            _regions.Clear();
            _freeRects.Clear();
        }
    }
}