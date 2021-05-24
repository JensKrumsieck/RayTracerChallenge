using RayTracer.Extension;
using System;
using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Extension.GeometricExtension;

namespace RayTracer.Shapes
{
    public class Cube : Entity
    {
        public Cube(Transform transform) : base(transform)
        {
            BoundingBox = Bounds.DefaultBox;
        }

        public Cube()
        {
            BoundingBox = Bounds.DefaultBox;
        }

        public override List<Intersection> IntersectLocal(ref Ray r)
        {
            var (xtMin, xtMax) = CheckAxis(r.Origin.X, r.Direction.X);
            var (ytMin, ytMax) = CheckAxis(r.Origin.Y, r.Direction.Y);
            var (ztMin, ztMax) = CheckAxis(r.Origin.Z, r.Direction.Z);

            var tMin = MathF.Max(MathF.Max(xtMin, ytMin), ztMin);
            var tMax = MathF.Min(MathF.Min(xtMax, ytMax), ztMax);
            if (tMin > tMax || tMax < 0) return new List<Intersection>();
            return new List<Intersection>
            {
                new(tMin, this),
                new(tMax, this)
            };
        }

        public override Vector4 LocalNormal(Vector4 at)
        {
            var n = Vector4.Abs(at);
            var maxC = MathF.Max(MathF.Max(n.X, n.Y), n.Z);
            if (maxC.Equal(n.X)) return Vector4.UnitX * at.X;
            if (maxC.Equal(n.Y)) return Vector4.UnitY * at.Y;
            return Vector4.UnitZ * at.Z;
        }
    }
}
