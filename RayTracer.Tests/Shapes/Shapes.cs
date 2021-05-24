using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Materials;
using RayTracer.Shapes;
using RayTracer.Tests.TestObjects;
using System;
using System.Collections.Generic;
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
            var xs = new List<Intersection>();
            s.Intersect(ref r, ref xs);
            Assert.AreEqual(s.SavedRay.Origin, Point(0f, 0f, -2.5f));
            Assert.AreEqual(s.SavedRay.Direction, Direction(0f, 0f, .5f));
        }

        [TestMethod]
        public void IntersectTranslatedShape()
        {
            var r = new Ray(Point(0f, 0f, -5f), Direction(0f, 0f, 1f));
            var s = new TestShape(Translation(5f, 0f, 0f));
            var xs = new List<Intersection>();
            s.Intersect(ref r, ref xs);
            Assert.AreEqual(xs.Count, 0);
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

        [TestMethod]
        public void ShapeHasParent()
        {
            var s = new TestShape();
            Assert.IsNull(s.Parent);
        }

        [TestMethod]
        public void PointFromWorldToObject()
        {
            var g1 = new Group(RotationY(MathF.PI / 2));
            var g2 = new Group(Scale(2));
            g1.AddChild(g2);
            var s = new Sphere(Translation(5, 0, 0));
            g2.AddChild(s);
            var p = s.WorldToObject(Point(-2, 0, -10));
            Assert.That.VectorsAreEqual(p, Point(0, 0, -1));
        }

        [TestMethod]
        public void ConvertNormalToWorld()
        {
            var g1 = new Group(RotationY(MathF.PI / 2));
            var g2 = new Group(Scale(1, 2, 3));
            g1.AddChild(g2);
            var s = new Sphere(Translation(5, 0, 0));
            g2.AddChild(s);
            var n = s.NormalToWorld(Direction(MathF.Sqrt(3) / 3, MathF.Sqrt(3) / 3, MathF.Sqrt(3) / 3));
            Assert.That.VectorsAreEqual(n, Direction(.2857f, .4286f, -.8571f), 1e-4f);
        }

        [TestMethod]
        public void FindNormalOnChild()
        {
            var g1 = new Group(RotationY(MathF.PI / 2));
            var g2 = new Group(Scale(1, 2, 3));
            g1.AddChild(g2);
            var s = new Sphere(Translation(5, 0, 0));
            g2.AddChild(s);
            Assert.That.VectorsAreEqual(s.Normal(Point(1.7321f, 1.1547f, -5.5774f)), Direction(.2857f, .4286f, -.8571f), 1e-4f);
        }
    }
}
