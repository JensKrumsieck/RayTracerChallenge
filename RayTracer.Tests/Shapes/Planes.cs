using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static RayTracer.Extension.VectorExtension;
using Plane = RayTracer.Shapes.Plane;

namespace RayTracer.Tests.Shapes
{
    [TestClass]
    public class Planes
    {
        [TestMethod]
        public void PlaneNormalIsUpVector()
        {
            var p = new Plane();
            var n1 = p.LocalNormal(Point(0f, 0f, 0f));
            var n2 = p.LocalNormal(Point(202f, 110f, 0f));
            var n3 = p.LocalNormal(Point(-202f, 1e10f, 99f));
            Assert.That.VectorsAreEqual(n1, Vector4.UnitY);
            Assert.That.VectorsAreEqual(n3, Vector4.UnitY);
            Assert.That.VectorsAreEqual(n2, Vector4.UnitY);
        }

        [TestMethod]
        public void IntersectRayParallel()
        {
            var p = new Plane();
            var r = new Ray(0f, 10f, 0f, 0f, 0f, 1f);
            var xs = p.IntersectLocal(r);
            Assert.AreEqual(xs.Length, 0);
        }

        [TestMethod]
        public void IntersectCoplanarRay()
        {
            var p = new Plane();
            var r = new Ray(0f, 0f, 0f, 0f, 0f, 1f);
            var xs = p.IntersectLocal(r);
            Assert.AreEqual(xs.Length, 0);
        }

        [TestMethod]
        public void IntersectPlaneFromAbove()
        {
            var p = new Plane();
            var r = new Ray(0f, 1f, 0f, 0f, -1f, 0f);
            var xs = p.IntersectLocal(r);
            Assert.AreEqual(xs.Length,1);
            Assert.AreEqual(xs[0].Distance, 1f);
            Assert.AreEqual(xs[0].Object, p);
        }

        [TestMethod]
        public void IntersectPlaneFromBelow()
        {
            var p = new Plane();
            var r = new Ray(0f, -1f, 0f, 0f, 1f, 0f);
            var xs = p.IntersectLocal(r);
            Assert.AreEqual(xs.Length, 1);
            Assert.AreEqual(xs[0].Distance, 1f);
            Assert.AreEqual(xs[0].Object, p);
        }
    }
}
