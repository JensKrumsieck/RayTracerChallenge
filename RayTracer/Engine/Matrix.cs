using RayTracer.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using static RayTracer.Constants;

namespace RayTracer.Engine
{
    /// <summary>
    /// Will probably use System.Numerics or MathNet.Numerics instead...
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
                for (var j = 0; j < cols; j++) storage[i, j] = i == j ? 1f : 0f;
            }
            return new Matrix(storage);
        }

        public int Rows => _storage.GetLength(0);
        public int Cols => _storage.GetLength(1);

        public bool Invertible => Determinant() != 0;

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
        /// Returns Inverse Matrix
        /// </summary>
        /// <returns></returns>
        public Matrix Inverse()
        {
            if (!Invertible) throw new InvalidOperationException("Matrix is not invertible!");
            var det = Determinant();
            var newStorage = new float[Rows, Cols];
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    var c = Cofactor(i, j);
                    newStorage[j, i] = c / det;
                }
            }
            return new Matrix(newStorage);
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

        /// <summary>
        /// Appends a row to a matrix
        /// </summary>
        /// <param name="rowValues"></param>
        /// <returns></returns>
        private Matrix AppendRow(IReadOnlyList<float> rowValues)
        {
            var storage = new float[Rows + 1, Cols];
            for (var i = 0; i < Rows + 1; i++)
            {
                for (var j = 0; j < Cols; j++)
                    storage[i, j] = i == Rows ? rowValues[j] : this[i, j];
            }
            return new Matrix(storage);
        }

        /// <summary>
        /// Appends a column to a matrix
        /// </summary>
        /// <param name="colValues"></param>
        /// <returns></returns>
        private Matrix AppendCol(IReadOnlyList<float> colValues)
        {
            var storage = new float[Rows , Cols + 1];
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols + 1; j++)
                    storage[i, j] = j == Cols ? colValues[i] : this[i, j];
            }

            return new Matrix(storage);
        }

        /// <summary>
        /// Builds Translation Matrix
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Matrix Translation(float x, float y, float z) => Identity(3, 3).AppendRow(new[] {0f, 0f, 0f}).AppendCol(new []{x, y, z, 1});

        /// <summary>
        /// Builds Scaling Matrix
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Matrix Scale(float x, float y, float z)
        {
            var storage = new float[4, 4];
            storage[0, 0] = x;
            storage[1, 1] = y;
            storage[2, 2] = z;
            storage[3, 3] = 1;
            return new Matrix(storage);
        }


        #region IEquatable
        public bool Equals(Matrix other) => Equals(other, Epsilon);
        public bool Equals(Matrix other, float floatThreshold)
        {
            if (Cols != other.Cols || Rows != other.Rows) return false;
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    if (!this[i, j].Equal(other[i, j], floatThreshold)) return false;
                }
            }
            return true;
        }
        public override bool Equals(object obj) => obj is Matrix other && Equals(other);
        public override readonly int GetHashCode()
        {
            return (_storage != null ? _storage.GetHashCode() : 0);
        }

        #endregion

        #region Operators
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

        public static Matrix operator *(Matrix m, float scalar)
        {
            var newStorage = new float[m.Rows, m.Cols];
            for (var i = 0; i < m.Rows; i++)
            {
                for (var j = 0; j < m.Cols; j++) newStorage[i, j] = m[i, j] * scalar;
            }
            return new Matrix(newStorage);
        }

        public static Vector3 operator *(Matrix m, Vector3 v)
        {
            if (m.Rows != 4 || m.Cols != 4) throw new NotSupportedException("Only 4x4 Matrices are supported");
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
