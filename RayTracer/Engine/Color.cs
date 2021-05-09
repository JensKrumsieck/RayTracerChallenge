using RayTracer.Extension;
using SkiaSharp;
using System;

namespace RayTracer.Engine
{
    public readonly struct Color : IEquatable<Color>
    {
        public static Color Black = new(0, 0, 0);
        public static Color Red = new(1, 0, 0);
        public static Color Green = new(0, 1, 0);
        public static Color Blue = new(0, 0, 1);

        public readonly float R;
        public readonly float G;
        public readonly float B;

        public Color(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        #region Operators
        public static bool operator ==(Color left, Color right) => left.Equals(right);
        public static bool operator !=(Color left, Color right) => !left.Equals(right);
        public static Color operator +(Color left, Color right) => new(left.R + right.R, left.G + right.G, left.B + right.B);
        public static Color operator -(Color left, Color right) => new(left.R - right.R, left.G - right.G, left.B - right.B);
        public static Color operator *(Color left, Color right) => new(left.R * right.R, left.G * right.G, left.B * right.B);
        public static Color operator *(Color col, float s) => new(col.R * s, col.G * s, col.B * s);
        public static Color operator *(float s, Color col) => col * s;
        public static Color operator /(Color col, float s) => new(col.R / s, col.G / s, col.B / s);
        #endregion

        #region IEquatable
        /// <inheritdoc />
        public bool Equals(Color other) => R.Equal(other.R) && G.Equal(other.G) & B.Equal(other.B);
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
