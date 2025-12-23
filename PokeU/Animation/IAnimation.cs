using PokeU.View;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Animation
{
    public interface IAnimation
    {
        AnimationState State
        {
            get;
        }

        void Run(Time deltaTime);

        void Reset();

        void Stop();

        void Visit(IObject2D parentObject);
    }

    public enum AnimationState
    {
        STARTING,
        RUNNING,
        ENDING
    }

    public enum AnimationType
    {
        ONETIME,
        LOOP
    }

    public enum InterpolationMethod
    {
        STEP,
        LINEAR,
        SQUARE_ACC,
        SQUARE_DEC,
        SIGMOID
    }
}
