using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeU.View.Animations;
using PokeU.View.Helpers;
using SFML.Graphics;
using SFML.System;

namespace PokeU.View
{
    public abstract class AObject2D : IObject2D
    {
        protected static AnimationManager animationManager;

        protected static ZoomAnimationManager zoomAnimationManager;

        protected static RectangleShape filter;

        protected Sprite sprite;

        protected List<IAnimation> animationsList;

        private float ratioAltitude;

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
                this.ObjectSprite.Position = value * MainWindow.MODEL_TO_VIEW;
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

            AObject2D.filter = new RectangleShape(new Vector2f(MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW));
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

        public virtual void DrawIn(RenderWindow window, ref FloatRect boundsView)
        {

            if (this.RatioAltitude < 0)
            {
                byte colorAltitude = (byte)(127 + 128 + this.ratioAltitude * 128);

                this.ObjectSprite.Color = new Color(colorAltitude, colorAltitude, colorAltitude, this.ObjectSprite.Color.A);
            }
            else if(this.RatioAltitude > 0)
            {
                byte colorAltitude = (byte)(127 + 128 - this.ratioAltitude * 128);

                this.ObjectSprite.Color = new Color(colorAltitude, colorAltitude, colorAltitude, this.ObjectSprite.Color.A);
            }
            else
            {
                this.ObjectSprite.Color = new Color(255, 255, 255, this.ObjectSprite.Color.A);
            }

            window.Draw(this.ObjectSprite);

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
