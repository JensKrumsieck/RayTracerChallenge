using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using System;

namespace RayTracer.Tests
{
    [TestClass]
    public class VectorTest
    {
        [TestMethod]
        public void VectorIsPoint()
        {
            var a = new Vector(4.3f, -4.2f, 3.1f, 1.0f);
            Assert.IsTrue(a.IsPoint);
            Assert.IsFalse(a.IsVector);
        }

        [TestMethod]
        public void VectorIsVector()
        {
            var a = new Vector(4.3f, -4.2f, 3.1f);
            Assert.IsFalse(a.IsPoint);
            Assert.IsTrue(a.IsVector);
        }

        [TestMethod]
        public void CreatePoint()
        {
            var a = Vector.Point(4f, -4f, 3f);
            Assert.IsTrue(a.IsPoint);
            Assert.AreEqual(a, new Vector(4f, -4f, 3f, 1f));
        }

        /// <summary>
        /// Of Course is that true with my implementation...
        /// </summary>
        [TestMethod]
        public void CreateVector()
        {
            var a = new Vector(4f, -4f, 3f);
            Assert.IsTrue(a.IsVector);
            Assert.AreEqual(a, new Vector(4f, -4f, 3f));
        }

        [TestMethod]
        public void AdditionTest()
        {
            var a = Vector.Point(3f, -2f, 5f);
            var b = new Vector(-2f, 3f, 1f);
            var ab = a + b;
            var res = Vector.Point(1f, 1f, 6f);
            Assert.IsTrue(ab.IsPoint);
            Assert.AreEqual(ab, res);
        }

        [TestMethod]
        public void SubtractionTest()
        {
            var a = Vector.Point(3f, 2f, 1f);
            var b = Vector.Point(5f, 6f, 7f);
            var ab = a - b;
            var res = new Vector(-2f, -4f, -6f);
            Assert.IsTrue(ab.IsVector);
            Assert.AreEqual(ab, res);
        }

        [TestMethod]
        public void SubtractionTest_2()
        {
            var a = Vector.Point(3f, 2f, 1f);
            var b = new Vector(5f, 6f, 7f);
            var ab = a - b;
            var res = Vector.Point(-2f, -4f, -6f);
            Assert.IsFalse(ab.IsVector);
            Assert.AreEqual(ab, res);
        }

        [TestMethod]
        public void SubtractionTest_3()
        {
            var a = new Vector(3f, 2f, 1f);
            var b = new Vector(5f, 6f, 7f);
            var ab = a - b;
            var res = new Vector(-2f, -4f, -6f);
            Assert.IsTrue(ab.IsVector);
            Assert.AreEqual(ab, res);
        }

        [TestMethod]
        public void NegationTest()
        {
            var a = new Vector(0f, 0f, 0f);
            var b = new Vector(1f, -2f, -3f);
            var neg = a - b;

            var res = -b;

            Assert.AreEqual(neg, res);
        }

        [TestMethod]
        public void ScalarMultiply()
        {
            var a = new Vector(1, -2, 3, -4);
            Assert.AreEqual(a * 3.5f, new Vector(3.5f, -7f, 10.5f, -14f));
            Assert.AreEqual(3.5f * a, a * 3.5f);
        }

        [TestMethod]
        public void ScalarDivide()
        {
            var a = new Vector(1, -2, 3, -4);
            Assert.AreEqual(a / 2, a * .5f);
        }

        [TestMethod]
        public void Magnitude()
        {
            var v1 = new Vector(1f, 0f, 0f);
            Assert.AreEqual(v1.Magnitude, 1);
            var v2 = new Vector(0f, 1f, 0f);
            Assert.AreEqual(v2.Magnitude, 1);
            var v3 = new Vector(0f, 0f, 1f);
            Assert.AreEqual(v3.Magnitude, 1);
            var v4 = new Vector(1f, -2f, 3f);
            Assert.AreEqual(v4.Magnitude, MathF.Sqrt(14));
        }

        [TestMethod]
        public void Normalization()
        {
            var v = new Vector(9879, 213, 30928);
            var w = v.Normalized;
            Assert.IsTrue(((Vector)w).IsNormalized);
        }

        [TestMethod]
        public void DotProduct()
        {
            var a = new Vector(1f, 2f, 3f);
            var b = new Vector(2f, 3f, 4f);
            Assert.AreEqual(Vector.Dot(a, b), 20);
        }

        [TestMethod]
        public void CrossProduct()
        {
            var a = new Vector(1f, 2f, 3f);
            var b = new Vector(2f, 3f, 4f);
            Assert.AreEqual(a.Cross(b), new Vector(-1, 2, -1));
            Assert.AreEqual(b.Cross(a), new Vector(1, -2, 1));
        }
    }
}