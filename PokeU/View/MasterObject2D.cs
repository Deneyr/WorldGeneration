using PokeU.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View
{
    public class MasterObject2D : AObject2D
    {
        public MasterObject2D(IAnimation animation)
        {
            animationManager.PlayAnimation(this, animation);
        }
    }
}
