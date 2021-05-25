﻿using System;
using System.Numerics;

namespace RayTracer.Extension
{
    public static class VectorExtension
    {
        public static Vector4 PointOne => Vector4.One;
        public static Vector4 PointMinusOne = Point(-1, -1, -1);
        public static Vector4 PointZero => Point(0, 0, 0);

        public static bool IsPoint(this Vector4 v) => !IsVector(v);
        public static bool IsVector(this Vector4 v) => v.W < Constants.EpsilonHigh;

        public static Vector4 Point(float x, float y, float z) => new(x, y, z, 1.0f);
        public static Vector4 Direction(float x, float y, float z) => new(x, y, z, 0.0f);
        public static Vector4 Direction(Vector3 v) => Direction(v.X, v.Y, v.Z);

        public static Vector4 Cross(this Vector4 a, Vector4 b)
        {
#if DEBUG
            if (a.IsPoint() || b.IsPoint()) throw new NotSupportedException("Points are not supported here!");
#endif
            return new Vector4(Vector3.Cross(a.ToVector3(), b.ToVector3()), 0f);
        }

        public static Vector4 Reflect(this Vector4 a, Vector4 normal) => a - normal * 2 * Vector4.Dot(a, normal);

        private static Vector3 ToVector3(this Vector4 v) => new(v.X, v.Y, v.Z);

        public static void Deconstruct(this Vector4 v, out float x, out float y, out float z)
        {
            x = v.X;
            y = v.Y;
            z = v.Z;
        }

        public static void Deconstruct(this Vector3 v, out float x, out float y, out float z)
        {
            x = v.X;
            y = v.Y;
            z = v.Z;
        }
    }
}
