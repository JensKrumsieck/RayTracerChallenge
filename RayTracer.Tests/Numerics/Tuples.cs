using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Extension;
using System;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;
using static System.Numerics.Vector4;

namespace RayTracer.Tests.Numerics
{
    [TestClass]
    public class Tuples
    {
        [TestMethod]
        public void TupleIsPoint()
        {
            var a = new Vector4(4.3f, -4.2f, 3.1f, 1.0f);
            Assert.That.FloatsAreEqual(a.X, 4.3f);
            Assert.That.FloatsAreEqual(a.Y, -4.2f);
            Assert.That.FloatsAreEqual(a.Z, 3.1f);
            Assert.IsTrue(a.IsPoint());
            Assert.IsFalse(a.IsVector());
        }

        [TestMethod]
        public void TupleIsVector()
        {
            var a = new Vector4(4.3f, -4.2f, 3.1f, 0.0f);
            Assert.That.FloatsAreEqual(a.X, 4.3f);
            Assert.That.FloatsAreEqual(a.Y, -4.2f);
            Assert.That.FloatsAreEqual(a.Z, 3.1f);
            Assert.IsFalse(a.IsPoint());
            Assert.IsTrue(a.IsVector());
        }

        [TestMethod]
        public void PointShorthand()
        {
            var p = Point(4f, -4f, 3f);
            Assert.That.VectorsAreEqual(p, new Vector4(4f, -4f, 3f, 1f));
        }

        [TestMethod]
        public void DirectionShorthand()
        {
            var p = Direction(4f, -4f, 3f);
            Assert.That.VectorsAreEqual(p, new Vector4(4f, -4f, 3f, 0f));
        }

        [TestMethod]
        public void Addition()
        {
            var a = new Vector4(3f, -2f, 5f, 1f);
            var b = new Vector4(-2f, 3f, 1f, 0f);
            Assert.That.VectorsAreEqual(a + b, Point(1f, 1f, 6f));
        }

        [TestMethod]
        public void SubtractingPoints()
        {
            var p1 = Point(3f, 2f, 1f);
            var p2 = Point(5f, 6f, 7f);
            Assert.That.VectorsAreEqual(p1 - p2, Direction(-2f, -4f, -6f));
        }

        [TestMethod]
        public void SubtractingVectorFromPoint()
        {
            var p = Point(3f, 2f, 1f);
            var v = Direction(5f, 6f, 7f);
            Assert.That.VectorsAreEqual(p - v, Point(-2f, -4f, -6f));
        }

        [TestMethod]
        public void SubtractingVectors()
        {
            var v1 = Direction(3f, 2f, 1f);
            var v2 = Direction(5f, 6f, 7f);
            Assert.That.VectorsAreEqual(v1 - v2, Direction(-2f, -4f, -6f));
        }

        [TestMethod]
        public void NegatingVector()
        {
            var v = Direction(1f, -2f, 3f);
            Assert.That.VectorsAreEqual(Vector4.Zero - v, Direction(-1f, 2f, -3f));
        }

        [TestMethod]
        public void NegatingTuple()
        {
            var v = new Vector4(1f, -2f, 3f, -4f);
            Assert.That.VectorsAreEqual(-v, new Vector4(-1f, 2f, -3f, 4f));
        }

        [TestMethod]
        public void MultiplyScalar()
        {
            var a = new Vector4(1f, -2f, 3f, -4f);
            Assert.That.VectorsAreEqual(a * 3.5f, new Vector4(3.5f, -7f, 10.5f, -14f));
        }

        [TestMethod]
        public void MultiplyFraction()
        {
            var a = new Vector4(1f, -2f, 3f, -4f);
            Assert.That.VectorsAreEqual(a * .5f, new Vector4(.5f, -1f, 1.5f, -2f));
        }

        [TestMethod]
        public void ScalarDivision()
        {
            var a = new Vector4(1f, -2f, 3f, -4f);
            Assert.That.VectorsAreEqual(a / 2f, new Vector4(.5f, -1f, 1.5f, -2f));
        }

