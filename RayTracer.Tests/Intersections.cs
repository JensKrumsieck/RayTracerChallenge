using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Shapes;

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
    }
}
