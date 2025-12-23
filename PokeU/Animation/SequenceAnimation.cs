using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeU.View;
using SFML.System;

namespace PokeU.Animation
{
    public class SequenceAnimation : IAnimation
    {
        private SortedDictionary<float, IAnimation> animationsSequence;
        private List<IAnimation> animationsToPlay;
        private SortedDictionary<float, IAnimation>.Enumerator animationEnumerator;
        bool isEnumeratorValid;

        private AnimationType type;

        private Time animationPeriod;
        private Time timeElapsed;

        public SequenceAnimation(Time animationPeriod, AnimationType type)
        {
            this.State = AnimationState.STARTING;

            this.animationsSequence = new SortedDictionary<float, IAnimation>();
            this.animationsToPlay = new List<IAnimation>();

            this.timeElapsed = Time.Zero;

            this.animationPeriod = animationPeriod;
            this.type = type;
        }

        public void AddAnimation(float timeInSec, IAnimation animationToAdd)
        {
            this.animationsSequence.Add(timeInSec, animationToAdd);
        }

        public AnimationState State
        {
            get;
            private set;
        }

        public void Reset()
        {
            this.animationsToPlay.Clear();

            foreach(IAnimation animation in this.animationsSequence.Values)
            {
                animation.Reset();
            }

            this.animationEnumerator = this.animationsSequence.GetEnumerator();
            this.isEnumeratorValid = this.animationEnumerator.MoveNext();

            this.timeElapsed = Time.Zero;

            this.State = AnimationState.STARTING;
        }

        public void Run(Time deltaTime)
        {

            switch (this.State)
            {
                case AnimationState.STARTING:
                    this.State = AnimationState.RUNNING;

                    this.Iterate(deltaTime);
                    break;
                case AnimationState.RUNNING:
                    this.Iterate(deltaTime);
                    break;
                case AnimationState.ENDING:
                    break;
            }
        }

        private void Iterate(Time deltaTime)
        {
            Time oldTime = this.timeElapsed;

            this.timeElapsed += deltaTime;
            float timeInSec = this.timeElapsed.AsSeconds();

            while(this.isEnumeratorValid
                && this.animationEnumerator.Current.Key <= timeInSec)
            {
                this.animationEnumerator.Current.Value.Reset();

                this.animationsToPlay.Add(this.animationEnumerator.Current.Value);

                this.isEnumeratorValid = this.animationEnumerator.MoveNext();
            }

            List<IAnimation> animationsToRemove = new List<IAnimation>();
            foreach (IAnimation animation in this.animationsToPlay)
            {
                animation.Run(deltaTime);

                if(animation.State == AnimationState.ENDING)
                {
                    animationsToRemove.Add(animation);
                }
            }

            foreach (IAnimation animation in animationsToRemove)
            {
                this.animationsToPlay.Remove(animation);
            }

            if((this.isEnumeratorValid || this.animationsToPlay.Any()) == false)
            {
                this.State = AnimationState.ENDING;
            }
        }

        public void Stop()
        {
            foreach (IAnimation animation in this.animationsToPlay)
            {
                animation.Stop();
            }

            this.State = AnimationState.ENDING;
        }

        public void Visit(IObject2D parentObject)
        {
            foreach (IAnimation animation in this.animationsToPlay)
            {
                animation.Visit(parentObject);
            }
        }
    }
}
