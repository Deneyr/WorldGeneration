using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SFML.Graphics;

namespace PokeU.View
{
    public interface IObject2D: IDisposable
    {
        (Texture2D, Rectangle) Texture
        {
            get;
        }

        Vector2 Position
        {
            get; set;
        }

        float Rotation
        {
            get; set;
        }

        Vector2 Scale
        {
            get; set;
        }

        Vector2 Origin
        {
            get; set;
        }

        Microsoft.Xna.Framework.Color Color
        {
            get;
        }

        Microsoft.Xna.Framework.Color EffectColor
        {
            get; set;
        }

        SpriteEffects Effects
        {
            get; set;
        }

        float RatioAltitude
        {
            get; set;
        }

        Rectangle TextureRect
        {
            get; set;
        }

        FloatRect ViewBound
        {
            get;
        }

        // Size helpers
        int Width
        {
            get;
        }
        int Height
        {
            get;
        }

        void DrawIn(SpriteBatch spriteBatch, ref FloatRect boundsView);

        // Part animations
        void SetCanevas(Rectangle newCanevas);

        void SetZoom(float newZoom);
    }
}
