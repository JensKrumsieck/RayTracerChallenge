using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using RayTracer.Primitives;
using System.Numerics;

namespace RayTracer.Tests
{
    [TestClass]
    public class IntersectionTest
    {

        [TestMethod]
        public void RayIntersectsSphere()
        {
            var ray = new Ray(new Vector3(0f, 0f, -5f), Vector3.UnitZ);
            var s = new Sphere();
            var xs = s.Intersect(ray);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, 4f);
            Assert.AreEqual(xs[1].Distance, 6f);
        }

        [TestMethod]
        public void RayIntersectsSphereOtherSyntax()
        {
            var s = new Sphere();
            var rayOrigin = new Vector3(0f, 0f, -5f);
            var isHit = Ray.Intersect(rayOrigin, Vector3.UnitZ, s, out var hits);
            Assert.IsTrue(isHit);
            Assert.AreEqual(hits[0].Distance, 4f);
            Assert.AreEqual(hits[1].Distance, 6f);
            Assert.AreEqual(hits[0].HitObject, s);
        }

        [TestMethod]
        public void RayIntersectsSphereTangent()
        {
            var ray = new Ray(new Vector3(0f, 1f, -5f), Vector3.UnitZ);
            var s = new Sphere();
            var xs = s.Intersect(ray);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, 5f);
            Assert.AreEqual(xs[1].Distance, 5f);
        }

        [TestMethod]
        public void RayMissesSphere()
        {
            var ray = new Ray(new Vector3(0f, 2f, -5f), Vector3.UnitZ);
            var s = new Sphere();
            var xs = s.Intersect(ray);
            Assert.AreEqual(xs.Length, 0);
        }

        [TestMethod]
        public void RayInsideSphere()
        {
            var ray = new Ray(new Vector3(0f, 0f, 0f), Vector3.UnitZ);
            var s = new Sphere();
            var xs = s.Intersect(ray);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, -1f);
            Assert.AreEqual(xs[1].Distance, 1f);
        }

        [TestMethod]
        public void SphereBehindRay()
        {
            var ray = new Ray(new Vector3(0f, 0f, 5f), Vector3.UnitZ);
            var s = new Sphere();
            var xs = s.Intersect(ray);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, -6f);
            Assert.AreEqual(xs[1].Distance, -4f);
        }

        [TestMethod]
        public void TrackingIntersections()
        {
            var s = new Sphere();
            var hit = new HitInfo(3.5f, s);
            Assert.AreEqual(hit.Distance, 3.5f);
            Assert.AreEqual(s, hit.HitObject);
        }

        [TestMethod]
        public void AggregateIntersections()
        {
            var s = new Sphere();
            var hit1 = new HitInfo(1f, s);
            var hit2 = new HitInfo(2f, s);
            var intersections = new[] { hit1, hit2 };
            Assert.AreEqual(intersections.Length, 2);
            Assert.AreEqual(intersections[0].HitObject, s);
            Assert.AreEqual(intersections[1].HitObject, s);
        }
    }
}
