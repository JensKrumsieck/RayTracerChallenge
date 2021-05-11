﻿using RayTracer.Engine;
using System;
using System.Numerics;

namespace RayTracer.Primitives
{
    public sealed class Sphere : Transform
    {
        public float Radius => TransformationMatrix[0, 0]; //for simplicity use uniform scale

        public Sphere(Vector3 position, float radius = 1f) : base(position)
        {
            TransformationMatrix *= Matrix.Scale(radius, radius, radius);
        }

        public Sphere(float radius = 1f)
        {
            TransformationMatrix *= Matrix.Scale(radius, radius, radius);
        }

        public override HitInfo[] Intersect(Ray ray, bool hit = false)
        {
            ray = ray.Transform(TransformationMatrix.Inverse());
            var sphereToRay = ray.Origin.ToVector3() - Position;
            var a = Vector3.Dot(ray.Direction.ToVector3(), ray.Direction.ToVector3());
            var b = 2f * Vector3.Dot(ray.Direction.ToVector3(), sphereToRay);
            var c = Vector3.Dot(sphereToRay, sphereToRay) - 1f;
            var d = b * b - 4f * a * c;
            if (d < 0) return Array.Empty<HitInfo>();
            var t1 = (-b - MathF.Sqrt(d)) / (2f * a);
            var t2 = (-b + MathF.Sqrt(d)) / (2f * a);
            if (hit) return new[] { new HitInfo(MathF.Min(t1, t2), this) };
            return new[]
            {
                new HitInfo(t1, this),
                new HitInfo(t2, this)
            };
        }
    }
}
