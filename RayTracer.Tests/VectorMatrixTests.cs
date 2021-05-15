using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Extension;
using System;
using System.Numerics;

namespace RayTracer.Tests
{
    [TestClass]
    public class VectorMatrixTests
    {
        [TestMethod]
        public void Vector4Multiply()
        {
            var v = new Vector4(1f, 2f, 3f, 1f);
            var m = new Matrix4x4(
                1f, 2f, 3f, 4f,
                2f, 4f, 4f, 2f,
                8f, 6f, 4f, 1f,
                0f, 0f, 0f, 1f);
            Assert.AreEqual(v.Multiply(m), new Vector4(18f, 24f, 33f, 1f));
        }

        [TestMethod]
        public void Vector3Multiply()
        {
            var v = new Vector3(1f, 2f, 3f);
            var m = new Matrix4x4(
                1f, 2f, 3f, 4f,
                2f, 4f, 4f, 2f,
                8f, 6f, 4f, 1f,
                0f, 0f, 0f, 1f);
            Assert.AreEqual(v.Multiply(m), new Vector3(18f, 24f, 33f));
        }

        [TestMethod]
        public void Magnitude()
        {
            Assert.AreEqual(1f, Vector3.UnitX.LengthSquared());
            Assert.AreEqual(1f, Vector3.UnitY.LengthSquared());
            Assert.AreEqual(1f, Vector3.UnitZ.LengthSquared());
            Assert.AreEqual(MathF.Sqrt(14), new Vector3(1f, 2f, 3f).Length());
            Assert.AreEqual(MathF.Sqrt(14), new Vector3(-1f, -2f, -3f).Length());
        }

        [TestMethod]
        public void Dot()
        {
            Assert.AreEqual(20f, Vector3.Dot(new Vector3(1f, 2f, 3f), new Vector3(2f, 3f, 4f)));
        }

        [TestMethod]
        public void Cross()
        {
            Assert.AreEqual(new Vector3(-1f, 2f, -1f), Vector3.Cross(new Vector3(1f, 2f, 3f), new Vector3(2f, 3f, 4f)));
            Assert.AreEqual(new Vector3(1f, -2f, 1f), Vector3.Cross(new Vector3(2f, 3f, 4f), new Vector3(1f, 2f, 3f)));
        }

        [TestMethod]
        public void Reflect()
        {
            var v1 = new Vector3(1, -1, 0);
            var n = new Vector3(0, 1, 0);
            var r = Vector3.Reflect(v1, n);
            Assert.AreEqual(r, new Vector3(1, 1, 0));

            var v2 = new Vector3(0, -1, 0);
            var n2 = new Vector3(MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f, 0f);
            var r2 = Vector3.Reflect(v2, n2);
            Assert.That.VectorsAreEqual(r2, new Vector3(1, 0, 0), 1e-5f);
        }

        [TestMethod]
        public void Transpose()
        {
            var m1 = new Matrix4x4(
                0f, 9f, 3f, 0f,
                9f, 8f, 0f, 8f,
                1f, 8f, 5f, 3f,
                0f, 0f, 5f, 8f);
            var m2 = new Matrix4x4(
                0f, 9f, 1f, 0f,
                9f, 8f, 8f, 0f,
                3f, 0f, 5f, 5f,
                0f, 8f, 3f, 8f);
            Assert.That.MatricesAreEqual(m1.Transpose(), m2);
            Assert.AreEqual(Matrix4x4.Transpose(m1), m2);
        }

        [TestMethod]
        public void Determinant()
        {
            var m = new Matrix4x4(
                -2f, -8f, 3f, 5f,
                -3f, 1f, 7f, 3f,
                1f, 2f, -9f, 6f,
                -6f, 7f, 7f, -9f);
            Assert.AreEqual(-4071, m.GetDeterminant());
        }

        [TestMethod]
        public void Invert()
        {
            var m = new Matrix4x4(
                -5f, 2f, 6f, -8f,
                1f, -5f, 1f, 8f,
                7f, 7f, -6f, -7f,
                1f, -3f, 7f, 4f);
            Assert.That.MatricesAreEqual(m.Invert(), new Matrix4x4(
                .21805f, .45113f, .24060f, -.04511f,
                -.80827f, -1.45677f, -.44361f, .52068f,
                -.07895f, -.22368f, -.05263f, .19737f,
                -.52256f, -.81391f, -.30075f, .30639f), 1e-5f);

            var m2 = new Matrix4x4(
                8f, -5f, 9f, 2f,
                7f, 5f, 6f, 1f,
                -6f, 0f, 9f, 6f,
                -3f, 0f, -9f, -4f);
            Assert.That.MatricesAreEqual(m2.Invert(), new Matrix4x4(
                -.15385f, -.15385f, -.28205f, -.53846f,
                -.07692f, .12308f, .02564f, .03077f,
                .35897f, .35897f, .43590f, .92308f,
                -.69231f, -.69231f, -.76923f, -1.92308f), 1e-5f);
        }
    }
}
