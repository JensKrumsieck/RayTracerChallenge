using RayTracer.Extension;
using SkiaSharp;
using System;
using System.Numerics;

namespace RayTracer
{
    public struct Color : IEquatable<Color>
    {
        public static Color Black => new(0f, 0f, 0f);
        public static Color White => new(1f, 1f, 1f);
        public static Color Red => new(1f, 0f, 0f);

        private readonly Vector3 _storage;

        public float R => _storage.X;
        public float G => _storage.Y;
        public float B => _storage.Z;

        public Color(float x, float y, float z) => _storage = new Vector3(x, y, z);

        #region Converters
        public override string ToString() => $"Color: R{R} G{G} B{B}";
        public ReadOnlySpan<byte> ToBytes() => new[] { R.ToByte(), G.ToByte(), B.ToByte() };
        #endregion

        #region Operators
        public static implicit operator Vector3(Color c) => c._storage;
        public static implicit operator Vector4(Color c) => new(c._storage, 0f);
        public static implicit operator Color(Vector3 v) => new(v.X, v.Y, v.Z);
        public static implicit operator Color(Vector4 v) => new(v.X, v.Y, v.Z);
        public static implicit operator SKColor(Color c) => new(c.R.ToByte(), c.G.ToByte(), c.B.ToByte());

        public static Color operator +(Color a, Color b) => a._storage + b._storage;
        public static Color operator -(Color a, Color b) => a._storage - b._storage;
        public static Color operator *(Color a, Color b) => a._storage * b._storage;
        public static Color operator *(Color a, float f) => a._storage * f;
        public static Color operator /(Color a, Color b) => a._storage / b._storage;
        public static Color operator /(Color a, float f) => a._storage / f;

        public static bool operator ==(Color left, Color right) => left.Equals(right);
        public static bool operator !=(Color left, Color right) => !left.Equals(right);
        #endregion

        #region IEquatable
        public bool Equals(Color other) => _storage.Equals(other._storage);
        public override bool Equals(object obj) => obj is Color other && Equals(other);
        public override int GetHashCode() => _storage.GetHashCode();
        #endregion
    }
}
