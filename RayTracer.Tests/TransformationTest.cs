using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Extension;
using System;
using System.Numerics;

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
            Assert.AreEqual(p.Multiply(Matrix.TranslationMatrix(new Vector3(5f, -3f, 2f))), res);
        }

        [TestMethod]
        public void TranslationNotAffectingVector4()
        {
            var v = new Vector4(-3f, 4f, 5f, 0f);
            Assert.AreEqual(v.Multiply(Matrix.TranslationMatrix(-3f, 4f, 5f)), v);
        }

        [TestMethod]
        public void InverseTranslation()
        {
            var mat = Matrix.TranslationMatrix(5f, -3f, 2f);
            var inv = mat.Invert();
            var p = new Vector3(-3f, 4f, 5f);
            var res = new Vector3(-8f, 7f, 3f);
            Assert.AreEqual(p.Multiply(inv), res);
        }

        [TestMethod]
        public void ScalePoint()
        {
            var p = new Vector3(-4f, 6f, 8f);
            var res = new Vector3(-8f, 18f, 32f);
            Assert.AreEqual(p.Multiply(Matrix.ScaleMatrix(2f, 3f, 4f)), res);
        }

        [TestMethod]
        public void ScaleVector()
        {
            var p = new Vector3(-4f, 6f, 8f);
            var res = new Vector3(-8f, 18f, 32f);
            Assert.AreEqual(p.Multiply(Matrix.ScaleMatrix(2f, 3f, 4f)), res);
        }

        [TestMethod]
        public void InverseScaling()
        {
            var v = new Vector3(-4f, 6f, 8f);
            var scale = Matrix.ScaleMatrix(2f, 3f, 4f);
            var inv = scale.Invert();
            Assert.AreEqual(v.Multiply(inv), new Vector3(-2f, 2f, 2f));
        }

        [TestMethod]
        public void Reflection()
        {
            var p = new Vector3(2f, 3f, 4f);
            Assert.AreEqual(p.Multiply(Matrix.ScaleMatrix(-1f, 1f, 1f)), new Vector3(-2f, 3f, 4f));
        }

        [TestMethod]
        public void RotateX()
        {
            var o = new Vector3(0f, 1f, 0f);
            Assert.AreEqual(o.Multiply(Matrix.RotationXMatrix(MathF.PI / 4f)), new Vector3(0f, MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f));
        }

        [TestMethod]
        public void RotateY()
        {
            var o = new Vector3(0f, 0f, 1f);

            Assert.That.VectorsAreEqual(o.Multiply(Matrix.RotationYMatrix(MathF.PI / 2f)), new Vector3(1f, 0f, 0f), 1e-5f);
            Assert.That.VectorsAreEqual(o.Multiply(Matrix.RotationYMatrix(MathF.PI / 4f)), new Vector3(MathF.Sqrt(2f) / 2f, 0, MathF.Sqrt(2f) / 2f), 1e-5f);
        }

        [TestMethod]
        public void RotateZ()
        {
            var o = new Vector3(0f, 1f, 0f);

            Assert.That.VectorsAreEqual(o.Multiply(Matrix.RotationZMatrix(MathF.PI / 2f)), new Vector3(-1f, 0f, 0f), 1e-5f);
            Assert.That.VectorsAreEqual(o.Multiply(Matrix.RotationZMatrix(MathF.PI / 4f)), new Vector3(-MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f, 0f), 1e-5f);
        }

        [TestMethod]
        public void SkewXY()
        {
            var o = new Vector3(2f, 3f, 4f);
            var res = o.Multiply(Matrix.SkewMatrix(1f, 0f, 0f, 0f, 0f, 0f));

            Assert.AreEqual(res, new Vector3(5f, 3f, 4f));
        }

        [TestMethod]
        public void SkewXZ()
        {
            var o = new Vector3(2f, 3f, 4f);
            var res = o.Multiply(Matrix.SkewMatrix(0f, 1f, 0f, 0f, 0f, 0f));

            Assert.AreEqual(res, new Vector3(6f, 3f, 4f));
        }

        [TestMethod]
        public void SkewYX()
        {
            var o = new Vector3(2f, 3f, 4f);
            var res = o.Multiply(Matrix.SkewMatrix(0f, 0f, 1f, 0f, 0f, 0f));

            Assert.AreEqual(res, new Vector3(2f, 5f, 4f));
        }

        [TestMethod]
        public void SkewYZ()
        {
            var o = new Vector3(2f, 3f, 4f);
            var res = o.Multiply(Matrix.SkewMatrix(0f, 0f, 0f, 1f, 0f, 0f));

            Assert.AreEqual(res, new Vector3(2f, 7f, 4f));
        }

        [TestMethod]
        public void SkewZX()
        {
            var o = new Vector3(2f, 3f, 4f);
            var res = o.Multiply(Matrix.SkewMatrix(0f, 0f, 0f, 0f, 1f, 0f));

            Assert.AreEqual(res, new Vector3(2f, 3f, 6f));
        }

        [TestMethod]
        public void SkewZY()
        {
            var o = new Vector3(2f, 3f, 4f);
            var res = o.Multiply(Matrix.SkewMatrix(0f, 0f, 0f, 0f, 0f, 1f));

            Assert.AreEqual(res, new Vector3(2f, 3f, 7f));
        }

        [TestMethod]
        public void ChainingTransformations()
        {
            var p = new Vector3(1f, 0f, 1f);
            var t = Matrix.TranslationMatrix(10f, 5f, 7f);
            var s = Matrix.ScaleMatrix(5f, 5f, 5f);
            var r = Matrix.RotationXMatrix(MathF.PI / 2f);
            var res = p.Multiply(r);
            res = res.Multiply(s);
            res = res.Multiply(t);
            var scl = p.Multiply(r).Multiply(s).Multiply(t);
            Assert.AreEqual(res, new Vector3(15f, 0f, 7f));
            Assert.AreEqual(res, scl);
        }
    }
}
