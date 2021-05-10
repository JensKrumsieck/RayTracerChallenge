using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;

namespace RayTracer.Tests
{
    [TestClass]
    public class RayTest
    {
        [TestMethod]
        public void CreateRay()
        {
            var ray = new Ray(Vector3.Point(1, 2, 3), Vector3.Vector(4, 5, 6));
            Assert.AreEqual(Vector3.Point(1, 2, 3), ray.Origin);
            Assert.AreEqual(Vector3.Vector(4, 5, 6), ray.Direction);
        }
    }
}
