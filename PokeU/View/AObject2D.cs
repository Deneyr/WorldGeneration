using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokeU.View.Animations;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;
using Texture = SFML.Graphics.Texture;

namespace PokeU.View
{
    public abstract class AObject2D : IObject2D
    {
        protected static Texture DEFAULT_TEXTURE = new Texture(16, 16);

        protected static AnimationManager animationManager;

        protected static ZoomAnimationManager zoomAnimationManager;

        protected static RectangleShape filter;

        protected Sprite sprite;

        protected List<IAnimation> animationsList;

        private float ratioAltitude;

        public Texture2D Texture
        {
            get;
            protected set;
        }

        public Sprite ObjectSprite
        {
            get
            {
                return this.sprite;
            }

            protected set
            {
                this.sprite = value;
            }
        }

        public Vector2f Position
        {
            get
            {
                return this.ObjectSprite.Position;
            }

            protected set
            {
                this.ObjectSprite.Position = value * MainGame.MODEL_TO_VIEW;
            }
        }

        public float RatioAltitude
        {
            get
            {
                return this.ratioAltitude;
            }

            set
            {
                this.ratioAltitude = value;
            }
        }

        static AObject2D()
        {
            AObject2D.animationManager = new AnimationManager();

            AObject2D.zoomAnimationManager = new ZoomAnimationManager();

            AObject2D.filter = new RectangleShape(new Vector2f(MainGame.MODEL_TO_VIEW, MainGame.MODEL_TO_VIEW));
        }

        public AObject2D()
        {
            this.sprite = new Sprite();

            this.animationsList = new List<IAnimation>();

            this.ratioAltitude = 0;
        }       

        public virtual void Dispose()
        {
            
        }

        public virtual void DrawIn(SpriteBatch spriteBatch, ref FloatRect boundsView)
        {

            float ratioAltitude = 1 - Math.Abs(this.ratioAltitude);
            byte colorAltitude = (byte)(ratioAltitude * ratioAltitude * 255f);

            //if (this.RatioAltitude < 0)
            //{
            //    byte colorAltitude = (byte)(-this.ratioAltitude * 255);

            //    this.ObjectSprite.Color = new Color(colorAltitude, colorAltitude, colorAltitude, this.ObjectSprite.Color.A);
            //}
            //else if(this.RatioAltitude > 0)
            //{
            //    byte colorAltitude = (byte)(this.ratioAltitude * 255);

            //    this.ObjectSprite.Color = new Color(colorAltitude, colorAltitude, colorAltitude, this.ObjectSprite.Color.A);
            //}
            //else
            //{
            //    this.ObjectSprite.Color = new Color(255, 255, 255, this.ObjectSprite.Color.A);
            //}

            Rectangle sourceRectangle = new Rectangle(this.ObjectSprite.TextureRect.Left, this.ObjectSprite.TextureRect.Top, this.ObjectSprite.TextureRect.Width, this.ObjectSprite.TextureRect.Height);
            Rectangle destinationRectangle = new Rectangle((int)this.ObjectSprite.Position.X, (int)this.ObjectSprite.Position.Y, (int)(this.ObjectSprite.TextureRect.Width * this.ObjectSprite.Scale.X), (int)(this.ObjectSprite.TextureRect.Height * this.ObjectSprite.Scale.Y));

            Color spriteColor = new Color(colorAltitude, colorAltitude, colorAltitude, this.ObjectSprite.Color.A);

            spriteBatch.Draw(texture: this.Texture, destinationRectangle: destinationRectangle, sourceRectangle: sourceRectangle, color: spriteColor);

            //if (this.RatioAltitude != 0)
            //{
            //    byte colorAltitude = (byte)(128 + this.ratioAltitude * 127);
            //    byte alpha = (byte)(Math.Abs(this.ratioAltitude) * 200);

            //    AObject2D.filter.Position = this.ObjectSprite.Position;
            //    AObject2D.filter.FillColor = new Color(colorAltitude, colorAltitude, colorAltitude, alpha);

            //    window.Draw(AObject2D.filter);
            //}
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

            if (animation is ZoomAnimation)
            {
                AObject2D.zoomAnimationManager.PlayAnimation(this, animation as ZoomAnimation);
            }
            else
            {
                AObject2D.animationManager.PlayAnimation(this, animation);
            }
        }

        public static void StopAnimationManager()
        {
            AObject2D.animationManager.Play = false;
        }

        public static void UpdateZoomAnimationManager(Time deltaTime)
        {
            AObject2D.zoomAnimationManager.Run(deltaTime);
        }

        public virtual void SetCanevas(IntRect newCanevas)
        {
            this.sprite.TextureRect = newCanevas;
        }

        public void SetZoom(float newZoom)
        {
            this.sprite.Scale = new Vector2f(newZoom, newZoom);
        }
    }
}
