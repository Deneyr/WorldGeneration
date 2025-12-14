using Microsoft.Xna.Framework.Graphics;
using SFML.Graphics;
using SFML.System;
using System;

namespace PokeU.View
{
    public interface IObject2D: IDisposable
    {
        Texture2D Texture
        {
            get;
        }

        Sprite ObjectSprite
        {
            get;
        }

        Vector2f Position
        {
            get;
        }

        float RatioAltitude
        {
            get;
            set;
        }

        void DrawIn(SpriteBatch spriteBatch, ref FloatRect boundsView);

        // Part animations
        void SetCanevas(IntRect newCanevas);

        void SetZoom(float newZoom);
    }
}
