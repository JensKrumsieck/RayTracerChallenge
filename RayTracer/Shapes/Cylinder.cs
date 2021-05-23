using RayTracer.Extension;
using System;
using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Shapes
{
    public class Cylinder : Conic
    {
        public Cylinder() { }

        public Cylinder(Transform transform) : base(transform) { }
        public override List<Intersection> IntersectLocal(ref Ray r)
        {
            var xs = new List<Intersection>();
            var a = r.Direction.X * r.Direction.X + r.Direction.Z * r.Direction.Z;
            if (a > Constants.Epsilon)
            {
                var b =
                    2 * r.Origin.X * r.Direction.X +
                    2 * r.Origin.Z * r.Direction.Z;
                var c =
                    r.Origin.X * r.Origin.X +
                    r.Origin.Z * r.Origin.Z - 1;
                var discriminant = b * b - 4 * a * c;
                if (discriminant < 0) return new List<Intersection>();
                var t0 = (-b - MathF.Sqrt(discriminant)) / (2 * a);
                var t1 = (-b + MathF.Sqrt(discriminant)) / (2 * a);

                if (t0 > t1)
                {
                    var tmp = t0;
                    t0 = t1;
                    t1 = tmp;
                }

                var y0 = r.Origin.Y + t0 * r.Direction.Y;
                if (Minimum < y0 && y0 < Maximum) xs.Add(new Intersection(t0, this));
                var y1 = r.Origin.Y + t1 * r.Direction.Y;
                if (Minimum < y1 && y1 < Maximum) xs.Add(new Intersection(t1, this));
            }

            IntersectCaps(ref r, ref xs);

            return xs;
        }

        private void IntersectCaps(ref Ray r, ref List<Intersection> xs)
        {
            if (!IsClosed || r.Direction.Y.Equal(0)) return;
            var t = (Minimum - r.Origin.Y) / r.Direction.Y;
            if (CheckCap(ref r, t)) xs.Add(new Intersection(t, this));
            t = (Maximum - r.Origin.Y) / r.Direction.Y;
            if (CheckCap(ref r, t)) xs.Add(new Intersection(t, this));
        }

        public override Vector4 LocalNormal(Vector4 at)
        {
            var dist = at.X * at.X + at.Z * at.Z;
            return dist switch
            {
                < 1 when at.Y >= Maximum - Constants.Epsilon => Vector4.UnitY,
                < 1 when at.Y <= Minimum + Constants.Epsilon => -Vector4.UnitY,
                _ => Direction(at.X, 0, at.Z)
            };
        }
    }
}
