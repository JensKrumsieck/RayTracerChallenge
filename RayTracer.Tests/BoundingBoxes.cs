using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests
{
    [TestClass]
    public class BoundingBoxes
    {
        [TestMethod]
        public void CreateEmpty()
        {
            var b = Bounds.Empty;
            Assert.AreEqual(b.Min, Point(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity));
            Assert.AreEqual(b.Max, Point(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity));
        }

        [TestMethod]
        public void CreateWithVolume()
        {
            var b = new Bounds { Min = Point(-1, -2, -3), Max = Point(3, 2, 1) };
            Assert.AreEqual(b.Max, Point(3, 2, 1));
            Assert.AreEqual(b.Min, Point(-1, -2, -3));
        }

        [TestMethod]
        public void AddingPoints()
        {
            var b = Bounds.Empty;
            var p1 = Point(-5, 2, 0);
            var p2 = Point(7, 0, -3);
            b.Add(p1, p2);
            Assert.AreEqual(b.Min, Point(-5, 0, -3));
            Assert.AreEqual(b.Max, Point(7, 2, 0));
        }

        [TestMethod]
        public void AddBoxToBox()
        {
            var b1 = new Bounds { Min = Point(-5, -2, 0), Max = Point(7, 4, 4) };
            var b2 = new Bounds { Min = Point(8, -7, -2), Max = Point(14, 2, 8) };
            b1.Add(b2);
            Assert.AreEqual(b1.Max, Point(14, 4, 8));
            Assert.AreEqual(b1.Min, Point(-5, -7, -2));
        }

        [TestMethod]
        public void ContainsPoint()
        {
            var b = new Bounds { Min = Point(5, -2, 0), Max = Point(11, 4, 7) };
            var points = new[]
            {
                (Point(5, -2, 0), true),
                (Point(11, 4, 7), true),
                (Point(8, 1, 3), true),
                (Point(3, 0, 3), false),
                (Point(8, -4, 3), false),
                (Point(8, 1, -1), false),
                (Point(13, 1, 3), false),
                (Point(8, 5, 3), false),
                (Point(8, 1, 8), false),
            };
            foreach (var (p, r) in points)
            {
                Assert.IsTrue(b.Contains(p) == r);
            }
        }

        [TestMethod]
        public void ContainsBox()
        {
            var b1 = new Bounds { Min = Point(5, -2, 0), Max = Point(11, 4, 7) };
            var boxes = new Bounds[]
            {
                new() {Min = Point(5, -2, 0), Max = Point(11, 4, 7)},
                new() {Min = Point(6, -1, 1), Max = Point(10, 3, 6)},
                new() {Min = Point(4, -3, -1), Max = Point(10, 3, 6)},
                new() {Min = Point(6, -1, 1), Max = Point(12, 5, 8)}
            };
            var res = new[] { true, true, false, false };
            for (var i = 0; i < boxes.Length; i++) Assert.AreEqual(b1.Contains(boxes[i]), res[i]);
        }

        [TestMethod]
        public void TransformedBox()
        {
            var box = Bounds.DefaultBox;
            var m = RotationX(MathF.PI / 4f) * RotationY(MathF.PI / 4f);
            var b2 = box.Transform(m);
            Assert.That.VectorsAreEqual(b2.Min, Point(-1.4142f, -1.7071f, -1.7071f), 1e-4f);
            Assert.That.VectorsAreEqual(b2.Max, Point(1.4142f, 1.7071f, 1.7071f), 1e-4f);
        }

        [TestMethod]
        public void IntersectBox()
        {
            var b = Bounds.DefaultBox;
            var rayToRes = new[]
            {
                (new Ray(5, .5f, 0, -1, 0, 0), true),
                (new Ray(-5, .5f, 0, 1, 0, 0), true),
                (new Ray(.5f, 5, 0, 0, -1, 0), true),
                (new Ray(.5f, -5, 0, 0, 1, 0), true),
                (new Ray(.5f, 0, 5, 0, 0, -1), true),
                (new Ray(.5f, 0, -5, 0, 0, 1), true),
                (new Ray(0, .5f, 0, 0, 0, 1), true),
                (new Ray(-2, 0, 0, 2, 4, 6), false),
                (new Ray(0, -2, 0, 6, 2, 4), false),
                (new Ray(0, 0, -2, 4, 6, 2), false),
                (new Ray(0, 2, 2, 0, -1, 0), false),
                (new Ray(2, 2, 0, -1, 0, 0), false),
            };
            foreach (var (r, res) in rayToRes)
            {
                var ray = new Ray(r.Origin, Vector4.Normalize(r.Direction));
                Assert.AreEqual(b.IntersectLocal(ref ray), res);
            }
        }

        [TestMethod]
        public void NonCubicBounds()
        {
            var b = new Bounds { Min = Point(5, -2, 0), Max = Point(11, 4, 7) };
            var rayToRes = new[]
            {
                (new Ray(15,1,2,-1,0,0), true),
                (new Ray(-5,1,4,1,0,0), true),
                (new Ray(7,6,5,0,-1,0), true),
                (new Ray(9,-5,6,0,1,0), true),
                (new Ray(8,2,12,0,0,-1), true),
                (new Ray(6,0,-5,0,0,1), true),
                (new Ray(8,1,3.5f,0,0,1), true),
                (new Ray(9,-1,-8,2,4,6), false),
                (new Ray(8,3,-4,6,2,4), false),
                (new Ray(9,-1,-2, 4, 6, 2), false),
                (new Ray(4,0,9,0,0,-1), false),
                (new Ray(8,6,-1,0,-1,0), false),
                (new Ray(12,5,4,-1,0,0), false)
            };
            foreach (var (r, res) in rayToRes)
            {
                var ray = new Ray(r.Origin, Vector4.Normalize(r.Direction));
                Assert.AreEqual(b.IntersectLocal(ref ray), res);
            }
        }
    }
}
