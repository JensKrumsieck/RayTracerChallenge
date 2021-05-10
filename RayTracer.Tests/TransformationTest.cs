using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using System;

namespace RayTracer.Tests
{
    [TestClass]
    public class TransformationTest
    {
        [TestMethod]
        public void MultiplyByTranslation()
        {
            var p = Vector3.Point(-3f, 4f, 5f);
            var res = Vector3.Point(2f, 1f, 7f);
            Assert.AreEqual(p.Translate(5f, -3f, 2f), res);
        }

        [TestMethod]
        public void TranslationNotAffectingVectors()
        {
            var v = Vector3.Vector(-3f, 4f, 5f);
            Assert.AreEqual(v.Translate(-3f, 4f, 5f), v);
        }

        [TestMethod]
        public void InverseTranslation()
        {
            var mat = Matrix.Translation(5f, -3f, 2f);
            var inv = mat.Inverse();
            var p = Vector3.Point(-3f, 4f, 5f);
            var res = Vector3.Point(-8f, 7f, 3f);
            Assert.AreEqual(inv * p, res);
        }

        [TestMethod]
        public void ScalePoint()
        {
            var p = Vector3.Point(-4f, 6f, 8f);
            var res = Vector3.Point(-8f, 18f, 32f);
            Assert.AreEqual(p.Scale(2f, 3f, 4f), res);
        }

        [TestMethod]
        public void ScaleVector()
        {
            var p = Vector3.Vector(-4f, 6f, 8f);
            var res = Vector3.Vector(-8f, 18f, 32f);
            Assert.AreEqual(p.Scale(2f, 3f, 4f), res);
        }

        [TestMethod]
        public void InverseScaling()
        {
            var v = Vector3.Vector(-4f, 6f, 8f);
            var scale = Matrix.Scale(2f, 3f, 4f);
            var inv = scale.Inverse();
            Assert.AreEqual(inv * v, Vector3.Vector(-2f, 2f, 2f));
        }

        [TestMethod]
        public void Reflection()
        {
            var p = Vector3.Point(2f, 3f, 4f);
            Assert.AreEqual(p.Scale(-1f, 1f, 1f), Vector3.Point(-2f, 3f, 4f));
        }

        [TestMethod]
        public void RotateX()
        {
            var o = Vector3.Point(0f, 1f, 0f);
            Assert.AreEqual(o.RotateX(MathF.PI / 2f), Vector3.Point(0f, 0f, 1f));
        }

        [TestMethod]
        public void RotateY()
        {
            var o = Vector3.Point(0f, 0f, 1f);
            var res = o.RotateY(MathF.PI / 2f);

            Assert.AreEqual(res, Vector3.Point(1f, 0f, 0f));
        }

        [TestMethod]
        public void RotateZ()
        {
            var o = Vector3.Point(0f, 1f, 0f);
            var res = o.RotateZ(MathF.PI / 2f);

            Assert.AreEqual(res, Vector3.Point(-1f, 0f, 0f));
        }
    }
}