        [TestMethod]
        public void Magnitude()
        {
            var a = UnitX;
            Assert.That.FloatsAreEqual(a.Length(), 1);

            var b = UnitY;
            Assert.That.FloatsAreEqual(b.Length(), 1);

            var c = UnitZ;
            Assert.That.FloatsAreEqual(c.Length(), 1);

            var d = Direction(1f, 2f, 3f);
            Assert.That.FloatsAreEqual(d.LengthSquared(), 14);
            Assert.That.FloatsAreEqual(d.Length(), MathF.Sqrt(14));

            var e = Direction(-1f, -2f, -3f);
            Assert.That.FloatsAreEqual(e.LengthSquared(), 14);
            Assert.That.FloatsAreEqual(e.Length(), MathF.Sqrt(14));
        }

        [TestMethod]
        public void Normalizing()
        {
            var v1 = Direction(4f, 0f, 0f);
            Assert.AreEqual(Normalize(v1), Direction(1f, 0f, 0f));

            var v2 = Direction(1f, 2f, 3f);
            var s4 = MathF.Sqrt(14);
            Assert.That.VectorsAreEqual(Normalize(v2), Direction(1f / s4, 2f / s4, 3f / s4));
        }

        [TestMethod]
        public void DotProduct()
        {
            var a = Direction(1f, 2f, 3f);
            var b = Direction(2f, 3f, 4f);
            Assert.That.FloatsAreEqual(Dot(a, b), 20f);
        }

        [TestMethod]
        public void CrossProduct()
        {
            var a = Direction(1f, 2f, 3f);
            var b = Direction(2f, 3f, 4f);
            Assert.That.VectorsAreEqual(a.Cross(b), Direction(-1f, 2f, -1f));
            Assert.That.VectorsAreEqual(b.Cross(a), Direction(1f, -2f, 1f));
        }

        [TestMethod]
        public void ColorsAreTuples()
        {
            var c = new Color(-.5f, .4f, 1.7f);
            Assert.That.FloatsAreEqual(c.R, -.5f);
            Assert.That.FloatsAreEqual(c.G, .4f);
            Assert.That.FloatsAreEqual(c.B, 1.7f);
        }

        [TestMethod]
        public void ColorsAddition()
        {
            var c1 = new Color(.9f, .6f, .75f);
            var c2 = new Color(.7f, .1f, .25f);
            Assert.That.VectorsAreEqual(c1 + c2, new Color(1.6f, .7f, 1.0f));
        }

        [TestMethod]
        public void ColorsSubtraction()
        {
            var c1 = new Color(.9f, .6f, .75f);
            var c2 = new Color(.7f, .1f, .25f);
            Assert.That.VectorsAreEqual(c1 - c2, new Color(.2f, .5f, .5f));
        }

        [TestMethod]
        public void ColorMultiplyScalar()
        {
            var c1 = new Color(.2f, .3f, .4f);
            Assert.That.VectorsAreEqual(c1 * 2, new Color(.4f, .6f, .8f));
        }

        [TestMethod]
        public void ColorMultiplication()
        {
            var c1 = new Color(1f, .2f, .4f);
            var c2 = new Color(.9f, 1f, .1f);
            Assert.That.VectorsAreEqual(c1 * c2, new Color(.9f, .2f, .04f));
        }

        [TestMethod]
        public void ReflectingVector45()
        {
            var v = Direction(1f, -1f, 0f);
            var n = Direction(0f, 1f, 0f);
            var r = v.Reflect(n);
            Assert.That.VectorsAreEqual(r, Direction(1f,1f,0f));
        }

        [TestMethod]
        public void ReflectingSlanted()
        {
            var v = Direction(0f, -1f, 0f);
            var val = MathF.Sqrt(2f) / 2f;
            var n = Direction(val,val ,0f);
            var r = v.Reflect(n);
            Assert.That.VectorsAreEqual(r, Direction(1f, 0f, 0f));
        }
    }
}
