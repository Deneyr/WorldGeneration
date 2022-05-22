using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokeU.View.Animations
{
    public class ZoomAnimationManager
    {
        private Dictionary<IObject2D, ZoomAnimation> animationsToPlay;

        public ZoomAnimationManager()
        {
            this.animationsToPlay = new Dictionary<IObject2D, ZoomAnimation>();
        }

        public void Run(Time deltaTime)
        {
            List<IObject2D> finishedAnimation = new List<IObject2D>();

            foreach (KeyValuePair<IObject2D, ZoomAnimation> keyValuePair in this.animationsToPlay)
            {
                if (keyValuePair.Value.State == AnimationState.ENDING)
                {
                    finishedAnimation.Add(keyValuePair.Key);
                }
                else
                {
                    keyValuePair.Value.DeltaTime = deltaTime;

                    keyValuePair.Value.Run();
                }
            }

            foreach (IObject2D object2D in finishedAnimation)
            {
                this.animationsToPlay.Remove(object2D);
            }
        }

        public IAnimation GetAnimationFromAObject2D(IObject2D object2D)
        {
          
            IAnimation animation = null;

            if (this.animationsToPlay.ContainsKey(object2D))
            {
                animation = this.animationsToPlay[object2D];
            }

            return animation;
        }

        public void PlayAnimation(IObject2D object2D, ZoomAnimation animation)
        {
            animation.Reset();

            if (this.animationsToPlay.ContainsKey(object2D))
            {
                this.animationsToPlay[object2D] = animation;
            }
            else
            {
                this.animationsToPlay.Add(object2D, animation);
            }
        }

    }
}

