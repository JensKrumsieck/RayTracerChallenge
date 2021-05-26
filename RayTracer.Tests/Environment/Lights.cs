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
                var intensity = l.IntensityAt(p.Item1, w);
                Assert.AreEqual(intensity, p.Item2);
            }
        }

        [TestMethod]
        public void CreateAreaLight()
        {
            var l = new AreaLight(PointZero, Direction(2, 0, 0), 4, Direction(0, 0, 1), 2, Color.White);
            Assert.AreEqual(l.Corner, PointZero);
            Assert.AreEqual(l.UVec, Vector4.UnitX * .5f);
            Assert.AreEqual(l.USteps, 4);
            Assert.AreEqual(l.VVec, Vector4.UnitZ * .5f);
            Assert.AreEqual(l.VSteps, 2);
            Assert.AreEqual(l.Samples, 8);
            Assert.AreEqual(l.Position, Direction(1, 0, .5f));
        }

        [TestMethod]
        public void SinglePointOnAreaLight()
        {
            var c = PointZero;
            var v1 = Vector4.UnitX * 2f;
            var v2 = Vector4.UnitZ;
            var l = new AreaLight(c, v1, 4, v2, 2, Color.White);
            var pts = new[]
            {
                (0, 0, Point(.25f, 0, .25f)),
                (1, 0, Point(.75f, 0, .25f)),
                (0, 1, Point(.25f, 0, .75f)),
                (2, 0, Point(1.25f, 0, .25f)),
                (3, 1, Point(1.75f, 0, .75f)),
            };
            foreach (var (u, v, p) in pts)
            {
                var pt = l.PointAt(u, v);
                Assert.AreEqual(pt, p);
            }
        }

        [TestMethod]
        public void AreaLightIntensity()
        {
            var w = World.Default;
            var corner = Point(-.5f, -.5f, -5);
            var v1 = Vector4.UnitX;
            var v2 = Vector4.UnitY;
            var l = new AreaLight(corner, v1, 2, v2, 2, Color.White);
            w.Lights[0] = l;
            var pts = new[]
            {
                (Point(0, 0, 2), 0f),
                (Point(1, -1, 2), .25f),
                (Point(1.5f, 0, 2), .5f),
                (Point(1.25f, 1.25f, 3), .75f),
                (Point(0, 0, -2), 1f)
            };
            foreach (var (p, i) in pts)
            {
                var intensity = l.IntensityAt(p, w);
                Assert.AreEqual(intensity, i);
            }
        }
    }
}
