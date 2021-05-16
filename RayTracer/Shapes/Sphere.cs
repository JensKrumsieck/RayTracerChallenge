using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using RayTracer.Materials;
using static RayTracer.Extension.VectorExtension;
using static System.MathF;

namespace RayTracer.Shapes
{
    public sealed class Sphere : IRayObject, IShadedObject
    {
        public Transform Transform;

        public Sphere(Transform transform)
        {
            Transform = transform;
            Material = PhongMaterial.Default;
        }
        public Sphere() : this(Transform.Identity) { }

        /// <inheritdoc />
        public Intersection[] Intersect(in Ray r)
        {
            var r2 = r.Transform(Transform.Inverse);
            var str = r2.Origin - Point(0f, 0f, 0f);
            var a = Vector4.Dot(r2.Direction, r2.Direction);
            var b = 2f * Vector4.Dot(r2.Direction, str);
            var c = Vector4.Dot(str, str) - 1f;
            var discriminant = b * b - 4 * a * c;
            if (discriminant < 0) return Array.Empty<Intersection>();
            return new[]
            {
                new Intersection((-b - Sqrt(discriminant)) / (2 * a), this),
                new Intersection((-b + Sqrt(discriminant)) / (2 * a), this)
            };
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Intersection? Hit(in Ray r)
        {
            var xs = Intersect(r);
            return Intersection.Hit(xs);
        }

        /// <inheritdoc />
        public Vector4 LocalNormal(in Vector4 at) => Vector4.Normalize(at - Point(0f,0f,0f));

        /// <inheritdoc />
        public Vector4 Normal(in Vector4 at)
        {
            var obj = Transform.WorldToObject(at);
            var localNormal = LocalNormal(obj);
            var worldNormal = Transform.ObjectToWorld(localNormal);
            worldNormal.W = 0f;
            return Vector4.Normalize(worldNormal);
        }

        public PhongMaterial Material { get; set; }
    }
}
