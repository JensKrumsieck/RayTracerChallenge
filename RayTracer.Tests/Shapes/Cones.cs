using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Shapes;
using System;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests.Shapes
{
    [TestClass]
    public class Cones
    {
        [TestMethod]
        public void IntersectingACone()
        {
            var c = new Cone();
            var rays = new[]
            {
                (new Ray(0, 0, -5, 0, 0, 1), 5, 5),
                (new Ray(0, 0, -4.999999f, 1, 1, 1), 8.66025f, 8.66025f), //-4.999999f = 5f FLOATING POINT ERROR!
                (new Ray(1f, 1, -5, -0.5f, -1, 1), 4.55006f, 49.44994f)
            };
            foreach (var (r, x1, x2) in rays)
            {
                var h = new Ray(r.Origin, Vector4.Normalize(r.Direction));
                var xs = c.IntersectLocal(ref h);
                Assert.AreEqual(xs.Count, 2);
                Assert.That.FloatsAreEqual(x1, xs[0].Distance, 1e-4f);
                Assert.That.FloatsAreEqual(x2, xs[1].Distance, 1e-4f);
            }
        }

        [TestMethod]
        public void IntersectionRayParalleltoOneHalf()
        {
            var c = new Cone();
            var dir = Vector4.Normalize(Direction(0, 1, 1));
            var r = new Ray(Point(0, 0, -1), dir);
            var xs = c.IntersectLocal(ref r);
            Assert.AreEqual(xs.Count, 1);
            Assert.That.FloatsAreEqual(xs[0].Distance, .35355f);
        }

        [TestMethod]
        public void IntersectConeOnCap()
        {
            var c = new Cone
            {
                Minimum = -.5f,
                Maximum = .5f,
                IsClosed = true
            };
            var rays = new[]
            {
                (new Ray(0, 0, -5, 0, 1, 0), 0),
                (new Ray(0, 0, -.25f, 0, 1, 1), 2),
                (new Ray(0, 0, -.25f, 0, 1, 0), 4)
            };
            foreach (var (r, count) in rays)
            {
                var h = new Ray(r.Origin, Vector4.Normalize(r.Direction));
                var xs = c.IntersectLocal(ref h);
                Assert.AreEqual(xs.Count, count);
            }
        }

        [TestMethod]
        public void NormalOnCone()
        {
            var c = new Cone();
            var points = new[]
            {
                (Point(0, 0, 0), Direction(0, 0, 0)),
                (Point(1, 1, 1), Direction(1, -MathF.Sqrt(2f), 1)),
                (Point(-1, -1, 0), Direction(-1, 1, 0))
            };
            foreach (var (point, normal) in points)
            {
                Assert.AreEqual(c.LocalNormal(point), normal);
            }
        }

        [TestMethod]
        public void BoundingBox()
        {
            var s = new Cone();
            var b = s.BoundingBox;
            Assert.AreEqual(b.Min, Point(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity));
            Assert.AreEqual(b.Max, Point(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity));
        }

        [TestMethod]
        public void BoundingBoxConstrained()
        {
            var s = new Cone { Minimum = -5, Maximum = 3 };
            var b = s.BoundingBox;
            Assert.AreEqual(b.Min, Point(-5, -5, -5));
            Assert.AreEqual(b.Max, Point(5, 3, 5));
        }
    }

}
