using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Shapes;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests.Shapes
{
    [TestClass]
    public class Triangles
    {
        public static Triangle DefaultTriangle = new Triangle(Point(0, 1, 0), Point(-1, 0, 0), Point(1, 0, 0));
        [TestMethod]
        public void FindNormal()
        {
            var t = DefaultTriangle;
            var n1 = t.LocalNormal(Point(0, .5f, 0));
            var n2 = t.LocalNormal(Point(-.5f, .75f, 0));
            var n3 = t.LocalNormal(Point(.5f, .25f, 0));
            Assert.AreEqual(n1, n2);
            Assert.AreEqual(n1, n3);
            Assert.AreEqual(n1, t.Normal(Vector4.Zero));
        }

        [TestMethod]
        public void RayParallelTri()
        {
            var t = DefaultTriangle;
            var r = new Ray(0, -1, -2, 0, 1, 0);
            var xs = t.IntersectLocal(ref r);
            Assert.AreEqual(xs.Count, 0);
        }

        [TestMethod]
        public void RayMissesP1P3()
        {
            var t = DefaultTriangle;
            var r = new Ray(1, 1, -2, 0, 0, 1);
            var xs = t.IntersectLocal(ref r);
            Assert.AreEqual(xs.Count, 0);
        }

        [TestMethod]
        public void RayMissesP1P2()
        {
            var t = DefaultTriangle;
            var r = new Ray(-1, 1, -2, 0, 0, 1);
            var xs = t.IntersectLocal(ref r);
            Assert.AreEqual(xs.Count, 0);
        }

        [TestMethod]
        public void RayMissesP2P3()
        {
            var t = DefaultTriangle;
            var r = new Ray(0, -1, -2, 0, 0, 1);
            var xs = t.IntersectLocal(ref r);
            Assert.AreEqual(xs.Count, 0);
        }

        [TestMethod]
        public void RayHitsTriangle()
        {
            var t = DefaultTriangle;
            var r = new Ray(0, .5f, -2, 0, 0, 1);
            var xs = t.IntersectLocal(ref r);
            Assert.AreEqual(xs.Count, 1);
            Assert.AreEqual(xs[0].Distance, 2);
        }

        [TestMethod]
        public void TriangleHasBounds()
        {
            var p1 = Point(-3, 7, 2);
            var p2 = Point(6, 2, -4);
            var p3 = Point(2, -1, -1);
            var s = new Triangle(p1, p2, p3);
            var b = s.BoundingBox;
            Assert.AreEqual(b.Min, Point(-3, -1, -4));
            Assert.AreEqual(b.Max, Point(6, 7, 2));
        }
    }
}
