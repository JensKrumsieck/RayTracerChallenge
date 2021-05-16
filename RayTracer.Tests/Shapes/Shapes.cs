using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Materials;
using RayTracer.Tests.TestObjects;
using System;
using System.Numerics;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests.Shapes
{
    [TestClass]
    public class Shapes
    {
        [TestMethod]
        public void DefaultTransformation()
        {
            var s = new TestShape();
            Assert.That.MatricesAreEqual(s.Transform, Matrix4x4.Identity);
        }

        [TestMethod]
        public void DefaultMaterial()
        {
            var s = new TestShape();
            Assert.AreEqual(s.Material, PhongMaterial.Default);
        }

        [TestMethod]
        public void AssignMaterial()
        {
            var m = PhongMaterial.Default;
            m.Ambient = 1f;
            var s = new TestShape
            {
                Material = m
            };
            Assert.AreEqual(s.Material.Ambient, 1f);
        }

        [TestMethod]
        public void IntersectScaledShape()
        {
            var r = new Ray(Point(0f, 0f, -5f), Direction(0f, 0f, 1f));
            var s = new TestShape(Scale(2f, 2f, 2f));
            s.Intersect(r);
            Assert.AreEqual(s.SavedRay.Origin, Point(0f, 0f, -2.5f));
            Assert.AreEqual(s.SavedRay.Direction, Direction(0f, 0f, .5f));
        }

        [TestMethod]
        public void IntersectTranslatedShape()
        {
            var r = new Ray(Point(0f, 0f, -5f), Direction(0f, 0f, 1f));
            var s = new TestShape(Translation(5f, 0f, 0f));
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 0);
            Assert.AreEqual(s.SavedRay.Origin, Point(-5f, 0f, -5f));
            Assert.AreEqual(s.SavedRay.Direction, Direction(0f, 0f, 1f));
        }


        [TestMethod]
        public void NormalOnTranslatedShape()
        {
            var s = new TestShape(Translation(0f, 1f, 0f));
            var n = s.Normal(Point(0f, 1.70711f, -.70711f));
            Assert.That.VectorsAreEqual(n, Direction(0f, .70711f, -.70711f));
        }

        [TestMethod]
        public void NormalOnTransformedShape()
        {
            var s = new TestShape(Scale(1f, .5f, 1f) * RotationZ(MathF.PI / 5f));
            var val = MathF.Sqrt(2f) / 2f;
            var n = s.Normal(Point(0f, val, -val));
            Assert.That.VectorsAreEqual(n, Direction(0f, .97014f, -.24254f));
        }
    }
}
