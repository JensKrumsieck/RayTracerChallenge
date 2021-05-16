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
            }
        }

        public Matrix4x4 Inverse { get; private set; }


        public Transform(Matrix4x4 matrix)
        {
            _matrix = matrix;
            Inverse = _matrix.Inverse();
        }

        public static implicit operator Matrix4x4(Transform t) => t.Matrix;
        public static implicit operator Transform(Matrix4x4 m) => new(m);
    }
}
