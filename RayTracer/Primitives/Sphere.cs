using RayTracer.Engine;
using System;

namespace RayTracer.Primitives
{
    public class Sphere
    {
        public Vector3 Position;
        public float Radius;

        public Sphere(Vector3 position, float radius = 1f)
        {
            Position = position;
            Radius = radius;
        }

        public float[] Intersect(Ray ray)
        {
            var sphereToRay = ray.Origin - Position;
            var a = Vector3.Dot(ray.Direction, ray.Direction);
            var b = 2f * Vector3.Dot(ray.Direction, sphereToRay);
            var c = Vector3.Dot(sphereToRay, sphereToRay) - Radius;
            var d = b * b - 4f * a * c;
            if (d < 0) return Array.Empty<float>();
            return new[]
            {
                -b - MathF.Sqrt(d) / 2f * a,
                -b + MathF.Sqrt(d) / 2f * a
            };
        }
    }
}
