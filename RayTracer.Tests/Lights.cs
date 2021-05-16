using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Lights;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests
{
    [TestClass]
    public class Lights
    {
        [TestMethod]
        public void LightHasColorPosition()
        {
            var l = new PointLight(Point(0f, 0f, 0f), Color.White);
            Assert.AreEqual(l.Intensity, Color.White);
            Assert.AreEqual(l.Position, Point(0f,0f,0f));
        }
    }
}
