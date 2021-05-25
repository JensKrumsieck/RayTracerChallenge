using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Shapes;
using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests.Shapes
{
    [TestClass]
    public class Triangles
    {
        private static readonly Triangle DefaultTriangle = new(Point(0, 1, 0), Point(-1, 0, 0), Point(1, 0, 0));

        private static readonly Triangle DefaultSmoothTriangle =
            new(Point(0, 1, 0), Point(-1, 0, 0), Point(1, 0, 0), Direction(0, 1, 0), Direction(-1, 0, 0), Direction(1,
                0, 0));

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

        [TestMethod]
        public void SmoothTriangle()
        {
            var t = DefaultSmoothTriangle;
            Assert.AreEqual(t.V1, Point(0, 1, 0));
            Assert.AreEqual(t.V2, Point(-1, 0, 0));
            Assert.AreEqual(t.V3, Point(1, 0, 0));
            Assert.AreEqual(t.N1, Direction(0, 1, 0));
            Assert.AreEqual(t.N2, Direction(-1, 0, 0));
            Assert.AreEqual(t.N3, Direction(1, 0, 0));
        }

        [TestMethod]
        public void IntersectionCanEncapsulateUV()
        {
            var s = DefaultTriangle;
            var i = new Intersection(3.5f, s, .2f, .4f);
            Assert.AreEqual(i.U, .2f);
            Assert.AreEqual(i.V, .4f);
        }

        [TestMethod]
        public void SmoothTriangleSavesUV()
        {
            var t = DefaultSmoothTriangle;
            var r = new Ray(-.2f, .3f, -2, 0, 0, 1);
            var xs = t.IntersectLocal(ref r);
            Assert.AreEqual(xs[0].U, .45f);
            Assert.AreEqual(xs[0].V, .25f);
        }

        [TestMethod]
        public void SmoothTriUsesInterpolationNormal()
        {
            var t = DefaultSmoothTriangle;
            var i = new Intersection(1, t, .45f, .25f);
            var n = t.Normal(PointZero, i);
            Assert.That.VectorsAreEqual(n, Direction(-.5547f, .83205f, 0), 1e-4f);
        }

        [TestMethod]
        public void PrepareNormalOnSmooth()
        {
            var t = DefaultSmoothTriangle;
            var i = new Intersection(1, t, .45f, .25f);
            var r = new Ray(-.2f, .3f, -2, 0, 0, 1);
            var xs = new List<Intersection> { i };
            var comps = IntersectionState.Prepare(ref i, ref r, xs);
            Assert.That.VectorsAreEqual(comps.Normal, Direction(-.5547f, .83205f, 0));
        }
    }
}
