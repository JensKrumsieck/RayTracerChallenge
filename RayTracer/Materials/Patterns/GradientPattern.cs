using System;
using System.Numerics;

namespace RayTracer.Materials.Patterns
{
    public sealed class GradientPattern : Pattern
    {
        public Color A;
        public Color B;

        public GradientPattern(Color a, Color b, Transform? t = null)
        {
            A = a;
            B = b;
            Transform = t ?? Transform.Identity;
        }

        public override Color At(Vector4 point)
        {
            var f = point.X - MathF.Floor(point.X);
            return A + (B - A) * f;
        }
    }
}
