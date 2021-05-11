using RayTracer.Engine;
using System;
using System.Numerics;
using Vector = RayTracer.Engine.Vector;

namespace RayTracer.Primitives
{
    public sealed class Sphere : Transform
    {
        public float Radius => TransformationMatrix[0, 0]; //for simplicity use uniform scale
        public Sphere(Vector position, float radius = 1f) : base(position)
        {
            TransformationMatrix *= Matrix.Scale(radius, radius, radius);
        }

        public Sphere(float radius = 1f)
        {
            TransformationMatrix *= Matrix.Scale(radius, radius, radius);
        }

        public override HitInfo[] Intersect(Ray ray)
        {
            ray = ray.Transform(TransformationMatrix.Inverse());
            var sphereToRay = ray.Origin - Position;
            var a = Vector.Dot(ray.Direction, ray.Direction);
            var b = 2f * Vector.Dot(ray.Direction, sphereToRay);
            var c = Vector.Dot(sphereToRay, sphereToRay) - 1f;
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
