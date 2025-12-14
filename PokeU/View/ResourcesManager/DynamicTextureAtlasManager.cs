using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class DynamicTextureAtlasManager
{
    private GraphicsDevice _device;
    private int _pageWidth, _pageHeight, _padding;

    public class AtlasPage
    {
        public Texture2D Texture;
        public MaxRectsBinPack Packer;
        public Dictionary<string, Rectangle> Regions = new Dictionary<string, Rectangle>();
        public int PageIndex;

        public AtlasPage(GraphicsDevice device, int width, int height, int padding, int index)
        {
            Texture = new Texture2D(device, width, height, false, SurfaceFormat.Color);
            Packer = new MaxRectsBinPack(width, height);
            PageIndex = index;
        }
    }

    private List<AtlasPage> _pages = new List<AtlasPage>();
    private Dictionary<string, int> _idToPage = new Dictionary<string, int>();

    public IReadOnlyList<AtlasPage> Pages => _pages;

    public DynamicTextureAtlasManager(GraphicsDevice device, int pageWidth = 2048, int pageHeight = 2048, int padding = 2)
    {
        _device = device;
        _pageWidth = pageWidth;
        _pageHeight = pageHeight;
        _padding = padding;
    }

    public (int pageIndex, Rectangle region) AddTexture(string id, Texture2D tex)
    {
        if (_idToPage.ContainsKey(id))
            throw new Exception("Texture already exists in atlas: " + id);

        for (int i = 0; i < _pages.Count; i++)
        {
            if (TryInsertTexture(_pages[i], id, tex, out var region))
            {
                _idToPage[id] = i;
                return (i, region);
            }
        }

        var newPage = new AtlasPage(_device, _pageWidth, _pageHeight, _padding, _pages.Count);
        _pages.Add(newPage);

        if (!TryInsertTexture(newPage, id, tex, out var finalRegion))
            throw new Exception("Failed to insert texture in a new page.");

        _idToPage[id] = newPage.PageIndex;
        return (newPage.PageIndex, finalRegion);
    }

    private bool TryInsertTexture(AtlasPage page, string id, Texture2D src, out Rectangle placed)
    {
        var node = page.Packer.Insert(src.Width + _padding * 2, src.Height + _padding * 2, MaxRectsBinPack.FreeRectChoiceHeuristic.BestShortSideFit);

        if (node == Rectangle.Empty)
        {
            placed = Rectangle.Empty;
            return false;
        }

        placed = new Rectangle(node.X + _padding, node.Y + _padding, src.Width, src.Height);

        Color[] pixels = new Color[src.Width * src.Height];
        src.GetData(pixels);
        page.Texture.SetData(0, placed, pixels, 0, pixels.Length);

        ApplyBleeding(page.Texture, placed, pixels, src.Width, src.Height);

        page.Regions[id] = placed;
        return true;
    }

    private void ApplyBleeding(Texture2D atlas, Rectangle rect, Color[] pixels, int w, int h)
    {
        Color[] colorTmp = new Color[1];

        for (int y = 0; y < h; y++)
        {
            colorTmp[0] = pixels[y * w];
            atlas.SetData(0, new Rectangle(rect.X - 1, rect.Y + y, 1, 1), colorTmp, 0, 1);

            colorTmp[0] = pixels[y * w + w - 1];
            atlas.SetData(0, new Rectangle(rect.X + w, rect.Y + y, 1, 1), colorTmp, 0, 1);
        }

        for (int x = 0; x < w; x++)
        {
            colorTmp[0] = pixels[x];
            atlas.SetData(0, new Rectangle(rect.X + x, rect.Y - 1, 1, 1), colorTmp, 0, 1);

            colorTmp[0] = pixels[(h - 1) * w + x];
            atlas.SetData(0, new Rectangle(rect.X + x, rect.Y + h, 1, 1), colorTmp, 0, 1);
        }
    }

    public bool TryGetRegion(string id, out Texture2D tex, out Rectangle region, out int pageIdx)
    {
        region = Rectangle.Empty;
        tex = null;
        pageIdx = -1;

        if (!_idToPage.TryGetValue(id, out pageIdx))
            return false;

        var page = _pages[pageIdx];
        tex = page.Texture;
        region = page.Regions[id];
        return true;
    }
}