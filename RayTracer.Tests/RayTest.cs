using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using RayTracer.Primitives;
using System.Linq;

namespace RayTracer.Tests
{
    [TestClass]
    public class RayTest
    {
        [TestMethod]
        public void CreateRay()
        {
            var ray = new Ray(Vector3.Point(1f, 2f, 3f), Vector3.Vector(4f, 5f, 6f));
            Assert.AreEqual(Vector3.Point(1f, 2f, 3f), ray.Origin);
            Assert.AreEqual(Vector3.Vector(4f, 5f, 6f), ray.Direction);
        }

        [TestMethod]
        public void PointByDistance()
        {
            var ray = new Ray(Vector3.Point(2f, 3f, 4f), Vector3.Vector(1f, 0f, 0f));
            Assert.AreEqual(Vector3.Point(2f, 3f, 4f), ray.PointByDistance(0f));
            Assert.AreEqual(Vector3.Point(3f, 3f, 4f), ray.PointByDistance(1f));
            Assert.AreEqual(Vector3.Point(1f, 3f, 4f), ray.PointByDistance(-1f));
            Assert.AreEqual(Vector3.Point(4.5f, 3f, 4f), ray.PointByDistance(2.5f));
        }

        [TestMethod]
        public void RayIntersectsSphere()
        {
            var ray = new Ray(Vector3.Point(0f, 0f, -5f), Vector3.Vector(1f, 0f, 0f));
            var s = new Sphere(Vector3.Point(0f, 0f, 0f));
            var xs = s.Intersect(ray);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0], 4f);
            Assert.AreEqual(xs[1], 6f);
        }

        [TestMethod]
        public void RayIntersectsSphereTangent()
        {
            var ray = new Ray(Vector3.Point(0f, 0f, -5f), Vector3.Vector(0f, 0f, 1f));
            var s = new Sphere(Vector3.Point(0f, 0f, 0f));
            var xs = s.Intersect(ray);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0], 5f);
            Assert.AreEqual(xs[1], 5f);
        }

        [TestMethod]
        public void RayMissesSphere()
        {
            var ray = new Ray(Vector3.Point(0f, 2f, -5f), Vector3.Vector(0f, 0f, 1f));
            var s = new Sphere(Vector3.Point(0f, 0f, 0f));
            var xs = s.Intersect(ray);
            Assert.AreEqual(xs.Length, 0);
        }

        [TestMethod]
        public void RayInsideSphere()
        {
            var ray = new Ray(Vector3.Point(0f, 0f, 0f), Vector3.Vector(0f, 0f, 1f));
            var s = new Sphere(Vector3.Point(0f, 0f, 0f));
            var xs = s.Intersect(ray);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0], -1f);
            Assert.AreEqual(xs[1], 1f);
        }
        [TestMethod]
        public void SphereBehindRay()
        {
            var ray = new Ray(Vector3.Point(0f, 0f, 5f), Vector3.Vector(0f, 0f, 1f));
            var s = new Sphere(Vector3.Point(0f, 0f, 0f));
            var xs = s.Intersect(ray);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0], -6f);
            Assert.AreEqual(xs[1], -4f);
        }
    }
}
