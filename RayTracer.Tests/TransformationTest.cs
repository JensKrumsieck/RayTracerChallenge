﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Assert.AreEqual(o.RotateX(MathF.PI / 4f), Vector3.Point(0f, MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f));
        }

        [TestMethod]
        public void RotateY()
        {
            var o = Vector3.Point(0f, 0f, 1f);

            Assert.AreEqual(o.RotateY(MathF.PI / 2f), Vector3.Point(1f, 0f, 0f));
            Assert.AreEqual(o.RotateY(MathF.PI / 4f), Vector3.Point(MathF.Sqrt(2f) / 2f, 0, MathF.Sqrt(2f) / 2f));
        }

        [TestMethod]
        public void RotateZ()
        {
            var o = Vector3.Point(0f, 1f, 0f);

            Assert.AreEqual(o.RotateZ(MathF.PI / 2f), Vector3.Point(-1f, 0f, 0f));
            Assert.AreEqual(o.RotateZ(MathF.PI / 4f), Vector3.Point(-MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f, 0f));
        }

        [TestMethod]
        public void SkewXY()
        {
            var o = Vector3.Point(2f, 3f, 4f);
            var res = o.Skew(1f, 0f, 0f, 0f, 0f, 0f);

            Assert.AreEqual(res, Vector3.Point(5f, 3f, 4f));
        }

        [TestMethod]
        public void SkewXZ()
        {
            var o = Vector3.Point(2f, 3f, 4f);
            var res = o.Skew(0f, 1f, 0f, 0f, 0f, 0f);

            Assert.AreEqual(res, Vector3.Point(6f, 3f, 4f));
        }

        [TestMethod]
        public void SkewYX()
        {
            var o = Vector3.Point(2f, 3f, 4f);
            var res = o.Skew(0f, 0f, 1f, 0f, 0f, 0f);

            Assert.AreEqual(res, Vector3.Point(2f, 5f, 4f));
        }

        [TestMethod]
        public void SkewYZ()
        {
            var o = Vector3.Point(2f, 3f, 4f);
            var res = o.Skew(0f, 0f, 0f, 1f, 0f, 0f);

            Assert.AreEqual(res, Vector3.Point(2f, 7f, 4f));
        }

        [TestMethod]
        public void SkewZX()
        {
            var o = Vector3.Point(2f, 3f, 4f);
            var res = o.Skew(0f, 0f, 0f, 0f, 1f, 0f);

            Assert.AreEqual(res, Vector3.Point(2f, 3f, 6f));
        }

        [TestMethod]
        public void SkewZY()
        {
            var o = Vector3.Point(2f, 3f, 4f);
            var res = o.Skew(0f, 0f, 0f, 0f, 0f, 1f);

            Assert.AreEqual(res, Vector3.Point(2f, 3f, 7f));
        }

        [TestMethod]
        public void ChainingTransformations()
        {
            var p = Vector3.Point(1f, 0f, 1f);
            var res = p.RotateX(MathF.PI / 2f).Scale(5f, 5f, 5f).Translate(10f, 5f, 7f);
            var resReverse = Matrix.Translation(10f, 5f, 7f) * Matrix.Scale(5f, 5f, 5f) * Matrix.RotationX(MathF.PI / 2f) * p;
            Assert.AreEqual(res, resReverse);
            Assert.AreEqual(res, Vector3.Point(15f, 0f, 7f));
        }
    }
}
