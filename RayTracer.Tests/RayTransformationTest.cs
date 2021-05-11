using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;

namespace RayTracer.Tests
{
    [TestClass]
    public class RayTransformationTest
    {
        [TestMethod]
        public void TranslateRay()
        {
            var r = new Ray(Vector3.Point(1f, 2f, 3f), Vector3.UnitY);
            var r2 = r.Transform(Matrix.Translation(3f, 4f, 5f));
            Assert.AreEqual(r2.Direction, Vector3.UnitY);
            Assert.AreEqual(r2.Origin, Vector3.Point(4f,6f,8f));
        }

        [TestMethod]
        public void ScaleRay()
        {
            var r = new Ray(Vector3.Point(1f, 2f, 3f), Vector3.UnitY);
            var r2 = r.Transform(Matrix.Scale(2f, 3f, 4f));
            Assert.AreEqual(r2.Direction, Vector3.UnitY * 3f);
            Assert.AreEqual(r2.Origin, Vector3.Point(2f, 6f, 12f));
        }
    }
}
