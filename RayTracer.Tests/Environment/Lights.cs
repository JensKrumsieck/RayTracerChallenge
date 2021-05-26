using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Environment;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests.Environment
{
    [TestClass]
    public class Lights
    {
        [TestMethod]
        public void LightHasColorPosition()
        {
            var l = new PointLight(Point(0f, 0f, 0f), Color.White);
            Assert.AreEqual(l.Intensity, Color.White);
            Assert.AreEqual(l.Position, Point(0f, 0f, 0f));
        }

        [TestMethod]
        public void PointLightsEvaluateAtGivenPoint()
        {
            var w = World.Default;
            var l = w.Lights[0];
            var pts = new (Vector4, float)[]
            {
                (Point(0, 1.0001f, 0), 1f),
                (Point(-1.0001f, 0, 0), 1f),
                (Point(0, 0, -1.0001f), 1f),
                (Point(0, 0, 1.0001f), 0f),
                (Point(0, -1.0001f, 0), 0f),
                (Point(1.0001f, 0, 0), 0f),
                (Point(0, 0, 0), 0f)
            };
            foreach (var p in pts)
            {
                var intensity = w.IntensityAt(l, p.Item1);
                Assert.AreEqual(intensity, p.Item2);
            }
        }
    }
}
