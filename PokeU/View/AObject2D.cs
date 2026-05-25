using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokeU.Animation;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;

namespace PokeU.View
{
    public abstract class AObject2D : IObject2D
    {
        protected static AnimationManager animationManager;

        protected List<IAnimation> animationsList;

        private float ratioAltitude;

        // Transform
        private Vector2 position;
        private float rotation;
        private Vector2 scale;
        private Vector2 origin;

        protected Rectangle textureRect;
        private FloatRect viewBound;

        // Appearance
        private Color color;
        private Color effectColor;
        private SpriteEffects effects;

        // Texture
        public (Texture2D, Rectangle) Texture
        {
            get;
            protected set;
        }

        public Vector2 Position
        {
            get => this.position;
            set
            {
                this.position = value;

                this.UpdateViewBound();
            }
        }

        public float Rotation
        {
            get => this.rotation;
            set => this.rotation = value;
        }

        public Vector2 Scale
        {
            get => this.scale;
            set => this.scale = value;
        }

        public Vector2 Origin
        {
            get => this.origin;
            set => this.origin = value;
        }

        public Color Color
        {
            get => this.color;
        }

        public Color EffectColor
        {
            get => this.effectColor;
            set
            {
                this.effectColor = value;

                this.UpdateRealColor();
            }
        }

        public SpriteEffects Effects
        {
            get => this.effects;
            set => this.effects = value;
        }

        public float RatioAltitude
        {
            get => this.ratioAltitude;
            set
            {
                this.ratioAltitude = value;

                this.UpdateRealColor();
            }
        }

        public virtual Rectangle TextureRect
        {
            get => this.textureRect;
            set
            {
                this.textureRect = value;

                this.UpdateViewBound();
            }
        }

        public FloatRect ViewBound
        {
            get
            {
                return this.viewBound;
            }
        }

        // Size helpers
        public virtual int Width => this.textureRect.Width;
        public virtual int Height => this.textureRect.Height;

        static AObject2D()
        {
            AObject2D.animationManager = new AnimationManager();
        }

        public AObject2D()
        {
            this.animationsList = new List<IAnimation>();

            this.Texture = (null, Rectangle.Empty);

            this.position = Vector2.Zero;
            this.textureRect = new Rectangle(0, 0, MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW);
            this.rotation = 0f;
            this.scale = Vector2.One;
            this.origin = Vector2.Zero;

            this.effectColor = Color.White;
            this.effects = SpriteEffects.None;

            this.ratioAltitude = 0;

            this.UpdateViewBound();
            this.UpdateRealColor();
        }       

        public virtual void Dispose()
        {
            
        }

        public virtual void DrawIn(SpriteBatch spriteBatch, ref FloatRect boundsView)
        {
            spriteBatch.Draw(
                texture: this.Texture.Item1,
                position: this.position,
                sourceRectangle: this.TextureRect,
                color: this.color,
                rotation: this.rotation,
                origin: this.origin,
                scale: this.scale,
                effects: this.effects,
                0);
        }

        public virtual void RenderIn(Vector2 renderPosition, SpriteBatch spriteBatch, ref FloatRect boundsView)
        {
            spriteBatch.Draw(
                texture: this.Texture.Item1,
                position: renderPosition,
                sourceRectangle: this.TextureRect,
                color: Color.White,
                rotation: this.rotation,
                origin: this.origin,
                scale: this.scale,
                effects: this.effects,
                0);
        }

        // Part animations.
        public static IntRect[] CreateAnimation(int leftStart, int topStart, int width, int height, int nbFrame)
        {
            IntRect[] result = new IntRect[nbFrame];

            for (int i = 0; i < nbFrame; i++)
            {
                result[i] = new IntRect(leftStart + i * width, topStart, width, height);
            }

            return result;
        }

        public void PlayAnimation(int index)
        {
            IAnimation animation = this.animationsList[index];
            AObject2D.animationManager.PlayAnimation(this, animation);
        }

        public static void UpdateAnimationManager(Time deltaTime)
        {
            AObject2D.animationManager.Run(deltaTime);
        }

        public virtual void SetCanevas(Rectangle newCanevas)
        {
            this.TextureRect = newCanevas;
        }

        public void SetZoom(float newScale)
        {
            this.Scale = new Vector2(newScale, newScale);
        }

        private void UpdateRealColor()
        {
            float ratioAltitude = 1 - Math.Abs(this.ratioAltitude);
            byte colorAltitude = (byte)(ratioAltitude * ratioAltitude * 255f);

            this.color = new Color(colorAltitude, colorAltitude, colorAltitude, this.effectColor.A);
        }

        private void UpdateViewBound()
        {
            this.viewBound = new FloatRect(this.position.X, this.Position.Y, this.Width, this.Height);
        }
    }
}
