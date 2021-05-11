using RayTracer.Extension;
using System;
using System.Numerics;
using static RayTracer.Constants;

namespace RayTracer.Engine
{
    public struct Matrix : IEquatable<Matrix>
    {
        private readonly Matrix4x4 _storage;

        public float this[int i, int j] => _storage.ToArray()[i, j];

        public Matrix(float[,] array) => _storage = array.ToMatrix();
        public Matrix(Matrix4x4 m) => _storage = m;

        public static Matrix Identity => Matrix4x4.Identity;

        public static int Rows => 4;
        public static int Cols => 4;

        public bool Invertible => Determinant() != 0;

        public static implicit operator Matrix(Matrix4x4 m) => new(m);
        public static implicit operator Matrix4x4(Matrix m) => m._storage;

        /// <summary>
        /// Calculates transpose matrix
        /// </summary>
        /// <returns></returns>
        public Matrix Transpose() => Matrix4x4.Transpose(this);

        /// <summary>
        /// Returns Determinant for a matrix
        /// </summary>
        /// <returns></returns>
        public float Determinant() => _storage.GetDeterminant();

        /// <summary>
        /// Returns Inverse Matrix
        /// </summary>
        /// <returns></returns>
        public Matrix Inverse()
        {
            if (!Matrix4x4.Invert(this, out var inversion)) throw new InvalidOperationException("Matrix is not invertible!");
            return inversion;
        }

        /// <summary>
        /// Multiplies Vector with Matrix
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector Multiply(Vector v) => new Vector4(
            _storage.M11 * v.X + _storage.M12 * v.Y + _storage.M13 * v.Z + _storage.M14 * v.W,
            _storage.M21 * v.X + _storage.M22 * v.Y + _storage.M23 * v.Z + _storage.M24 * v.W,
            _storage.M31 * v.X + _storage.M32 * v.Y + _storage.M33 * v.Z + _storage.M34 * v.W,
            _storage.M41 * v.X + _storage.M42 * v.Y + _storage.M43 * v.Z + _storage.M44 * v.W
        );
        /// <summary>
        /// Builds Translation Matrix
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Matrix Translation(float x, float y, float z) => Matrix4x4.CreateTranslation(x, y, z);

        /// <summary>
        /// Builds Scaling Matrix
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Matrix Scale(float x, float y, float z) => Matrix4x4.CreateScale(x, y, z);

        /// <summary>
        /// Build 4x4 Rotation matrix for X
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Matrix RotationX(float r) => Matrix4x4.CreateRotationX(r);

        /// <summary>
        /// Build 4x4 Rotation matrix for Y
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Matrix RotationY(float r) => Matrix4x4.CreateRotationY(r);

        /// <summary>
        /// Build 4x4 Rotation matrix for Z
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Matrix RotationZ(float r) => Matrix4x4.CreateRotationZ(r);

        public static Matrix Skew(float xy, float xz, float yx, float yz, float zx, float zy)
        {
            var storage = new float[4, 4];
            storage[0, 0] = 1f;
            storage[0, 1] = xy;
            storage[0, 2] = xz;
            storage[1, 0] = yx;
            storage[1, 1] = 1f;
            storage[1, 2] = yz;
            storage[2, 0] = zx;
            storage[2, 1] = zy;
            storage[2, 2] = 1f;
            storage[3, 3] = 1f;
            return new Matrix(storage);
        }

        #region IEquatable
        public bool Equals(Matrix other) => Equals(other, Epsilon);
        public bool Equals(Matrix other, float threshold){
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    if (!this[i, j].Equal(other[i, j], threshold)) return false;
                }
            }
            return true;
        }
        public override bool Equals(object obj) => obj is Matrix other && Equals(other);
        public override readonly int GetHashCode() => _storage.GetHashCode();
        #endregion

        #region Operators
        public static Matrix operator *(Matrix left, Matrix right) => Matrix4x4.Multiply(left, right);
        public static Matrix operator *(Matrix m, float scalar) => Matrix4x4.Multiply(m, scalar);
        public static Vector operator *(Matrix m, Vector v) => m.Multiply(v);

        public static bool operator ==(Matrix left, Matrix right) => left.Equals(right);
        public static bool operator !=(Matrix left, Matrix right) => !left.Equals(right);
        #endregion

        /// <inheritdoc/>
        public override string ToString()
        {
            var value = "[";
            for (var i = 0; i < Rows; i++)
            {
                value += "\n[";
                for (var j = 0; j < Cols; j++) value += " " + this[i, j] + " ";
                value += "]";
            }
            value += "]";
            return value;
        }
    }
}
