using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Extension;
using System.Numerics;
using static System.Numerics.Matrix4x4;

namespace RayTracer.Tests.Numerics
{
    [TestClass]
    public class Matrices
    {
        [TestMethod]
        public void Constructor()
        {
            var m = new Matrix4x4(
                1f, 2f, 3f, 4f,
                5.5f, 6.5f, 7.5f, 8.5f,
                9f, 10f, 11f, 12f,
                13.5f, 14.5f, 15.5f, 16.5f);
            Assert.That.FloatsAreEqual(m.M11, 1);
            Assert.That.FloatsAreEqual(m.M14, 4);
            Assert.That.FloatsAreEqual(m.M21, 5.5f);
            Assert.That.FloatsAreEqual(m.M23, 7.5f);
            Assert.That.FloatsAreEqual(m.M33, 11f);
            Assert.That.FloatsAreEqual(m.M43, 15.5f);
        }

        //SKIP MATRIX 3x3 AND 2x2 TESTS

        [TestMethod]
        public void MatrixEquality()
        {
            var a = new Matrix4x4(
                1f, 2f, 3f, 4f,
                5f, 6f, 7f, 8f,
                9f, 8f, 7f, 6f,
                5f, 4f, 3f, 2f);
            var b = new Matrix4x4(
                1f, 2f, 3f, 4f,
                5f, 6f, 7f, 8f,
                9f, 8f, 7f, 6f,
                5f, 4f, 3f, 2f);
            Assert.That.MatricesAreEqual(a, b);
        }

        [TestMethod]
        public void MatrixInEquality()
        {
            var a = new Matrix4x4(
                1f, 2f, 3f, 4f,
                5f, 6f, 7f, 8f,
                9f, 8f, 7f, 6f,
                5f, 4f, 3f, 2f);
            var b = new Matrix4x4(
                2f, 3f, 4f, 5f,
                6f, 7f, 8f, 9f,
                8f, 7f, 6f, 5f,
                4f, 3f, 2f, 1f);
            Assert.IsFalse(a.Equals(b));
        }

        [TestMethod]
        public void MatrixMultiply()
        {
            var a = new Matrix4x4(
                1f, 2f, 3f, 4f,
                5f, 6f, 7f, 8f,
                9f, 8f, 7f, 6f,
                5f, 4f, 3f, 2f);
            var b = new Matrix4x4(
                -2f, 1f, 2f, 3f,
                3f, 2f, 1f, -1f,
                4f, 3f, 6f, 5f,
                1f, 2f, 7f, 8f);
            var res = new Matrix4x4(
                20f, 22f, 50f, 48f,
                44f, 54f, 114f, 108f,
                40f, 58f, 110f, 102f,
                16f, 26f, 46f, 42f);
            Assert.That.MatricesAreEqual(a * b, res);
        }

        [TestMethod]
        public void MatrixMultiplyVector()
        {
            var a = new Matrix4x4(
                1f, 2f, 3f, 4f,
                2f, 4f, 4f, 2f,
                8f, 6f, 4f, 1f,
                0f, 0f, 0f, 1f);
            var v = new Vector4(1f, 2f, 3f, 1f);
            Assert.That.VectorsAreEqual(a.Multiply(v), new Vector4(18f, 24f, 33f, 1f));
        }

        [TestMethod]
        public void MultiplyIdentity()
        {
            var a = new Matrix4x4(
                0f, 1f, 2f, 4f,
                1f, 2f, 4f, 8f,
                2f, 3f, 8f, 16f,
                4f, 8f, 16f, 32f);
            Assert.That.MatricesAreEqual(a * Identity, a);
        }

        [TestMethod]
        public void Transposing()
        {
            var a = new Matrix4x4(
                0f, 9f, 3f, 0f,
                9f, 8f, 0f, 8f,
                1f, 8f, 5f, 3f,
                0f, 0f, 5f, 8f);
            var aT = new Matrix4x4(
                0f, 9f, 1f, 0f,
                9f, 8f, 8f, 0f,
                3f, 0f, 5f, 5f,
                0f, 8f, 3f, 8f);
            Assert.That.MatricesAreEqual(Transpose(a), aT);
        }

