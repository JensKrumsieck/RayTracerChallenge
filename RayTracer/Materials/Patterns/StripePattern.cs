using System;
using System.Numerics;

namespace RayTracer.Materials.Patterns
{
    public sealed class StripePattern : Pattern
    {
        public Color A;
        public Color B;

        public StripePattern(Color a, Color b, Transform? t = null)
        {
            A = a;
            B = b;
            Transform = t ?? Transform.Identity;
        }

        /// <summary>
        /// Local Color at Point in 3d Space
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Color At(Vector4 point) => MathF.Floor(point.X) % 2 == 0 ? A : B;
    }
}
