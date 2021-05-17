using System;
using System.Numerics;

namespace RayTracer.Materials.Patterns
{
    public sealed class CheckerPattern : Pattern
    {
        public Color A;
        public Color B;

        public CheckerPattern(Color a, Color b, Transform? t = null)
        {
            A = a;
            B = b;
            Transform = t ?? Transform.Identity;
        }

        public override Color At(Vector4 point) =>
            (MathF.Floor(point.X) + MathF.Floor(point.Y) + MathF.Floor(point.Z)) % 2 == 0 ? A : B;
    }
}
