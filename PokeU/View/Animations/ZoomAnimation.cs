using SFML.System;

namespace PokeU.View.Animations
{
    public class ZoomAnimation : IAnimation
    {
        private volatile AnimationState currentState;

        private float zoomFrom;
        private float zoomTo;
        private float currentZoom;

        private Time animationPeriod;
        private Time timeElapsed;

        private Time deltaTime;

        private AnimationType type;


        public ZoomAnimation(float zoomFrom, float zoomTo, Time animationPeriod, AnimationType type)
        {
            this.animationPeriod = animationPeriod;
            timeElapsed = Time.Zero;

            this.zoomFrom = zoomFrom;
            this.zoomTo = zoomTo;
            currentZoom = zoomFrom;

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

        public Time DeltaTime
        {
            get
            {
                return deltaTime;
            }

            set
            {
                deltaTime = value;
            }
        }

        public void Run()
        {

            switch (currentState)
            {
                case AnimationState.STARTING:
                    currentState = AnimationState.RUNNING;

                    iterate();
                    break;
                case AnimationState.RUNNING:
                    iterate();

                    if (timeElapsed >= animationPeriod)
                    {
                        currentState = AnimationState.FINALIZING;
                    }
                    break;
                case AnimationState.FINALIZING:

                    if (type == AnimationType.LOOP)
                    {
                        timeElapsed = Time.Zero;

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
        }

        private void iterate()
        {
            this.timeElapsed += deltaTime;

            Time timeElapsed = this.timeElapsed;
            if(this.timeElapsed > animationPeriod / 2)
            {
                timeElapsed = this.timeElapsed - animationPeriod / 2;
            }

            float scale = (float) timeElapsed.AsMicroseconds() / (animationPeriod / 2).AsMicroseconds();

            if (this.timeElapsed < animationPeriod / 2)
            {
                currentZoom = zoomTo * scale + zoomFrom * (1 - scale);
            }
            else
            {
                currentZoom = zoomFrom * scale + zoomTo * (1 - scale);
            }
        }

        public void Reset()
        {
            timeElapsed = Time.Zero;

            currentZoom = zoomFrom;

            currentState = AnimationState.STARTING;
        }

        public void Stop(bool reset)
        {
            if (reset)
            {
                currentZoom = 1;
            }

            currentState = AnimationState.ENDING;
        }

        public void Visit(IObject2D parentObject2D)
        {
            parentObject2D.SetZoom(currentZoom);
        }
    }
}
