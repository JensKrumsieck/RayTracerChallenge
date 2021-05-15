using RayTracer.Engine;
using RayTracer.Extension;
using System;
using System.Numerics;

namespace RayTracer.Primitives
{
    public sealed class Sphere : Transform
    {
        public Sphere(Vector3 position, float radius = 1f)
        {
            TransformationMatrix = Matrix.TranslationMatrix(position) * Matrix.ScaleMatrix(radius);
        }

        public Sphere(float radius = 1f)
        {
            TransformationMatrix = Matrix.ScaleMatrix(radius);
        }

        public override Vector3 Normal(Vector3 worldPoint)
        {
            var objectPoint = WorldToObject(worldPoint);
            var objectNormal = Vector3.Normalize(objectPoint - Vector3.Zero);
            var normal = ObjectToWorld(objectNormal);
            return Vector3.Normalize(normal);
        }

        public override HitInfo[] Intersect(Ray ray)
        {
            ray = ray.Transform(TransformationMatrix.Invert());
            var sphereToRay = ray.Origin - Position;
            var a = Vector3.Dot(ray.Direction, ray.Direction);
            var b = 2f * Vector3.Dot(ray.Direction, sphereToRay);
            var c = Vector3.Dot(sphereToRay, sphereToRay) - 1f;
            var d = b * b - 4f * a * c;
            if (d < 0) return Array.Empty<HitInfo>();
            var t1 = (-b - MathF.Sqrt(d)) / (2f * a);
            var t2 = (-b + MathF.Sqrt(d)) / (2f * a);
            return new[]
            {
                new HitInfo(t1, this),
                new HitInfo(t2, this)
            };
        }
    }
}
