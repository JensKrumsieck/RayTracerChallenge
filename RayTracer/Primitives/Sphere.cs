using RayTracer.Engine;
using System;

namespace RayTracer.Primitives
{
    public class Sphere : Transform
    {
        public float Radius;

        public Sphere(Vector3 position, float radius = 1f) : base(position)
        {
            Radius = radius;
        }

        public override HitInfo[] Intersect(Ray ray)
        {
            var sphereToRay = ray.Origin - Position;
            var a = Vector3.Dot(ray.Direction, ray.Direction);
            var b = 2f * Vector3.Dot(ray.Direction, sphereToRay);
            var c = Vector3.Dot(sphereToRay, sphereToRay) - Radius;
            var d = b * b - 4f * a * c;
            if (d < 0) return Array.Empty<HitInfo>();
            return new[]
            {
                new HitInfo((-b - MathF.Sqrt(d)) / (2f * a), this),
                new HitInfo((-b + MathF.Sqrt(d)) / (2f * a), this)
            };
        }
    }
}
