using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Shapes;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests.Shapes
{
    [TestClass]
    public class Cylinders
    {
        [TestMethod]
        public void RayMissesCylinder()
        {
            var c = new Cylinder();
            var rays = new Ray[]
            {
                new(1, 0, 0, 0, 1, 0),
                new(0, 0, 0, 0, 1, 0),
                new(0, 0, -5, 1, 1, 1)
            };
            foreach (var r in rays)
            {
                var h = r;
                var xs = c.IntersectLocal(ref h);
                Assert.AreEqual(xs.Count, 0);
            }
        }
        [TestMethod]
        public void RayHitsCylinder()
        {
            var c = new Cylinder();
            var rays = new[]
            {
                (new Ray(1, 0, -5, 0, 0, 1), 5, 5),
                (new Ray(0, 0, -5, 0, 0, 1), 4, 6),
                (new Ray(0.5f, 0, -5, 0.1f, 1, 1), 6.80798f, 7.08872f)
            };
            foreach (var (r, x1, x2) in rays)
            {
                var h = new Ray(r.Origin, Vector4.Normalize(r.Direction));
                var xs = c.IntersectLocal(ref h);
                Assert.AreEqual(xs.Count, 2);
                Assert.That.FloatsAreEqual(x1, xs[0].Distance, 1E-4f);
                Assert.That.FloatsAreEqual(x2, xs[1].Distance, 1e-4f);
            }
        }

        [TestMethod]
        public void NormalsAreCorrect()
        {
            var c = new Cylinder();
            var points = new[]
            {
                (Point(1, 0, 0), Direction(1, 0, 0)),
                (Point(0, 5, -1), Direction(0, 0, -1)),
                (Point(0, -2, 1), Direction(0, 0, 1)),
                (Point(-1, 1, 0), Direction(-1, 0, 0))
            };
            foreach (var (point, normal) in points)
            {
                Assert.AreEqual(c.LocalNormal(point), normal);
            }
        }

        [TestMethod]
        public void ConstrainedCylinder()
        {
            var c = new Cylinder { Maximum = 2, Minimum = 1 };
            var rays = new Ray[]
            {
                new(0, 1.5f, 0, 0.1f, 1, 0),
                new(0, 3, -5, 0, 0, 1),
                new(0, 0, -5, 0, 0, 1),
                new(0, 2, -5, 0, 0, 1),
                new(0, 1, -5, 0, 0, 1),
                new(0, 1.5f, -2, 0, 0, 1)
            };
            foreach (var r in rays)
            {
                var h = r;
                var xs = c.IntersectLocal(ref h);
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                Assert.AreEqual(xs.Count, r.Origin.Z != -2 ? 0 : 2);
            }
        }

        [TestMethod]
        public void CylinderIsNotClosed()
        {
            var c = new Cylinder();
            Assert.IsFalse(c.IsClosed);
        }

        [TestMethod]
        public void IntersectingCylinderCaps()
        {
            var c = new Cylinder { Minimum = 1, Maximum = 2, IsClosed = true };
            var rays = new Ray[]
            {
                new(0, 3, 0, 0, -1, 0),
                new(0, 3, -2, 0, -1, 2),
                new(0, 4, -2, 0, -1, 1),
                new(0, 0, -2, 0, 1, 2),
                new(0, -1, -2, 0, 1, 1)
            }; foreach (var r in rays)
            {
                var h = new Ray(r.Origin, Vector4.Normalize(r.Direction));
                var xs = c.IntersectLocal(ref h);
                Assert.AreEqual(xs.Count, 2);
            }
        }

        [TestMethod]
        public void CylinderCapNormals()
        {
            var c = new Cylinder()
            {
                Minimum = 1,
                Maximum = 2,
                IsClosed = true
            };

            var points = new[]
            {
                (Point(0,1,0), Direction(0, -1, 0)),
                (Point(0.5f,1,0), Direction(0, -1, 0)),
                (Point(0,1,0.5f), Direction(0, -1, 0)),
                (Point(0,2,0), Direction(0, 1, 0)),
                (Point(0.5f,2,0), Direction(0, 1, 0)),
                (Point(0,2,0.5f), Direction(0, 1, 0)),
            };
            foreach (var (point, normal) in points)
            {
                Assert.AreEqual(c.LocalNormal(point), normal);
            }
        }
    }
}
