using System.Numerics;

namespace RayTracer.Extension
{
    public static class VectorExtension
    {
        public static Vector4 Translate(this Vector4 v, float x, float y, float z) => Vector4.Transform(v, Matrix4x4.CreateTranslation(x, y, z));
        public static Vector4 Scale(this Vector4 v, float x, float y, float z) => Vector4.Transform(v, Matrix4x4.CreateScale(x, y, z));
        public static Vector4 RotateX(this Vector4 v, float r) => Vector4.Transform(v, Matrix4x4.CreateRotationX(r));
        public static Vector4 RotateY(this Vector4 v, float r) => Vector4.Transform(v, Matrix4x4.CreateRotationY(r));
        public static Vector4 RotateZ(this Vector4 v, float r) => Vector4.Transform(v, Matrix4x4.CreateRotationZ(r));
        public static Vector4 Skew(this Vector4 v, float xy, float xz, float yx, float yz, float zx, float zy) => Vector4.Transform(v, Matrix4x4.Transpose(new Matrix4x4(1f, xy, xz, 0f, yx, 1f, yz, 0f, zx, zy, 1f, 0f, 0f, 0f, 0f, 1f)));

        public static float Dot(this Vector4 left, Vector4 right) => Vector4.Dot(left, right);
        public static Vector4 Cross(this Vector4 a, Vector4 b) => new(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X, 0f);
        public static Vector4 Normalize(this Vector4 v) => Vector4.Normalize(v);

        public static Vector4 AsVector4(this Vector3 v) => new(v.X, v.Y, v.Z, 0f);
        public static Vector3 AsVector3(this Vector4 v) => new(v.X, v.Y, v.Z);
    }

}
