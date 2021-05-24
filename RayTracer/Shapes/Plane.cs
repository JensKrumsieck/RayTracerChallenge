using System;
using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Shapes
{
    public sealed class Plane : Entity
    {
        public Plane(Transform transform) : base(transform) { }

        public Plane()
        {
            BoundingBox = new Bounds { Min = Point(float.NegativeInfinity, 0, float.NegativeInfinity), Max = Point(float.PositiveInfinity, 0, float.PositiveInfinity) };
        }

        public override List<Intersection> IntersectLocal(ref Ray r)
        {
            if (MathF.Abs(r.Direction.Y) < Constants.Epsilon) return new List<Intersection>();
            var t = -r.Origin.Y / r.Direction.Y;
            return new List<Intersection>() { new(t, this) };
        }

        public override Vector4 LocalNormal(Vector4 at) => Direction(0f, 1f, 0f);
    }
}
