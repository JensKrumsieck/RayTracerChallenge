using System.Numerics;
using static System.MathF;

namespace RayTracer.Extension
{
    public static class Matrix
    {
        public static float[,] ToArray(this Matrix4x4 m) => new [,]
        {
            {m.M11, m.M12, m.M13, m.M14},
            {m.M21, m.M22, m.M23, m.M24},
            {m.M31, m.M32, m.M33, m.M34},
            {m.M41, m.M42, m.M43, m.M44}
        };

        public static Matrix4x4 Invert(this Matrix4x4 m) => Matrix4x4.Invert(m, out var result) ? result : Matrix4x4.Identity;

        public static Matrix4x4 Transpose(this Matrix4x4 m) => Matrix4x4.Transpose(m);

        public static Matrix4x4 TranslationMatrix(Vector3 v) => Matrix4x4.CreateTranslation(v).Transpose();

        public static Matrix4x4 TranslationMatrix(float x, float y, float z) => TranslationMatrix(new Vector3(x, y, z));

        public static Matrix4x4 ScaleMatrix(float x, float y, float z) => ScaleMatrix(new Vector3(x, y, z));

        public static Matrix4x4 ScaleMatrix(Vector3 s) => Matrix4x4.CreateScale(s);

        public static Matrix4x4 ScaleMatrix(float f) => Matrix4x4.CreateScale(f, f, f);

        public static Matrix4x4 RotationXMatrix(float r) =>
            new(1f, 0f, 0f, 0f,
                0f, Cos(r), -Sin(r), 0f,
                0f, Sin(r), Cos(r), 0f,
                0f, 0f, 0f, 1f);

        public static Matrix4x4 RotationYMatrix(float r) =>
            new(Cos(r), 0f, Sin(r), 0f,
                0f, 1f, 0f, 0f,
                -Sin(r), 0f, Cos(r), 0f,
                0f, 0f, 0f, 1f);

        public static Matrix4x4 RotationZMatrix(float r) =>
            new(Cos(r), -Sin(r), 0f, 0f,
                Sin(r), Cos(r), 0f, 0f,
                0f, 0f, 1f, 0f,
                0f, 0f, 0f, 1f);

        public static Matrix4x4 SkewMatrix(float xy, float xz, float yx, float yz, float zx, float zy) =>
            new(1f, xy, xz, 0f,
                yx, 1f, yz, 0f,
                zx, zy, 1f, 0f,
                0f, 0f, 0f, 1f);
    }
}
