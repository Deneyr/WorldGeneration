using Microsoft.Xna.Framework;
using PokeU.View;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokeU.Animation
{
    public class FrameAnimation: AAnimation
    {
        private Rectangle[] frames;

        public FrameAnimation(Rectangle[] frames, Time animationPeriod, AnimationType type, InterpolationMethod method):
            base(0, frames.Count(), animationPeriod, type, method)
        {
            this.frames = frames;
        }

        public override void Visit(IObject2D parentObject)
        {
            int index = (int)this.currentValue;

            if (index >= frames.Count())
            {
                index = frames.Count() - 1;
            }

            parentObject.SetCanevas(this.frames[index]);
        }
    }
}
