using System;
using System.Numerics;

namespace RayTracer.Extension
{
    public static class VectorExtension
    {
        public static bool IsPoint(this Vector4 v) => MathF.Abs(v.W - 1.0f) < float.Epsilon;
        public static bool IsVector(this Vector4 v) => !v.IsPoint();

        public static Vector4 Point(float x, float y, float z) => new(x, y, z, 1.0f);
        public static Vector4 Direction(float x, float y, float z) => new(x, y, z, 0.0f);

        public static Vector4 Cross(this Vector4 a, Vector4 b)
        {
#if DEBUG
            if (a.IsPoint() || b.IsPoint()) throw new NotSupportedException("Points are not supported here!");
#endif
            return new Vector4(Vector3.Cross(a.ToVector3(), b.ToVector3()), 0f);
        }

        public static Vector4 Reflect(this Vector4 a, Vector4 normal) => a - normal * 2 * Vector4.Dot(a, normal);

        private static Vector3 ToVector3(this Vector4 v) => new(v.X, v.Y, v.Z);
    }
}
