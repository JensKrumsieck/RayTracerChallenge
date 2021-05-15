using RayTracer.Extension;
using SkiaSharp;
using System;
using System.Numerics;

namespace RayTracer.Engine
{
    public readonly struct Color : IEquatable<Color>
    {
        public static readonly Color Black = new(0, 0, 0);
        public static readonly Color Red = new(1, 0, 0);
        public static readonly Color Green = new(0, 1, 0);
        public static readonly Color Blue = new(0, 0, 1);
        public static readonly Color White = new(1, 1, 1);

        private readonly Vector3 _storage;
        public float R => _storage.X;
        public float G => _storage.Y;
        public float B => _storage.Z;

        public Color(float r, float g, float b)
        {
            _storage = new Vector3(r, g, b);
        }

        public Color(Vector3 vec) => _storage = vec;

        #region Operators
        public static bool operator ==(Color left, Color right) => left.Equals(right);
        public static bool operator !=(Color left, Color right) => !left.Equals(right);
        public static Color operator +(Color left, Color right) => new(left._storage + right._storage);
        public static Color operator -(Color left, Color right) => new(left._storage - right._storage);
        public static Color operator *(Color left, Color right) => new(left._storage * right._storage);
        public static Color operator *(Color col, float s) => new(col._storage * s);
        public static Color operator *(float s, Color col) => col * s;
        public static Color operator /(Color col, float s) => new(col._storage / s);
        #endregion

        #region IEquatable
        /// <inheritdoc />
        public bool Equals(Color other) => R.Equal(other.R) && G.Equal(other.G) & B.Equal(other.B);

        public bool Equals(Color other, float threshold) => R.Equal(other.R, threshold) && G.Equal(other.G, threshold) & B.Equal(other.B, threshold);
        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Color other && Equals(other);
        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(R, G, B);
        #endregion

        public override string ToString() => $"R: {R} G: {G} B: {B}";

        /// <summary>
        /// Converts color to SKColor structure
        /// </summary>
        /// <returns></returns>
        public SKColor ToSKColor() => new(R.ToByte(), G.ToByte(), B.ToByte());

        /// <summary>
        /// Converts Color to Bytes
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<byte> ToBytes() => new(new[] { R.ToByte(), G.ToByte(), B.ToByte() });
    }
}
