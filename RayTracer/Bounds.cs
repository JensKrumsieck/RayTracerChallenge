using RayTracer.Extension;
using System;
using System.Linq;
using System.Numerics;
using static RayTracer.Extension.GeometricExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer
{
    public struct Bounds
    {
        public Vector4 Min;
        public Vector4 Max;

        public static readonly Bounds DefaultBox = new() { Min = PointMinusOne, Max = PointOne };

        public static readonly Bounds Empty = new()
        {
            Min = Point(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity),
            Max = Point(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity)
        };

        public readonly bool IntersectLocal(ref Ray r)
        {
            var (xtMin, xtMax) = CheckAxis(r.Origin.X, r.Direction.X, Min.X, Max.X);
            var (ytMin, ytMax) = CheckAxis(r.Origin.Y, r.Direction.Y, Min.Y, Max.Y);
            if (xtMin > ytMax || ytMin > xtMax) return false;
            var (ztMin, ztMax) = CheckAxis(r.Origin.Z, r.Direction.Z, Min.Z, Max.Z);

            var tMin = MathF.Max(MathF.Max(xtMin, ytMin), ztMin);
            var tMax = MathF.Min(MathF.Min(xtMax, ytMax), ztMax);
            return tMin < tMax;
        }

        public void Add(params Vector4[] points)
        {
            var xMin = points.Min(s => s.X);
            var yMin = points.Min(s => s.Y);
            var zMin = points.Min(s => s.Z);
            var xMax = points.Max(s => s.X);
            var yMax = points.Max(s => s.Y);
            var zMax = points.Max(s => s.Z);
            Add(Point(xMin, yMin, zMin));
            Add(Point(xMax, yMax, zMax));
        }

        public void Add(Vector4 point)
        {
            var x = MathF.Min(point.X, Min.X);
            var y = MathF.Min(point.Y, Min.Y);
            var z = MathF.Min(point.Z, Min.Z);
            Min = Point(x, y, z);
            x = MathF.Max(point.X, Max.X);
            y = MathF.Max(point.Y, Max.Y);
            z = MathF.Max(point.Z, Max.Z);
            Max = Point(x, y, z);
        }

        public void Add(Bounds b) => Add(b.Max, b.Min);

        public readonly bool Contains(Vector4 p) =>
            Min.X <= p.X && p.X <= Max.X &&
            Min.Y <= p.Y && p.Y <= Max.Y &&
            Min.Z <= p.Z && p.Z <= Max.Z;

        public readonly bool Contains(Bounds b) => Contains(b.Min) && Contains(b.Max);

        public readonly Bounds Transform(Matrix4x4 t)
        {
            var p1 = Min;
            var p2 = Point(Min.X, Min.Y, Max.Z);
            var p3 = Point(Min.X, Max.Y, Min.Z);
            var p4 = Point(Min.X, Max.Y, Max.Z);
            var p5 = Point(Max.X, Min.Y, Min.Z);
            var p6 = Point(Max.X, Min.Y, Max.Z);
            var p7 = Point(Max.X, Max.Y, Min.Z);
            var p8 = Max;
            var b = Empty;
            foreach (var p in new[] { p1, p2, p3, p4, p5, p6, p7, p8 }) b.Add(t.Multiply(p));
            return b;
        }

        public readonly (Bounds left, Bounds right) Split()
        {
            var dx = MathF.Abs(Min.X - Max.X);
            var dy = MathF.Abs(Min.Y - Max.Y);
            var dz = MathF.Abs(Min.Z - Max.Z);
            var greatest = MathF.Max(MathF.Max(dx, dy), dz);
            var (x0, y0, z0) = Min;
            var (x1, y1, z1) = Max;
            if (greatest.Equals(dx)) x0 = x1 = x0 + dx / 2f;
            else if (greatest.Equals(dy)) y0 = y1 = y0 + dy / 2f;
            else if (greatest.Equals(dz)) z0 = z1 = z0 + dz / 2f;
            var minMin = Point(x0, y0, z0);
            var midMax = Point(x1, y1, z1);
            var left = new Bounds { Min = Min, Max = midMax };
            var right = new Bounds { Min = minMin, Max = Max };
            return (left, right);
        }
    }
}
