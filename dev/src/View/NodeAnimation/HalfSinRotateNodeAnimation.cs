using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Math = Mogre.Math;

namespace Wof.View.NodeAnimation
{
    /// <summary>
    /// Animacja sinusoidalna obrotów , funkcja sin x/2
    /// <author>Kamil S³awiñski</author>
    /// </summary>
    class HalfSinRotateNodeAnimation : RotateNodeAnimation
    {
        public HalfSinRotateNodeAnimation(SceneNode node, float animationDuration, Degree maxAngle, Radian cycleLength,
                                      Vector3 axis, string name , bool increasing)
            : base(node, animationDuration, maxAngle, cycleLength, axis, name)
        {
        }

        protected override float animationFunction(float x)
        {
            return Math.Sin(x / 2);
        }
    }
}
