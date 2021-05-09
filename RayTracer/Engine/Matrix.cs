using System;
using System.Linq;

namespace RayTracer.Engine
{
    /// <summary>
    /// Will probably use System.Numerics instead...
    /// </summary>
    public struct Matrix : IEquatable<Matrix>
    {
        private readonly float[,] _storage;
        public readonly float this[int row, int col] => _storage[row, col];
        public Matrix(float[,] array) => _storage = array;

        public static Matrix Identity(int rows, int cols)
        {
            var storage = new float[rows, cols];
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    storage[i, j] = i == j ? 1f : 0f;
                }
            }
            return new Matrix(storage);
        }

        public int Rows => _storage.GetLength(0);
        public int Cols => _storage.GetLength(1);

        /// <summary>
        /// Calculates transpose matrix
        /// </summary>
        /// <returns></returns>
        public Matrix Transpose()
        {
            var storage = new float[Cols, Rows];
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++) storage[j, i] = this[i, j];
            }
            return new Matrix(storage);
        }

        /// <summary>
        /// Creates submatrix with given column and row removed
        /// </summary>
        /// <param name="removeRow"></param>
        /// <param name="removeCol"></param>
        /// <returns></returns>
        public Matrix Submatrix(int removeRow, int removeCol)
        {
            var newStorage = new float[Cols - 1, Rows - 1];
            for (var i = 0; i < Rows - 1; i++)
            {
                for (var j = 0; j < Cols - 1; j++)
                    newStorage[i, j] = this[i >= removeRow ? i + 1 : i, j >= removeCol ? j + 1 : j];
            }
            return new Matrix(newStorage);
        }

        /// <summary>
        /// Returns Determinant for a matrix
        /// </summary>
        /// <returns></returns>
        public float Determinant()
        {
            if (Rows != Cols) throw new NotSupportedException("Determinant only exists in square matrices");
            if (Rows == 2 && Cols == 2) return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            var det = 0f;
            for (var i = 0; i < Cols; i++) det += this[0, i] * Cofactor(0, i);
            return det;
        }

        /// <summary>
        /// Calculates submatrix determinant
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public float Minor(int row, int col) => Submatrix(row, col).Determinant();

        /// <summary>
        /// Calculates cofactor of given row and column
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public float Cofactor(int row, int col) => Minor(row, col) * ((row + col) % 2 == 1 ? -1 : 1);

        #region IEquatable
        public bool Equals(Matrix other) => _storage.Cast<float>().SequenceEqual(other._storage.Cast<float>());
        public override bool Equals(object obj) => obj is Matrix other && Equals(other);
        public override readonly int GetHashCode() => _storage != null ? _storage.GetHashCode() : 0;
        #endregion

        #region operators
        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.Cols != right.Rows)
                throw new InvalidOperationException(
                    $"Can not multiply matrices [{left.Rows}x{left.Cols}] and [{right.Rows}x{right.Cols}]");
            var newStorage = new float[left.Rows, right.Cols];
            for (var i = 0; i < left.Rows; i++)
            {
                for (var j = 0; j < right.Cols; j++)
                {
                    for (var k = 0; k < left.Cols; k++)
                        newStorage[i, j] += left[i, k] * right[k, j];
                }
            }
            return new Matrix(newStorage);
        }

        public static Vector3 operator *(Matrix m, Vector3 v)
        {
            if (m.Rows != 4 || m.Cols != 4) throw new NotSupportedException();
            var vec = new Matrix(new[,]
            {
                {v.X},
                {v.Y},
                {v.Z},
                {v.W}
            });
            var res = m * vec;
            return Vector3.OfArray(res._storage.Cast<float>().ToArray());
        }

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
