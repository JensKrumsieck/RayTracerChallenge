using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine.Camera;
using System;
using System.Numerics;
using RayTracer.Engine;

namespace RayTracer.Tests
{
    [TestClass]
    public class CameraTest
    {
        [TestMethod]
        public void DefaultTransform()
        {
            var from = Vector3.Zero;
            var to = -Vector3.UnitZ;
            var up = Vector3.UnitY;
            var t = Camera.ViewTransform(from, to, up);
            Assert.AreEqual(t, Matrix4x4.Identity);
        }

        [TestMethod]
        public void LookingPositiveZ()
        {
            var from = Vector3.Zero;
            var to = Vector3.UnitZ;
            var up = Vector3.UnitY;
            var t = Camera.ViewTransform(from, to, up);
            Assert.AreEqual(t, Matrix4x4.CreateScale(-1f, 1f, -1f));
        }

        [TestMethod]
        public void MovingWorld()
        {
            var from = Vector3.UnitZ * 8f;
            var to = Vector3.Zero;
            var up = Vector3.UnitY;
            var t = Camera.ViewTransform(from, to, up);
            Assert.AreEqual(t, Matrix4x4.Transpose(Matrix4x4.CreateTranslation(0f, 0f, -8f)));
        }

        [TestMethod]
        public void ArbitraryView()
        {
            var from = new Vector3(1f, 3f, 2f);
            var to = new Vector3(4f,-2f,8f);
            var up = new Vector3(1f, 1f, 0f);
            var t = Camera.ViewTransform(from, to, up);
            Assert.That.MatricesAreEqual(t,
                new Matrix4x4(
                    -.50709f, .50709f, .67612f, -2.36643f, 
                    .76772f, .60609f, .12122f, -2.82843f, 
                    -.35857f, .59761f, -.71714f, 0f, 
                    0f, 0f, 0f, 1f), 1e-4f);
        }

        [TestMethod]
        public void CameraConstructor()
        {
            var c = new Camera(160, 120, MathF.PI / 2f);
            Assert.AreEqual(c.Resolution.X, 160);
            Assert.AreEqual(c.Resolution.Y, 120);
            Assert.AreEqual(c.FieldOfView, MathF.PI / 2f);
            Assert.AreEqual(c.TransformationMatrix, Matrix4x4.Identity);
        }

        [TestMethod]
        public void PixelSize()
        {
            var c = new Camera(200, 125, MathF.PI / 2f);
            Assert.AreEqual(c.PixelSize, .01f);
        }

        [TestMethod]
        public void RayThroughCanvasCenter()
        {
            var c = new Camera(201, 101, MathF.PI / 2f);
            var r = c.RayTo(100, 50);
            Assert.AreEqual(r.Origin, Vector3.Zero);
            Assert.That.VectorsAreEqual(r.Direction, -Vector3.UnitZ, 1e-5f);
        }

        [TestMethod]
        public void RayThroughCanvasCorner()
        {
            var c = new Camera(201, 101, MathF.PI / 2f);
            var r = c.RayTo(0, 0);
            Assert.AreEqual(r.Origin, Vector3.Zero);
            Assert.That.VectorsAreEqual(r.Direction, new Vector3(.66519f, .33259f, -.66851f), 1e-5f);
        }

        [TestMethod]
        public void RayWithTransformedCamera()
        {
            var c = new Camera(201, 101, MathF.PI / 2f)
            {
                TransformationMatrix = Matrix4x4.Multiply(Matrix4x4.CreateTranslation(0f, -2f, 5f), Matrix4x4.CreateRotationY(MathF.PI / 4))
            };
            var r = c.RayTo(100, 50);
            Assert.That.VectorsAreEqual(r.Origin, new Vector3(0f,2f,-5f), 1e-5f);
            Assert.That.VectorsAreEqual(r.Direction, new Vector3(MathF.Sqrt(2f)/2f,0f, -MathF.Sqrt(2f) / 2f), 1e-5f);
        }

        [TestMethod]
        public void RenderWithCamera()
        {
            var w = World.Default;
            var c = new Camera(11, 11, MathF.PI / 2f);
            var from = new Vector3(0f, 0f, -5f);
            var to = Vector3.Zero;
            var up = Vector3.UnitY;
            c.TransformationMatrix = Camera.ViewTransform(from, to, up);
            var image = c.Render(w);
            Assert.That.ColorsAreEqual(image.PixelAt(5,5), new Color(.38066f, .47583f, .2855f), 1e-5f);
        }
    }
}
