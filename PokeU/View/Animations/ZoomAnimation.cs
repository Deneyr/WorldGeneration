using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.timeElapsed = Time.Zero;

            this.zoomFrom = zoomFrom;
            this.zoomTo = zoomTo;
            this.currentZoom = zoomFrom;

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

        public Time DeltaTime
        {
            get
            {
                return this.deltaTime;
            }

            set
            {
                this.deltaTime = value;
            }
        }

        public void Run()
        {

            switch (this.currentState)
            {
                case AnimationState.STARTING:
                    this.currentState = AnimationState.RUNNING;

                    this.iterate();
                    break;
                case AnimationState.RUNNING:
                    this.iterate();

                    if (this.timeElapsed >= this.animationPeriod)
                    {
                        this.currentState = AnimationState.FINALIZING;
                    }
                    break;
                case AnimationState.FINALIZING:

                    if (this.type == AnimationType.LOOP)
                    {
                        this.timeElapsed = Time.Zero;

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
        }

        private void iterate()
        {
            this.timeElapsed += this.deltaTime;

            Time timeElapsed = this.timeElapsed;
            if(this.timeElapsed > this.animationPeriod / 2)
            {
                timeElapsed = this.timeElapsed - this.animationPeriod / 2;
            }

            float scale = ((float) timeElapsed.AsMicroseconds()) / (this.animationPeriod / 2).AsMicroseconds();

            if (this.timeElapsed < this.animationPeriod / 2)
            {
                this.currentZoom = this.zoomTo * scale + this.zoomFrom * (1 - scale);
            }
            else
            {
                this.currentZoom = this.zoomFrom * scale + this.zoomTo * (1 - scale);
            }
        }

        public void Reset()
        {
            this.timeElapsed = Time.Zero;

            this.currentZoom = zoomFrom;

            this.currentState = AnimationState.STARTING;
        }

        public void Stop(bool reset)
        {
            if (reset)
            {
                this.currentZoom = 1;
            }

            this.currentState = AnimationState.ENDING;
        }

        public void Visit(IObject2D parentObject2D)
        {
            parentObject2D.SetZoom(this.currentZoom);
        }
    }
}
