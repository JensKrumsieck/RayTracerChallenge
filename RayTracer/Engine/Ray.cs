#nullable enable
using System.ComponentModel;
using RayTracer.Extension;
using System.Numerics;

namespace RayTracer.Engine
{
    public struct Ray
    {
        public Vector3 Origin;
        public Vector3 Direction;

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public readonly Vector3 PointByDistance(float d) => Origin + Direction * d;

        public readonly Ray Transform(Matrix4x4 m) => new(Origin.Multiply(m), Direction.MultiplyVector(m));

        private static bool Intersect(Ray ray, Transform transform, out HitInfo[] hits)
        {
            hits = transform.Intersect(ray);
            return hits.Length != 0;
        }
        public static bool Intersect(Vector3 origin, Vector3 direction, Transform transform, out HitInfo[] hits) =>
            Intersect(new Ray(origin, direction), transform, out hits);

        public static HitInfo? Hit(Vector3 origin, Vector3 dir, Transform transform) => Hit(new Ray(origin, dir), transform);
        public static HitInfo? Hit(Ray ray, Transform transform) => transform.Hit(ray);
    }
}
