using System;
using System.Numerics;

namespace RayTracer.Materials.Patterns
{
    public sealed class RingPattern : Pattern
    {
        public Color A;
        public Color B;

        public RingPattern(Color a, Color b, Transform? t = null)
        {
            A = a;
            B = b;
            Transform = t ?? Transform.Identity;
        }

        public override Color At(Vector4 point) =>
            MathF.Abs(MathF.Floor(MathF.Sqrt(point.X * point.X + point.Z * point.Z)) % 2f) <= Constants.EpsilonLow ? A : B;
    }
}
