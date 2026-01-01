using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PokeU.View.ResourcesManager
{
    public class AtlasManager : IDisposable
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly int _atlasWidth;
        private readonly int _atlasHeight;

        private readonly List<DynamicMaxRectsAtlas> _atlases;
        private readonly Dictionary<string, AtlasRegionHandle> _lookup;

        public IReadOnlyList<DynamicMaxRectsAtlas> Atlases => _atlases;

        public AtlasManager(GraphicsDevice graphicsDevice, int atlasWidth, int atlasHeight)
        {
            _graphicsDevice = graphicsDevice;
            _atlasWidth = atlasWidth;
            _atlasHeight = atlasHeight;

            _atlases = new List<DynamicMaxRectsAtlas>();
            _lookup = new Dictionary<string, AtlasRegionHandle>();
        }

        #region Add / Remove

        public void AddTexture(string key, Texture2D texture)
        {
            if (_lookup.ContainsKey(key))
                throw new InvalidOperationException($"Key '{key}' already exists.");

            for (int i = 0; i < _atlases.Count; i++)
            {
                if (_atlases[i].TryAdd(key, texture, out Rectangle region))
                {
                    _lookup[key] = new AtlasRegionHandle
                    {
                        AtlasIndex = i,
                        Region = region
                    };
                    return;
                }
            }

            var atlas = new DynamicMaxRectsAtlas(_graphicsDevice, _atlasWidth, _atlasHeight);
            _atlases.Add(atlas);

            if (!atlas.TryAdd(key, texture, out Rectangle newRegion))
                throw new InvalidOperationException("Texture too large for atlas.");

            _lookup[key] = new AtlasRegionHandle
            {
                AtlasIndex = _atlases.Count - 1,
                Region = newRegion
            };
        }

        public bool Remove(string key)
        {
            if (!_lookup.TryGetValue(key, out var handle))
                return false;

            var atlas = _atlases[handle.AtlasIndex];
            atlas.Remove(key);
            _lookup.Remove(key);

            if (atlas.IsEmpty)
            {
                atlas.Dispose();
                _atlases.RemoveAt(handle.AtlasIndex);

                // Réindexation
                foreach (var k in new List<string>(_lookup.Keys))
                {
                    var h = _lookup[k];
                    if (h.AtlasIndex > handle.AtlasIndex)
                    {
                        h.AtlasIndex--;
                        _lookup[k] = h;
                    }
                }
            }

            return true;
        }

        #endregion

        #region Access

        public (Texture2D texture, Rectangle region) GetTextureRegion(string key)
        {
            if (!_lookup.TryGetValue(key, out var handle))
                throw new KeyNotFoundException(key);

            var atlas = _atlases[handle.AtlasIndex];
            return (atlas.Texture, handle.Region);
        }

        public bool TryGetTextureRegion(
            string key,
            out Texture2D texture,
            out Rectangle region)
        {
            texture = null;
            region = Rectangle.Empty;

            if (!_lookup.TryGetValue(key, out var handle))
                return false;

            texture = _atlases[handle.AtlasIndex].Texture;
            region = handle.Region;
            return true;
        }

        #endregion

        #region Draw helper

        public void Draw(
            SpriteBatch spriteBatch,
            string key,
            Vector2 position,
            Color color)
        {
            var (texture, region) = GetTextureRegion(key);
            spriteBatch.Draw(texture, position, region, color);
        }

        #endregion

        public void Dispose()
        {
            foreach (var atlas in _atlases)
                atlas.Dispose();

            _atlases.Clear();
            _lookup.Clear();
        }

        public struct AtlasRegionHandle
        {
            public int AtlasIndex;
            public Rectangle Region;
        }
    }
}
