using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokeU.View.Animations
{
    public class AnimationManager
    {
        public static int ANIMATION_MANAGER_PERIOD = 100;

        private Mutex mutex;

        private Thread mainThread;

        private volatile bool play;

        private Dictionary<IObject2D, IAnimation> animationsToPlay;

        public bool Play
        {
            get
            {
                return this.play;
            }
            set
            {
                this.play = value;
            }
        }

        public AnimationManager()
        {
            this.mutex = new Mutex();

            this.mainThread = new Thread(new ThreadStart(this.Run));

            this.Play = true;

            this.animationsToPlay = new Dictionary<IObject2D, IAnimation>();

            this.mainThread.Start();
        }

        private void Run()
        {
            while (this.Play)
            {
                List<IObject2D> finishedAnimation = new List<IObject2D>();

                this.mutex.WaitOne();

                foreach (KeyValuePair<IObject2D, IAnimation> keyValuePair in this.animationsToPlay)
                {
                    if(keyValuePair.Value.State == AnimationState.ENDING)
                    {
                        finishedAnimation.Add(keyValuePair.Key);
                    }
                    else
                    {
                        keyValuePair.Value.Run();
                    }
                }

                foreach (IObject2D object2D in finishedAnimation)
                {
                    this.animationsToPlay.Remove(object2D);
                }

                this.mutex.ReleaseMutex();

                Thread.Sleep(100);
            }
        }

        public IAnimation GetAnimationFromAObject2D(IObject2D object2D)
        {
            this.mutex.WaitOne();

            IAnimation animation = null;

            if (this.animationsToPlay.ContainsKey(object2D))
            {
                animation = this.animationsToPlay[object2D];
            }

            this.mutex.ReleaseMutex();

            return animation;
        }

        public void PlayAnimation(IObject2D object2D, IAnimation animation)
        {
            this.mutex.WaitOne();

            animation.Reset();

            if (this.animationsToPlay.ContainsKey(object2D))
            {
                this.animationsToPlay[object2D] = animation;
            }
            else
            {
                this.animationsToPlay.Add(object2D, animation);
            }

            this.mutex.ReleaseMutex();
        }

    }
}
