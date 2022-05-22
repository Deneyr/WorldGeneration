using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View.Animations
{
    public interface IAnimation
    {
        AnimationState State
        {
            get;
        }

        void Run();

        void Reset();

        void Stop(bool reset);

        void Visit(IObject2D parentObject2D);
    }

    public enum AnimationState
    {
        STARTING,
        RUNNING,
        FINALIZING,
        ENDING
    }

    public enum AnimationType
    {
        ONETIME,
        LOOP
    }
}
