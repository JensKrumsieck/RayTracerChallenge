using RayTracer.Extension;
using System;
using System.Numerics;
using static RayTracer.Constants;

namespace RayTracer.Engine
{
    public readonly struct Vector : IEquatable<Vector>
    {
        private readonly Vector4 _storage;

        public static readonly Vector UnitX = new(1f, 0f, 0f);
        public static readonly Vector UnitY = new(0f, 1f, 0f);
        public static readonly Vector UnitZ = new(0f, 0f, 1f);
        public static Vector Point(float x, float y, float z) => new Vector4(x, y, z, 1f);
        public static Vector Point(Vector3 v) => new Vector4(v, 1f);

        public Vector(float x, float y, float z)
        {
            _storage = new Vector4(x, y, z, 0f);
        }

        public Vector(float x, float y, float z, float w)
        {
            _storage = new Vector4(x, y, z, w);
        }

        public float X => _storage.X;
        public float Y => _storage.Y;
        public float Z => _storage.Z;
        public float W => _storage.W;

        public Vector(Vector4 v) => _storage = v;

        public bool IsVector => _storage.W == 0;
        public bool IsPoint => !IsVector;

        public bool IsNormalized => MathF.Abs(Magnitude - 1f) < float.Epsilon;
        public Vector4 Normalized => _storage.Normalize();

        public float Magnitude => _storage.Length();
        public float Dot(Vector other) => _storage.Dot(other);
        public Vector Cross(Vector other) => _storage.Cross(other);
        public static float Dot(Vector left, Vector right) => left.Dot(right);

        public Vector Translate(float x, float y, float z) => _storage.Translate(x, y, z);
        public Vector Scale(float x, float y, float z) => _storage.Scale(x, y, z);
        public Vector RotateX(float r) => _storage.RotateX(r);
        public Vector RotateY(float r) => _storage.RotateY(r);
        public Vector RotateZ(float r) => _storage.RotateZ(r);

        public Vector Skew(float xy, float xz, float yx, float yz, float zx, float zy) =>
            _storage.Skew(xy, xz, yx, yz, zx, zy);

        public static implicit operator Vector(Vector4 v) => new(v);
        public static implicit operator Vector4(Vector v) => v._storage;
        public static implicit operator Vector(Vector3 v) => new(v.X, v.Y, v.Z);
        public static implicit operator Vector3(Vector v) => new(v.X, v.Y, v.Z);

        public static bool operator ==(Vector left, Vector right) => left.Equals(right);
        public static bool operator !=(Vector left, Vector right) => !left.Equals(right);
        public static Vector operator +(Vector left, Vector right) => Vector4.Add(left._storage, right._storage);
        public static Vector operator -(Vector left, Vector right) => Vector4.Subtract(left, right);

        public static Vector operator -(Vector v) => Vector4.Negate(v);
        public static Vector operator *(Vector v, float s) => Vector4.Multiply(v, s);
        public static Vector operator *(float s, Vector v) => v * s;
        public static Vector operator /(Vector v, float s) => Vector4.Divide(v, s);

        /// <inheritdoc/>
        public bool Equals(Vector other) => Equals(other, Epsilon);

        public bool Equals(Vector other, float threshold) => MathF.Abs(X - other.X) < threshold &&
                                                             MathF.Abs(Y - other.Y) < threshold &&
                                                             MathF.Abs(Z - other.Z) < threshold &&
                                                             MathF.Abs(W - other.W) < threshold;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Vector other && Equals(other);
        /// <inheritdoc/>
        public override int GetHashCode() => _storage.GetHashCode();
        public override string ToString() => $"{(IsPoint ? "Point" : "Vector")}: ({_storage.X}, {_storage.Y}, {_storage.Z})";
        public Vector3 ToVector3() => new(X, Y, Z);
    }
}