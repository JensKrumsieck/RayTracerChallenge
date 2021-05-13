using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using System.Numerics;

namespace RayTracer.Tests
{
    [TestClass]
    public class RayTest
    {
        [TestMethod]
        public void CreateRay()
        {
            var ray = new Ray(new Vector3(1f, 2f, 3f), new Vector3(4f, 5f, 6f));
            Assert.AreEqual(new Vector3(1f, 2f, 3f), ray.Origin);
            Assert.AreEqual(new Vector3(4f, 5f, 6f), ray.Direction);
        }

        [TestMethod]
        public void PointByDistance()
        {
            var ray = new Ray(new Vector3(2f, 3f, 4f), new Vector3(1f, 0f, 0f));
            Assert.AreEqual(new Vector3(2f, 3f, 4f), ray.PointByDistance(0f));
            Assert.AreEqual(new Vector3(3f, 3f, 4f), ray.PointByDistance(1f));
            Assert.AreEqual(new Vector3(1f, 3f, 4f), ray.PointByDistance(-1f));
            Assert.AreEqual(new Vector3(4.5f, 3f, 4f), ray.PointByDistance(2.5f));
        }
    }
}
