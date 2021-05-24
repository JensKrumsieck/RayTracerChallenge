using RayTracer.Extension;
using System.Numerics;

namespace RayTracer
{
    public struct Transform
    {
        public static Transform Identity => new(Matrix4x4.Identity);

        private Matrix4x4 _matrix;
        public Matrix4x4 Matrix
        {
            get => _matrix;
            set
            {
                _matrix = value;
                Inverse = _matrix.Inverse();
                TransposeInverse = Matrix4x4.Transpose(Inverse);
            }
        }

        /// <summary>
        /// Caches Inverse Transform
        /// </summary>
        public Matrix4x4 Inverse { get; private set; }
        /// <summary>
        /// Caches Transposed Inverse Transform
        /// </summary>
        public Matrix4x4 TransposeInverse { get; private set; }


        public Transform(Matrix4x4 matrix)
        {
            _matrix = matrix;
            Inverse = _matrix.Inverse();
            TransposeInverse = Matrix4x4.Transpose(Inverse);
        }

        public static implicit operator Matrix4x4(Transform t) => t.Matrix;
        public static implicit operator Transform(Matrix4x4 m) => new(m);

        public readonly Vector4 ObjectToWorld(in Vector4 objectV) => TransposeInverse.Multiply(objectV);
        public readonly Vector4 WorldToObject(in Vector4 worldV) => Inverse.Multiply(worldV);
    }
}
