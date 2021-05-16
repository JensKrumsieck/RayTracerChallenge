using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Shapes;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests
{
    [TestClass]
    public class Spheres
    {
        [TestMethod]
        public void RayIntersectsSphereAtTwoPoints()
        {
            var r = new Ray(0f, 0f, -5f, 0f, 0f, 1f);
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, 4.0f);
            Assert.AreEqual(xs[1].Distance, 6.0f);
        }

        [TestMethod]
        public void RayIntersectsSphereAtTangent()
        {
            var r = new Ray(0f, 1f, -5f, 0f, 0f, 1f);
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, 5.0f);
            Assert.AreEqual(xs[1].Distance, 5.0f);
        }

        [TestMethod]
        public void RayMissesSphere()
        {
            var r = new Ray(0f, 2f, -5f, 0f, 0f, 1f);
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 0);
        }

        [TestMethod]
        public void RayInsideSphere()
        {
            var r = new Ray(0f, 0f, 0f, 0f, 0f, 1f);
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, -1.0f);
            Assert.AreEqual(xs[1].Distance, 1.0f);
        }

        [TestMethod]
        public void RayWithSphereBehind()
        {
            var r = new Ray(0f, 0f, 5f, 0f, 0f, 1f);
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, -6.0f);
            Assert.AreEqual(xs[1].Distance, -4.0f);
        }

        [TestMethod]
        public void IntersectionCheckObject()
        {
            var r = new Ray(0f, 0f, -5f, 0f, 0f, 1f);
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Object, s);
            Assert.AreEqual(xs[1].Object, s);
        }

        [TestMethod]
        public void DefaultTransform()
        {
            var s = new Sphere();
            Assert.AreEqual(s.Transform, Transform.Identity);
        }

        [TestMethod]
        public void ChangingTransform()
        {
            var s = new Sphere(Translation(2f, 3f, 4f));
            Assert.AreEqual(s.Transform.Matrix, Translation(2f, 3f, 4f));
        }

        [TestMethod]
        public void IntersectScaledSphere()
        {
            var r = new Ray(Point(0f, 0f, -5f), Direction(0f, 0f, 1f));
            var s = new Sphere(Scale(2f, 2f, 2f));
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, 3f);
            Assert.AreEqual(xs[1].Distance, 7f);
        }

        [TestMethod]
        public void IntersectTranslatedSphere()
        {
            var r = new Ray(Point(0f, 0f, -5f), Direction(0f, 0f, 1f));
            var s = new Sphere(Translation(5f, 0f, 0f));
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 0);
        }
    }
}
