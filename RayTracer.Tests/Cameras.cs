using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Environment;
using System;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests
{
    [TestClass]
    public class Cameras
    {
        [TestMethod]
        public void Constructor()
        {
            var c = new Camera(160, 120, MathF.PI / 2f);
            Assert.AreEqual(c.Resolution.X, 160);
            Assert.AreEqual(c.Resolution.Y, 120);
            Assert.That.FloatsAreEqual(c.FieldOfView, MathF.PI / 2f);
        }

        [TestMethod]
        public void PixelSize()
        {
            var c1 = new Camera(200, 125, MathF.PI / 2f);
            var c2 = new Camera(125, 200, MathF.PI / 2f);
            Assert.AreEqual(c1.PixelSize, .01f);
            Assert.AreEqual(c2.PixelSize, .01f);
        }

        [TestMethod]
        public void RayThroughCenter()
        {
            var c = new Camera(201, 101, MathF.PI / 2f);
            var r = c.RayTo(100, 50);
            Assert.That.VectorsAreEqual(r.Origin, Point(0f, 0f, 0f));
            Assert.That.VectorsAreEqual(r.Direction, Direction(0f, 0f, -1f));
        }

        [TestMethod]
        public void RayThroughCorner()
        {
            var c = new Camera(201, 101, MathF.PI / 2f);
            var r = c.RayTo(0, 0);
            Assert.That.VectorsAreEqual(r.Origin, Point(0f, 0f, 0f));
            Assert.That.VectorsAreEqual(r.Direction, Direction(.66519f, .33259f, -.66851f));
        }

        [TestMethod]
        public void RayWithTransformedCam()
        {
            var c = new Camera(201, 101, MathF.PI / 2f)
            {
                Transform = RotationY(MathF.PI / 4f) * Translation(0f, -2f, 5f)
            };
            var r = c.RayTo(100, 50);
            Assert.That.VectorsAreEqual(r.Origin, Point(0f, 2f, -5f));
            Assert.That.VectorsAreEqual(r.Direction, Direction(MathF.Sqrt(2f) / 2f, 0f, -MathF.Sqrt(2f) / 2f));
        }

        [TestMethod]
        public void RenderWorldWithCam()
        {
            var w = World.Default;
            var c = new Camera(11, 11, MathF.PI / 2f);
            var from = Point(0f, 0f, -5f);
            var to = Point(0f, 0f, 0f);
            var up = Direction(0f, 1f, 0f);
            c.Transform = Camera.ViewTransform(from, to, up);
            var image = c.Render(w);
            Assert.That.VectorsAreEqual(image[5, 5], new Color(.38066f, .47583f, .2855f));
        }
    }
}
