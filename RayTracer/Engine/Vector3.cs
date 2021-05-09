using RayTracer.Extension;
using System;

namespace RayTracer.Engine
{
    /// <summary>
    /// The Books Tuple implementation called Vector3 and by default assumes w = 0
    /// I will probably use the system.numerics structs anyway
    /// </summary>
    public readonly struct Vector3 : IEquatable<Vector3>
    {
        public static Vector3 Point(float x, float y, float z) => new(x, y, z, 1.0f);
        public static Vector3 Vector(float x, float y, float z) => new(x, y, z);
        public static Vector3 OfArray(float[] f) => new(f[0], f[1], f[2], f[3]);

        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public float W { get; }

        public Vector3(float x, float y, float z, float w = 0f)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public bool IsVector => W == 0f;
        public bool IsPoint => !IsVector;
        public bool IsNormalized => MathF.Abs(Magnitude - 1f) < float.Epsilon;

        public Vector3 Normalized => this / Magnitude;

        public float Magnitude => MathF.Sqrt(X * X + Y * Y + Z * Z + W * W);

        public static float Dot(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;

        public static Vector3 Cross(Vector3 a, Vector3 b) =>
            new(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);

        public Vector3 Translate(float x, float y, float z) => Matrix.Translation(x, y, z) * this;

        public static bool operator ==(Vector3 left, Vector3 right) => left.Equals(right);
        public static bool operator !=(Vector3 left, Vector3 right) => !left.Equals(right);
        public static Vector3 operator +(Vector3 left, Vector3 right) =>
            new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        public static Vector3 operator -(Vector3 left, Vector3 right) =>
            new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
        public static Vector3 operator -(Vector3 v) => new(-v.X, -v.Y, -v.Z, -v.W);
        public static Vector3 operator *(Vector3 v, float s) => new(v.X * s, v.Y * s, v.Z * s, v.W * s);
        public static Vector3 operator *(float s, Vector3 v) => v * s;
        public static Vector3 operator /(Vector3 v, float s) => new(v.X / s, v.Y / s, v.Z / s, v.W / s);

        /// <inheritdoc/>
        public bool Equals(Vector3 other) =>
            X.Equal(other.X) && Y.Equal(other.Y) && Z.Equal(other.Z) && W.Equal(other.W);

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Vector3 other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

        public override string ToString() => (IsVector ? "Vector" : "Point") + $"({X},{Y},{Z})";
    }
}