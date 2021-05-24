using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Shapes;
using RayTracer.Tests.TestObjects;
using System.Collections.Generic;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests.Shapes
{
    [TestClass]
    public class Groups
    {
        [TestMethod]
        public void CreateGroup()
        {
            var g = new Group();
            Assert.AreEqual(g.Transform, Transform.Identity);
            Assert.AreEqual(g.Count, 0);
        }

        [TestMethod]
        public void AddChild()
        {
            var g = new Group();
            var t = new TestShape();
            g.AddChild(t);
            Assert.AreEqual(g.Count, 1);
            Assert.IsTrue(g.Contains(t));
            Assert.AreEqual(t.Parent, g);
        }

        [TestMethod]
        public void IntersectRayEmptyGroup()
        {
            var g = new Group();
            var r = new Ray(0, 0, 0, 0, 0, 1);
            var xs = g.IntersectLocal(ref r);
            Assert.AreEqual(xs.Count, 0);
        }

        [TestMethod]
        public void IntersectingWithNonEmptyGroup()
        {
            var g = new Group();
            var s1 = new Sphere();
            var s2 = new Sphere(Translation(0, 0, -3));
            var s3 = new Sphere(Translation(5, 0, 0));
            g.AddChildren(s1, s2, s3);
            var r = new Ray(0, 0, -5, 0, 0, 1);
            var xs = g.IntersectLocal(ref r);
            xs.Sort(); //do not sort in groups as world does already do that
            Assert.AreEqual(xs.Count, 4);
            Assert.AreEqual(xs[0].Object, s2);
            Assert.AreEqual(xs[1].Object, s2);
            Assert.AreEqual(xs[2].Object, s1);
            Assert.AreEqual(xs[3].Object, s1);
        }

        [TestMethod]
        public void IntersectTransformedGroup()
        {
            var g = new Group(Scale(2));
            var s = new Sphere(Translation(5, 0, 0));
            g.AddChild(s);
            var r = new Ray(10, 0, -10, 0, 0, 1);
            var xs = new List<Intersection>();
            g.Intersect(ref r, ref xs);
            Assert.AreEqual(xs.Count, 2);
        }

        [TestMethod]
        public void GroupHasBoxContainingChildren()
        {
            var s = new Sphere(Translation(2, 5, -3) * Scale(2));
            var c = new Cylinder(Translation(-4, -1, 4) * Scale(.5f, 1, .5f)) { Minimum = -2, Maximum = 2 };
            var g = new Group();
            g.AddChildren(s, c);
            var b = g.BoundingBox;
            Assert.AreEqual(b.Min, Point(-4.5f, -3, -5));
            Assert.AreEqual(b.Max, Point(4, 7, 4.5f));
        }

        [TestMethod]
        public void IntersectDoesNotTestChildIfMissed()
        {
            var c = new TestShape();
            var s = new Group();
            s.AddChild(c);
            var r = new Ray(0, 0, -5, 0, 1, 0);
            var xs = new List<Intersection>();
            s.Intersect(ref r, ref xs);
            Assert.AreNotEqual(c.SavedRay, r);
            Assert.IsNull(c.SavedRay);
        }

        [TestMethod]
        public void SavedRayIsSetIfHit()
        {
            var c = new TestShape();
            var s = new Group();
            s.AddChild(c);
            var r = new Ray(0, 0, -5, 0, 0, 1);
            var xs = new List<Intersection>();
            s.Intersect(ref r, ref xs);
            Assert.IsNotNull(c.SavedRay);
            Assert.AreEqual(c.SavedRay, r);
        }

        [TestMethod]
        public void PartitioningChildren()
        {
            var s1 = new Sphere(Translation(-2, 0, 0));
            var s2 = new Sphere(Translation(2, 0, 0));
            var s3 = new Sphere();
            var g = new Group();
            g.AddChildren(s1, s2, s3);
            var (left, right) = g.Partition();
            Assert.IsTrue(g.Contains(s3));
            Assert.IsTrue(left.Contains(s1));
            Assert.IsTrue(right.Contains(s2));
            Assert.AreEqual(g.Count, 1);
            Assert.AreEqual(left.Count, 1);
            Assert.AreEqual(right.Count, 1);
        }

        [TestMethod]
        public void SubDivideGroup()
        {
            var s1 = new Sphere(Translation(-2, -2, 0));
            var s2 = new Sphere(Translation(-2, 2, 0));
            var s3 = new Sphere(Scale(4));
            var g = new Group();
            g.AddChildren(s1, s2, s3);
            g.Divide();
            Assert.AreEqual(g.Count, 2);
            Assert.IsFalse(g.Contains(s1));
            Assert.IsFalse(g.Contains(s2));
            Assert.IsTrue(g.Contains(s3));
        }
    }
}
