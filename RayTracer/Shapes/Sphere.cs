using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;
using static System.MathF;

namespace RayTracer.Shapes
{
    public sealed class Sphere : Entity
    {
        public Sphere(Transform transform) : base(transform) { }
        public Sphere() { }

        /// <inheritdoc />
        public override List<Intersection> IntersectLocal(in Ray r)
        {
            var str = r.Origin - Vector4.UnitW;
            var a = Vector4.Dot(r.Direction, r.Direction);
            var b = 2f * Vector4.Dot(r.Direction, str);
            var c = Vector4.Dot(str, str) - 1f;
            var discriminant = b * b - 4 * a * c;
            if (discriminant < 0) return new List<Intersection>();
            return new()
            {
                new Intersection((-b - Sqrt(discriminant)) / (2 * a), this),
                new Intersection((-b + Sqrt(discriminant)) / (2 * a), this)
            };
        }

        /// <inheritdoc />
        public override Vector4 LocalNormal(Vector4 at) => Vector4.Normalize(at - Point(0f, 0f, 0f));

        public static Sphere GlassSphere => new() { Material = { Transparency = 1, IOR = 1.5f } };
    }
}
