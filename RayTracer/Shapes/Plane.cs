using System;
using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Shapes
{
    public sealed class Plane : Entity
    {
        public Plane(Transform transform) : base(transform) { }
        public Plane() { }

        public override List<Intersection> IntersectLocal(ref Ray r)
        {
            if (MathF.Abs(r.Direction.Y) < Constants.Epsilon) return new List<Intersection>();
            var t = -r.Origin.Y / r.Direction.Y;
            return new() { new Intersection(t, this) };
        }

        public override Vector4 LocalNormal(Vector4 at) => Direction(0f, 1f, 0f);
    }
}
