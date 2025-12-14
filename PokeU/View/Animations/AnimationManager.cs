using System.Collections.Generic;
using System.Threading;

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
                return play;
            }
            set
            {
                play = value;
            }
        }

        public AnimationManager()
        {
            mutex = new Mutex();

            mainThread = new Thread(new ThreadStart(Run));

            Play = true;

            animationsToPlay = new Dictionary<IObject2D, IAnimation>();

            mainThread.Start();
        }

        private void Run()
        {
            while (Play)
            {
                List<IObject2D> finishedAnimation = new List<IObject2D>();

                mutex.WaitOne();

                foreach (KeyValuePair<IObject2D, IAnimation> keyValuePair in animationsToPlay)
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
                    animationsToPlay.Remove(object2D);
                }

                mutex.ReleaseMutex();

                Thread.Sleep(100);
            }
        }

        public IAnimation GetAnimationFromAObject2D(IObject2D object2D)
        {
            mutex.WaitOne();

            IAnimation animation = null;

            if (animationsToPlay.ContainsKey(object2D))
            {
                animation = animationsToPlay[object2D];
            }

            mutex.ReleaseMutex();

            return animation;
        }

        public void PlayAnimation(IObject2D object2D, IAnimation animation)
        {
            mutex.WaitOne();

            animation.Reset();

            if (animationsToPlay.ContainsKey(object2D))
            {
                animationsToPlay[object2D] = animation;
            }
            else
            {
                animationsToPlay.Add(object2D, animation);
            }

            mutex.ReleaseMutex();
        }

    }
}
