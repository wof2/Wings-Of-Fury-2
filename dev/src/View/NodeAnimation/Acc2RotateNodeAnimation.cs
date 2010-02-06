using System.Collections.Generic;
using System.Text;
using Mogre;
using Math=System.Math;
namespace Wof.View.NodeAnimation
{
    /// <summary>
    /// Animacja obrotu ktora przyspiesza jak pierwiastek(X) -> na poczatku szybko potem wolno
    /// </summary>
    class Acc2RotateNodeAnimation : RotateNodeAnimation
    {

        public Acc2RotateNodeAnimation(SceneNode node, float animationDuration, Degree maxAngle, Radian cycleLength,
                                      Vector3 axis, string name)
            : base(node, animationDuration, maxAngle, cycleLength, axis, name)
        {
        }

        protected override float animationFunction(float x)
        {
            if (x <= cycleLength.ValueRadians)
            {
                return (float) Math.Pow(x, 0.8f);
            }
            else
            {
                return (float)Math.Pow(cycleLength.ValueRadians, 0.8f); 
               
            }
        }
    }
}