        [TestMethod]
        public void TransposingIdentity()
        {
            Assert.That.MatricesAreEqual(Transpose(Identity), Identity);
        }

        //SKIP MATRIX DETERMINANT/MINOR/SUBMATRIX TESTS FOR SMALL MATRICES

        [TestMethod]
        public void Determinant()
        {
            var m = new Matrix4x4(
                -2f, -8f, 3f, 5f,
                -3f, 1f, 7f, 3f,
                1f, 2f, -9f, 6f,
                -6f, 7f, 7f, -9f);
            Assert.That.FloatsAreEqual(m.GetDeterminant(), -4071);
        }

        [TestMethod]
        public void Invertible()
        {
            var m = new Matrix4x4(
                6f, 4f, 4f, 4f,
                5f, 5f, 7f, 6f,
                4f, -9f, 3f, -7f,
                9f, 1f, 7f, -6f);
            Assert.That.FloatsAreEqual(m.GetDeterminant(), -2120);
            Assert.IsTrue(m.IsInvertible());

            var b = new Matrix4x4(
                -4f, 2f, -2f, -3f,
                9f, 6f, 2f, 6f,
                0f, -5f, 1f, -5f,
                0f, 0f, 0f, 0f);
            Assert.IsFalse(b.IsInvertible());
            Assert.That.FloatsAreEqual(b.GetDeterminant(), 0f);
        }

        [TestMethod]
        public void InverseMatrix()
        {
            var m = new Matrix4x4(
                -5f, 2f, 6f, -8f,
                1f, -5f, 1f, 8f,
                7f, 7f, -6f, -7f,
                1f, -3f, 7f, 4f);
            var v = new Matrix4x4(
                .21805f, .45113f, .24060f, -.04511f,
                -.80827f, -1.45677f, -.44361f, .52068f,
                -.07895f, -.22368f, -.05263f, .19737f,
                -.52256f, -.81391f, -.30075f, .30639f);
            Assert.That.MatricesAreEqual(m.Inverse(), v);

            var m1 = new Matrix4x4(
                8f, -5f, 9f, 2f,
                7f, 5f, 6f, 1f,
                -6f, 0f, 9f, 6f,
                -3f, 0f, -9f, -4f);
            var v1 = new Matrix4x4(
                -.15385f, -.15385f, -.28205f, -.53846f,
                -.07692f, .12308f, .02564f, .03077f,
                .35897f, .35897f, .43590f, .92308f,
                -.69231f, -.69231f, -.76923f, -1.92308f);
            Assert.That.MatricesAreEqual(m1.Inverse(), v1);

            var m2 = new Matrix4x4(
                9f, 3f, 0f, 9f,
                -5f, -2f, -6f, -3f,
                -4f, 9f, 6f, 4f,
                -7f, 6f, 6f, 2f);
            var v2 = new Matrix4x4(
                -.04074f, -.07778f, .14444f, -.22222f,
                -.07778f, .03333f, .36667f, -.33333f,
                -.02901f, -.14630f, -.10926f, .12963f,
                .17778f, .06667f, -.26667f, .33333f);
            Assert.That.MatricesAreEqual(m2.Inverse(), v2);
        }

        [TestMethod]
        public void MultiplyByInverse()
        {
            var m = new Matrix4x4(
                3f, -9f, 7f, 3f,
                3f, -8f, 2f, -9f,
                -4f, 4f, 4f, 1f,
                -6f, 5f, -1f, 1f);
            var i = new Matrix4x4(
                8f, 2f, 2f, 2f,
                3f, -1f, 7f, 0f,
                7f, 0f, 5f, 4f,
                6f, -2f, 0f, 5f);
            var c = m * i;
            Assert.That.MatricesAreEqual(c * i.Inverse(), m);
        }
    }
}
