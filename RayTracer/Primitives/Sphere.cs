using RayTracer.Engine;
using System;

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

        public override HitInfo[] Intersect(Ray ray)
        {
            ray = ray.Transform(TransformationMatrix.Inverse());
            var sphereToRay = ray.Origin - Position;
            var a = Vector3.Dot(ray.Direction, ray.Direction);
            var b = 2f * Vector3.Dot(ray.Direction, sphereToRay);
            var c = Vector3.Dot(sphereToRay, sphereToRay) - 1f;
            var d = b * b - 4f * a * c;
            if (d < 0) return Array.Empty<HitInfo>();
            return new[]
            {
                new HitInfo((-b - MathF.Sqrt(d)) / (2f * a), this),
                new HitInfo((-b + MathF.Sqrt(d)) / (2f * a), this)
            };
        }
    }

    public sealed class NativeSphere : NativeTransform
    {
        public float Radius => TransformationMatrix.M11; //for simplicity use uniform scale
        public NativeSphere(System.Numerics.Vector3 position, float radius = 1f) : base(position)
        {
            TransformationMatrix *=
                System.Numerics.Matrix4x4.CreateScale(new System.Numerics.Vector3(radius, radius, radius));
        }

        public NativeSphere(float radius = 1f)
        {
            TransformationMatrix *=
                System.Numerics.Matrix4x4.CreateScale(new System.Numerics.Vector3(radius, radius, radius));
        }

        public override NativeHitInfo[] Intersect(NativeRay ray)
        {
            System.Numerics.Matrix4x4.Invert(TransformationMatrix, out var inverted);
            ray = ray.Transform(inverted);
            var sphereToRay = ray.Origin - Position;
            var a = System.Numerics.Vector3.Dot(ray.Direction, ray.Direction);
            var b = 2f * System.Numerics.Vector3.Dot(ray.Direction, sphereToRay);
            var c = System.Numerics.Vector3.Dot(sphereToRay, sphereToRay) - 1f;
            var d = b * b - 4f * a * c;
            if (d < 0) return Array.Empty<NativeHitInfo>();
            return new[]
            {
                new NativeHitInfo((-b - MathF.Sqrt(d)) / (2f * a), this),
                new NativeHitInfo((-b + MathF.Sqrt(d)) / (2f * a), this)
            };
        }
    }
}
