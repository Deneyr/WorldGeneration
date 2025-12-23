using PokeU.View;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokeU.Animation
{
    public class AnimationManager
    {
        private Dictionary<IObject2D, IAnimation> animationsToPlay;

        public AnimationManager()
        {
            this.animationsToPlay = new Dictionary<IObject2D, IAnimation>();
        }

        public void Run(Time deltaTime)
        {
            List<IObject2D> finishedAnimation = new List<IObject2D>();

            foreach (KeyValuePair<IObject2D, IAnimation> keyValuePair in this.animationsToPlay)
            {
                if (keyValuePair.Value.State == AnimationState.ENDING)
                {
                    finishedAnimation.Add(keyValuePair.Key);
                }
                else
                {
                    keyValuePair.Value.Run(deltaTime);

                    keyValuePair.Value.Visit(keyValuePair.Key);
                }
            }

            foreach (IObject2D obj in finishedAnimation)
            {
                this.animationsToPlay.Remove(obj);
            }
        }

        public IAnimation GetAnimationFromAObject2D(IObject2D obj)
        {
            IAnimation animation = null;

            if (this.animationsToPlay.ContainsKey(obj))
            {
                animation = this.animationsToPlay[obj];
            }

            return animation;
        }

        public void PlayAnimation(IObject2D obj, IAnimation animation)
        {
            animation.Reset();
            this.animationsToPlay[obj] = animation;
        }

        public void StopAnimation(IObject2D obj)
        {
            this.animationsToPlay.Remove(obj);
        }
    }
}
