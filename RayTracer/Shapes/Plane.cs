using System;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Shapes
{
    public sealed class Plane : Entity
    {
        public Plane(Transform transform) : base(transform) { }
        public Plane() { }

        public override Intersection[] IntersectLocal(in Ray r)
        {
            if (MathF.Abs(r.Direction.Y) < Constants.Epsilon) return Array.Empty<Intersection>();
            var t = -r.Origin.Y / r.Direction.Y;
            return new[] { new Intersection(t, this) };
        }

        public override Vector4 LocalNormal(in Vector4 at) => Direction(0f, 1f, 0f);
    }
}
