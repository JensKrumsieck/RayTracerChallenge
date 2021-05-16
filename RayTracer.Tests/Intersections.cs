using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Shapes;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests
{
    [TestClass]
    public class Intersections
    {
        [TestMethod]
        public void IntersectionHasObjectAndDistance()
        {
            var s = new Sphere();
            var i = new Intersection(3.5f, s);
            Assert.AreEqual(i.Distance, 3.5f);
            Assert.AreEqual(i.Object, s);
        }

        [TestMethod]
        public void AggregatingIntersections()
        {
            var s = new Sphere();
            var i1 = new Intersection(1f, s);
            var i2 = new Intersection(2f, s);
            var xs = new[] { i1, i2 };
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0], i1);
            Assert.AreEqual(xs[1], i2);
        }

        [TestMethod]
        public void HitWhenAllPositive()
        {
            var s = new Sphere();
            var i1 = new Intersection(1, s);
            var i2 = new Intersection(2, s);
            var xs = new[] { i1, i2 };
            var i = Intersection.Hit(xs);
            Assert.AreEqual(i, i1);
        }

        [TestMethod]
        public void HitWhenNegative()
        {
            var s = new Sphere();
            var i1 = new Intersection(-1, s);
            var i2 = new Intersection(1, s);
            var xs = new[] { i1, i2 };
            var i = Intersection.Hit(xs);
            Assert.AreEqual(i, i2);
        }

        [TestMethod]
        public void HitWhenAllNegative()
        {
            var s = new Sphere();
            var i1 = new Intersection(-1, s);
            var i2 = new Intersection(-2, s);
            var xs = new[] { i1, i2 };
            var i = Intersection.Hit(xs);
            Assert.AreEqual(i, null);
        }

        [TestMethod]
        public void HitLowestNonNegative()
        {
            var s = new Sphere();
            var i1 = new Intersection(5, s);
            var i2 = new Intersection(7, s);
            var i3 = new Intersection(-3, s);
            var i4 = new Intersection(2, s);
            var xs = new[] { i1, i2, i3, i4 };
            var i = Intersection.Hit(xs);
            Assert.AreEqual(i, i4);
        }

        [TestMethod]
        public void PrecomputingStateOfIntersection()
        {
            var r = new Ray(0f, 0f, -5f, 0f, 0f, 1f);
            var s = new Sphere();
            var i = new Intersection(4f, s);
            var comps = IntersectionState.Prepare(i, r);
            Assert.AreEqual(comps.Distance, i.Distance);
            Assert.AreEqual(comps.Object, i.Object);
            Assert.That.VectorsAreEqual(comps.Point, Point(0f, 0f, -1f));
            Assert.That.VectorsAreEqual(comps.Eye, Direction(0f, 0f, -1f));
            Assert.That.VectorsAreEqual(comps.Normal, Direction(0f, 0f, -1f));
        }

        [TestMethod]
        public void HitOccursOutside()
        {
            var r = new Ray(0f, 0f, -5f, 0f, 0f, 1f);
            var s = new Sphere();
            var i = new Intersection(4f, s);
            var comps = IntersectionState.Prepare(i, r);
            Assert.IsFalse(comps.IsInside);
        }

        [TestMethod]
        public void HitOccursInside()
        {
            var r = new Ray(0f, 0f, 0f, 0f, 0f, 1f);
            var s = new Sphere();
            var i = new Intersection(1f, s);
            var comps = IntersectionState.Prepare(i, r);
            Assert.IsTrue(comps.IsInside);
            Assert.That.VectorsAreEqual(comps.Point, Point(0f, 0f, 1f));
            Assert.That.VectorsAreEqual(comps.Eye, Direction(0f, 0f, -1f));
            Assert.That.VectorsAreEqual(comps.Normal, Direction(0f, 0f, -1f));
        }
    }
}
