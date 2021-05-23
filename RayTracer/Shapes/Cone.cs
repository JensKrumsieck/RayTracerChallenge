using RayTracer.Extension;
using System;
using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Shapes
{
    public class Cone : Conic
    {
        public Cone() { }
        public Cone(Transform transform) : base(transform) { }
        public override List<Intersection> IntersectLocal(ref Ray r)
        {
            var xs = new List<Intersection>();
            var a =
                r.Direction.X * r.Direction.X -
                r.Direction.Y * r.Direction.Y +
                r.Direction.Z * r.Direction.Z;

            var b =
                2 * r.Origin.X * r.Direction.X -
                2 * r.Origin.Y * r.Direction.Y +
                2 * r.Origin.Z * r.Direction.Z;

            var c =
                r.Origin.X * r.Origin.X -
                r.Origin.Y * r.Origin.Y +
                r.Origin.Z * r.Origin.Z;
            if (a.Equal(0) && !b.Equal(0)) xs.Add(new Intersection(-c / (2 * b), this));
            if (!a.Equal(0))
            {
                var discriminant = b * b - 4 * a * c;
                if (discriminant + Constants.Epsilon < 0) return new List<Intersection>();
                var t0 = (-b - MathF.Sqrt(discriminant)) / (2 * a);
                var t1 = (-b + MathF.Sqrt(discriminant)) / (2 * a);

                var y0 = r.Origin.Y + t0 * r.Direction.Y;
                if (Minimum < y0 && y0 < Maximum) xs.Add(new Intersection(t0, this));
                var y1 = r.Origin.Y + t1 * r.Direction.Y;
                if (Minimum < y1 && y1 < Maximum) xs.Add(new Intersection(t1, this));
            }
            IntersectCaps(ref r, ref xs, true);
            return xs;
        }

        public override Vector4 LocalNormal(Vector4 at)
        {
            var dist = at.X * at.X + at.Z * at.Z;
            var y = MathF.Sqrt(dist);
            if (at.Y > 0) y = -y;
            if (dist < Maximum * Maximum && at.Y >= Maximum + Constants.Epsilon) return Vector4.UnitY;
            if (dist < Minimum * Minimum && at.Y <= Minimum + Constants.Epsilon) return -Vector4.UnitY;
            return Direction(at.X, y, at.Z);
        }
    }
}
