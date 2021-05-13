using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using RayTracer.Primitives;
using System.Numerics;

namespace RayTracer.Tests
{
    [TestClass]
    public class WorldTest
    {
        [TestMethod]
        public void DefaultWorldIntersect()
        {
            var w = World.Default;
            var ray = new Ray(new Vector3(0f, 0f, -5f), Vector3.UnitZ);
            var xs = w.Intersections(ray);
            Assert.AreEqual(xs.Length, 4);
            Assert.AreEqual(xs[0].Distance, 4f);
            Assert.AreEqual(xs[1].Distance, 4.5f);
            Assert.AreEqual(xs[2].Distance, 5.5f);
            Assert.AreEqual(xs[3].Distance, 6f);
        }

        [TestMethod]
        public void Precomputation()
        {
            var ray = new Ray(new Vector3(0f, 0f, -5f), Vector3.UnitZ);
            var s = new Sphere();
            var i = new HitInfo(4f, s);
            var c = IntersectionPoint.Prepare(i, ray);
            Assert.AreEqual(c.HitPoint, -Vector3.UnitZ);
            Assert.AreEqual(c.Eye, -Vector3.UnitZ);
            Assert.AreEqual(c.Normal, -Vector3.UnitZ);
        }

        [TestMethod]
        public void PrecomputationNotInside()
        {
            var ray = new Ray(new Vector3(0f, 0f, -5f), Vector3.UnitZ);
            var s = new Sphere();
            var i = new HitInfo(4f, s);
            var c = IntersectionPoint.Prepare(i, ray);
            Assert.IsFalse(c.IsInside);
        }

        [TestMethod]
        public void PrecomputationInside()
        {
            var ray = new Ray(new Vector3(0f, 0f, 0f), Vector3.UnitZ);
            var s = new Sphere();
            var i = new HitInfo(1f, s);
            var c = IntersectionPoint.Prepare(i, ray);
            Assert.AreEqual(c.HitPoint, Vector3.UnitZ);
            Assert.AreEqual(c.Eye, -Vector3.UnitZ);
            Assert.IsTrue(c.IsInside);
            Assert.AreEqual(c.Normal, -Vector3.UnitZ);
        }

        [TestMethod]
        public void IntersectionShading()
        {
            var w = World.Default;
            var s = w.Objects[0];
            var ray = new Ray(new Vector3(0f, 0f, -5f), Vector3.UnitZ);
            var i = new HitInfo(4f, s);
            var c = IntersectionPoint.Prepare(i, ray);
            var col = w.Shade(c);
            Assert.That.ColorsAreEqual(col, new Color(.38066f, .47583f, .2855f), 1e-5f);
        }

        [TestMethod]
        public void IntersectionShadingInside()
        {
            var w = World.Default;
            w.Lights[0].Position = new Vector3(0f, .25f, 0f);
            var s = w.Objects[1];
            var ray = new Ray(new Vector3(0f, 0f, 0f), Vector3.UnitZ);
            var i = new HitInfo(.5f, s);
            var c = IntersectionPoint.Prepare(i, ray);
            var col = w.Shade(c);
            Assert.That.ColorsAreEqual(col, new Color(.90498f, .90498f, .90498f), 1e-5f);
        }
    }
}
