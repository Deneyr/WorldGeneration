using SFML.System;
using System.Collections.Generic;

namespace PokeU.View.Animations
{
    public class ZoomAnimationManager
    {
        private Dictionary<IObject2D, ZoomAnimation> animationsToPlay;

        public ZoomAnimationManager()
        {
            animationsToPlay = new Dictionary<IObject2D, ZoomAnimation>();
        }

        public void Run(Time deltaTime)
        {
            List<IObject2D> finishedAnimation = new List<IObject2D>();

            foreach (KeyValuePair<IObject2D, ZoomAnimation> keyValuePair in animationsToPlay)
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
                animationsToPlay.Remove(object2D);
            }
        }

        public IAnimation GetAnimationFromAObject2D(IObject2D object2D)
        {
          
            IAnimation animation = null;

            if (animationsToPlay.ContainsKey(object2D))
            {
                animation = animationsToPlay[object2D];
            }

            return animation;
        }

        public void PlayAnimation(IObject2D object2D, ZoomAnimation animation)
        {
            animation.Reset();

            if (animationsToPlay.ContainsKey(object2D))
            {
                animationsToPlay[object2D] = animation;
            }
            else
            {
                animationsToPlay.Add(object2D, animation);
            }
        }

    }
}

