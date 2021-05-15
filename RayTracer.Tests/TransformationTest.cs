using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using System;
using System.Numerics;
using RayTracer.Extension;

namespace RayTracer.Tests
{
    [TestClass]
    public class TransformationTest
    {
        [TestMethod]
        public void MultiplyByTranslation()
        {
            var p = new Vector3(-3f, 4f, 5f);
            var res = new Vector3(2f, 1f, 7f);
            Assert.AreEqual(p.Translate(5f, -3f, 2f), res);
        }

        [TestMethod]
        public void TranslationNotAffectingVector4()
        {
            var v = new Vector4(-3f, 4f, 5f, 0f);
            Assert.AreEqual(v.Translate(-3f, 4f, 5f), v);
        }

        [TestMethod]
        public void InverseTranslation()
        {
            var mat = Matrix4x4.CreateTranslation(5f, -3f, 2f);
            var inv = mat.Invert();
            var p = new Vector3(-3f, 4f, 5f);
            var res = new Vector3(-8f, 7f, 3f);
            Assert.AreEqual(Vector3.Transform(p,inv), res);
        }

        [TestMethod]
        public void ScalePoint()
        {
            var p = new Vector3(-4f, 6f, 8f);
            var res = new Vector3(-8f, 18f, 32f);
            Assert.AreEqual(p.Scale(2f, 3f, 4f), res);
        }

        [TestMethod]
        public void ScaleVector()
        {
            var p = new Vector3(-4f, 6f, 8f);
            var res = new Vector3(-8f, 18f, 32f);
            Assert.AreEqual(p.Scale(2f, 3f, 4f), res);
        }

        [TestMethod]
        public void InverseScaling()
        {
            var v = new Vector3(-4f, 6f, 8f);
            var scale = Matrix4x4.CreateScale(2f, 3f, 4f);
            var inv = scale.Invert();
            Assert.AreEqual(Vector3.Transform(v, inv), new Vector3(-2f, 2f, 2f));
        }

        [TestMethod]
        public void Reflection()
        {
            var p = new Vector3(2f, 3f, 4f);
            Assert.AreEqual(p.Scale(-1f, 1f, 1f), new Vector3(-2f, 3f, 4f));
        }

        [TestMethod]
        public void RotateX()
        {
            var o = new Vector3(0f, 1f, 0f);
            Assert.AreEqual(o.RotateX(MathF.PI / 4f), new Vector3(0f, MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f));
        }

        [TestMethod]
        public void RotateY()
        {
            var o = new Vector3(0f, 0f, 1f);

            Assert.That.VectorsAreEqual(o.RotateY(MathF.PI / 2f), new Vector3(1f, 0f, 0f), 1e-5f);
            Assert.That.VectorsAreEqual(o.RotateY(MathF.PI / 4f), new Vector3(MathF.Sqrt(2f) / 2f, 0, MathF.Sqrt(2f) / 2f), 1e-5f);
        }

        [TestMethod]
        public void RotateZ()
        {
            var o = new Vector3(0f, 1f, 0f);

            Assert.That.VectorsAreEqual(o.RotateZ(MathF.PI / 2f), new Vector3(-1f, 0f, 0f), 1e-5f);
            Assert.That.VectorsAreEqual(o.RotateZ(MathF.PI / 4f), new Vector3(-MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f, 0f), 1e-5f);
        }

        [TestMethod]
        public void SkewXY()
        {
            var o = new Vector3(2f, 3f, 4f);
            var res = o.Skew(1f, 0f, 0f, 0f, 0f, 0f);

            Assert.AreEqual(res, new Vector3(5f, 3f, 4f));
        }

        [TestMethod]
        public void SkewXZ()
        {
            var o = new Vector3(2f, 3f, 4f);
            var res = o.Skew(0f, 1f, 0f, 0f, 0f, 0f);

            Assert.AreEqual(res, new Vector3(6f, 3f, 4f));
        }

        [TestMethod]
        public void SkewYX()
        {
            var o = new Vector3(2f, 3f, 4f);
            var res = o.Skew(0f, 0f, 1f, 0f, 0f, 0f);

            Assert.AreEqual(res, new Vector3(2f, 5f, 4f));
        }

        [TestMethod]
        public void SkewYZ()
        {
            var o = new Vector3(2f, 3f, 4f);
            var res = o.Skew(0f, 0f, 0f, 1f, 0f, 0f);

            Assert.AreEqual(res, new Vector3(2f, 7f, 4f));
        }

        [TestMethod]
        public void SkewZX()
        {
            var o = new Vector3(2f, 3f, 4f);
            var res = o.Skew(0f, 0f, 0f, 0f, 1f, 0f);

            Assert.AreEqual(res, new Vector3(2f, 3f, 6f));
        }

        [TestMethod]
        public void SkewZY()
        {
            var o = new Vector3(2f, 3f, 4f);
            var res = o.Skew(0f, 0f, 0f, 0f, 0f, 1f);

            Assert.AreEqual(res, new Vector3(2f, 3f, 7f));
        }

        [TestMethod]
        public void ChainingTransformations()
        {
            var p = new Vector3(1f, 0f, 1f);
            var t = Matrix4x4.CreateTranslation(10f, 5f, 7f);
            var s = Matrix4x4.CreateScale(5f);
            var r = Matrix4x4.CreateRotationX(MathF.PI / 2f);
            var res = Vector3.Transform(p, r);
            res = Vector3.Transform(res, s);
            res = Vector3.Transform(res, t);
            var scl = Vector3.Transform(p, s * r * t);
            Assert.AreEqual(res, new Vector3(15f, 0f, 7f));
            Assert.AreEqual(res, scl);
        }
    }
}
