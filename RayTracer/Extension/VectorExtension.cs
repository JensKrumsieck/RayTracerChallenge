using System;
using System.Numerics;

namespace RayTracer.Extension
{
    public static class VectorExtension
    {
        public static Vector4 Multiply(this Vector4 v, Matrix4x4 m)
        {
            var vStorage = v.ToArray();
            var mStorage = m.ToArray();
            var rStorage = new float[4];
            for (var i = 0; i < 4; i++)
            {
                var tmp = 0f;
                for (var j = 0; j < 4; j++)
                {
                    tmp += mStorage[i, j] * vStorage[j];
                }
                rStorage[i] = tmp;
            }
            return new Vector4(rStorage[0], rStorage[1], rStorage[2], rStorage[3]);
        }
        public static Vector3 Multiply(this Vector3 v, Matrix4x4 m) => v.AsPoint4().Multiply(m).AsVector3();
        public static Vector3 MultiplyVector(this Vector3 v, Matrix4x4 m) => v.AsVector4().Multiply(m).AsVector3();

        public static Vector4 AsVector4(this Vector3 v) => new(v.X, v.Y, v.Z, 0f);
        public static Vector4 AsPoint4(this Vector3 v) => new(v.X, v.Y, v.Z, 1f);
        public static Vector3 AsVector3(this Vector4 v) => new(v.X, v.Y, v.Z);
        public static ReadOnlySpan<float> ToArray(this Vector4 v) => new[] { v.X, v.Y, v.Z, v.W };
    }

}
