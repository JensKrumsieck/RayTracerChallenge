using System.Numerics;

namespace RayTracer.Extension
{
    public static class MatrixExtension
    {
        public static float[,] ToArray(this Matrix4x4 m) => new float[,]
        {
            {m.M11, m.M12, m.M13, m.M14},
            {m.M21, m.M22, m.M23, m.M24},
            {m.M31, m.M32, m.M33, m.M34},
            {m.M41, m.M42, m.M43, m.M44}
        };

        public static Matrix4x4 ToMatrix(this float[,] f) => new(
            f[0, 0], f[0, 1], f[0, 2], f[0, 3],
            f[1, 0], f[1, 1], f[1, 2], f[1, 3],
            f[2, 0], f[2, 1], f[2, 2], f[2, 3],
            f[3, 0], f[3, 1], f[3, 2], f[3, 3]);

        /// <summary>
        /// Returns self or inverted matrix
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Matrix4x4 Invert(this Matrix4x4 m) => Matrix4x4.Invert(m, out var result) ? result : m;
    }
}
