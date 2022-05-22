using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokeU.View.Animations
{
    public class Animation: IAnimation
    {
        private Mutex mutex;

        private volatile AnimationState currentState;

        private AnimationType type;

        private IntRect[] animation;

        private int animationPeriod;
        private int currentIteration;
        private int currentIndex;

        public Animation(IntRect[] animation, Time animationPeriod, AnimationType type)
        {
            this.mutex = new Mutex();

            this.animation = animation;

            this.animationPeriod = animationPeriod.AsMilliseconds() / AnimationManager.ANIMATION_MANAGER_PERIOD;
            this.currentIteration = 0;
            this.currentIndex = 0;

            this.currentState = AnimationState.STARTING;

            this.type = type;
        }

        public AnimationState State
        {
            get
            {
                return this.currentState;
            }
        }

        public void Run()
        {
            this.mutex.WaitOne();

            switch (this.currentState)
            {
                case AnimationState.STARTING:
                    this.currentState = AnimationState.RUNNING;

                    this.iterate();
                    break;
                case AnimationState.RUNNING:
                    this.iterate();

                    if(this.currentIndex >= this.animation.Length - 1)
                    {
                        this.currentState = AnimationState.FINALIZING;
                    }
                    break;
                case AnimationState.FINALIZING:

                    if(this.type == AnimationType.LOOP)
                    {
                        this.currentState = AnimationState.RUNNING;

                        this.iterate();
                    }
                    else
                    {
                        this.currentState = AnimationState.ENDING;
                    }
                    break;
                case AnimationState.ENDING:
                    break;
            }

            this.mutex.ReleaseMutex();
        }

        private void iterate()
        {
            if(this.currentIteration < this.animationPeriod)
            {
                this.currentIteration++;
            }
            else
            {
                this.currentIndex++;

                if(this.currentIndex >= this.animation.Length)
                {
                    this.currentIndex = 0;
                }

                this.currentIteration = 0;
            }
        }

        public void Reset()
        {
            this.mutex.WaitOne();

            this.currentIteration = 0;
            this.currentIndex = 0;
            this.currentState = AnimationState.STARTING;

            this.mutex.ReleaseMutex();
        }

        public void Stop(bool reset)
        {
            this.mutex.WaitOne();

            if (reset)
            {
                this.currentIndex = 0;
            }

            this.currentState = AnimationState.ENDING;

            this.mutex.ReleaseMutex();
        }

        public void Visit(IObject2D parentObject2D)
        {
            this.mutex.WaitOne();

            parentObject2D.SetCanevas(this.animation[this.currentIndex]);

            this.mutex.ReleaseMutex();
        }
    }
}
