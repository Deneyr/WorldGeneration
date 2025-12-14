using SFML.Graphics;
using SFML.System;
using System.Threading;

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
            mutex = new Mutex();

            this.animation = animation;

            this.animationPeriod = animationPeriod.AsMilliseconds() / AnimationManager.ANIMATION_MANAGER_PERIOD;
            currentIteration = 0;
            currentIndex = 0;

            currentState = AnimationState.STARTING;

            this.type = type;
        }

        public AnimationState State
        {
            get
            {
                return currentState;
            }
        }

        public void Run()
        {
            mutex.WaitOne();

            switch (currentState)
            {
                case AnimationState.STARTING:
                    currentState = AnimationState.RUNNING;

                    iterate();
                    break;
                case AnimationState.RUNNING:
                    iterate();

                    if(currentIndex >= animation.Length - 1)
                    {
                        currentState = AnimationState.FINALIZING;
                    }
                    break;
                case AnimationState.FINALIZING:

                    if(type == AnimationType.LOOP)
                    {
                        currentState = AnimationState.RUNNING;

                        iterate();
                    }
                    else
                    {
                        currentState = AnimationState.ENDING;
                    }
                    break;
                case AnimationState.ENDING:
                    break;
            }

            mutex.ReleaseMutex();
        }

        private void iterate()
        {
            if(currentIteration < animationPeriod)
            {
                currentIteration++;
            }
            else
            {
                currentIndex++;

                if(currentIndex >= animation.Length)
                {
                    currentIndex = 0;
                }

                currentIteration = 0;
            }
        }

        public void Reset()
        {
            mutex.WaitOne();

            currentIteration = 0;
            currentIndex = 0;
            currentState = AnimationState.STARTING;

            mutex.ReleaseMutex();
        }

        public void Stop(bool reset)
        {
            mutex.WaitOne();

            if (reset)
            {
                currentIndex = 0;
            }

            currentState = AnimationState.ENDING;

            mutex.ReleaseMutex();
        }

        public void Visit(IObject2D parentObject2D)
        {
            mutex.WaitOne();

            parentObject2D.SetCanevas(animation[currentIndex]);

            mutex.ReleaseMutex();
        }
    }
}
