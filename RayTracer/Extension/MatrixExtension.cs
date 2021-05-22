using System;
using System.Numerics;
using static System.MathF;

namespace RayTracer.Extension
{
    public static class MatrixExtension
    {
        public static Vector4 Multiply(this Matrix4x4 m, Vector4 v)
            => new(
            m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z + m.M14 * v.W,
            m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z + m.M24 * v.W,
            m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z + m.M34 * v.W,
            m.M41 * v.X + m.M42 * v.Y + m.M43 * v.Z + m.M44 * v.W
        );

        public static bool IsInvertible(this Matrix4x4 m) => m.GetDeterminant() != 0;

        public static Matrix4x4 Inverse(this Matrix4x4 m)
        {
            if (!Matrix4x4.Invert(m, out var result))
                throw new NotSupportedException($"Matrix is not Invertible, Determinant is {m.GetDeterminant()}");
            return result;
        }

        public static Matrix4x4 Translation(float x, float y, float z) =>
            new(
                1f, 0f, 0f, x,
                0f, 1f, 0f, y,
                0f, 0f, 1f, z,
                0f, 0f, 0f, 1f);

        public static Matrix4x4 Translation(Vector3 v) => Translation(v.X, v.Y, v.Z);

        public static Matrix4x4 Scale(float x, float y, float z) =>
            new(
                x, 0f, 0f, 0f,
                0f, y, 0f, 0f,
                0f, 0f, z, 0f,
                0f, 0f, 0f, 1f);

        public static Matrix4x4 Scale(float f) => Scale(f, f, f);

        public static Matrix4x4 RotationX(float r) =>
            new(
                1f, 0f, 0f, 0f,
                0f, Cos(r), -Sin(r), 0f,
                0f, Sin(r), Cos(r), 0f,
                0f, 0f, 0f, 1f);

        public static Matrix4x4 RotationY(float r) =>
            new(
                Cos(r), 0f, Sin(r), 0f,
                0f, 1f, 0f, 0f,
                -Sin(r), 0f, Cos(r), 0f,
                0f, 0f, 0f, 1f);

        public static Matrix4x4 RotationZ(float r) =>
            new(
                Cos(r), -Sin(r), 0f, 0f,
                Sin(r), Cos(r), 0f, 0f,
                0f, 0f, 1f, 0f,
                0f, 0f, 0f, 1f);

        public static Matrix4x4 Shear(float xy, float xz, float yx, float yz, float zx, float zy) =>
            new(
                1f, xy, xz, 0f,
                yx, 1f, yz, 0f,
                zx, zy, 1f, 0f,
                0f, 0f, 0f, 1f
            );
    }
}
