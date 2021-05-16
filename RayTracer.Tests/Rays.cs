using Microsoft.VisualStudio.TestTools.UnitTesting;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests
{
    [TestClass]
    public class Rays
    {
        [TestMethod]
        public void CreateRay()
        {
            var r = new Ray(1f, 2f, 3, 4f, 5f, 6f);
            Assert.AreEqual(r.Origin, Point(1f, 2f, 3f));
            Assert.AreEqual(r.Direction, Direction(4f, 5f, 6f));
        }

        [TestMethod]
        public void RayDistance()
        {
            var r = new Ray(Point(2f, 3f, 4f), Direction(1f, 0f, 0f));
            Assert.AreEqual(r.PointByDistance(0f), Point(2f, 3f, 4f));
            Assert.AreEqual(r.PointByDistance(1f), Point(3f, 3f, 4f));
            Assert.AreEqual(r.PointByDistance(-1f), Point(1f, 3f, 4f));
            Assert.AreEqual(r.PointByDistance(2.5f), Point(4.5f, 3f, 4f));
        }

        [TestMethod]
        public void TranslateRay()
        {
            var r = new Ray(Point(1f, 2f, 3f), Direction(0f, 1f, 0f));
            var m = Translation(3f, 4f, 5f);
            var r2 = r.Transform(m);
            Assert.That.VectorsAreEqual(r2.Origin, Point(4f, 6f, 8f));
            Assert.That.VectorsAreEqual(r2.Direction, r.Direction);
        }

        [TestMethod]
        public void ScaleRay()
        {
            var r = new Ray(Point(1f, 2f, 3f), Direction(0f, 1f, 0f));
            var m = Scale(2f, 3f, 4f);
            var r2 = r.Transform(m);
            Assert.That.VectorsAreEqual(r2.Origin, Point(2f, 6f, 12f));
            Assert.That.VectorsAreEqual(r2.Direction, Direction(0f, 3f, 0f));
        }
    }
}
