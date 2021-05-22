using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MathNet.Numerics.LinearAlgebra.Complex.Solvers;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests
{
    [TestClass]
    public class Intersections
    {
        [TestMethod]
        public void IntersectionHasObjectAndDistance()
        {
            var s = new Sphere();
            var i = new Intersection(3.5f, s);
            Assert.AreEqual(i.Distance, 3.5f);
            Assert.AreEqual(i.Object, s);
        }

        [TestMethod]
        public void AggregatingIntersections()
        {
            var s = new Sphere();
            var i1 = new Intersection(1f, s);
            var i2 = new Intersection(2f, s);
            var xs = new List<Intersection> { i1, i2 };
            Assert.AreEqual(xs.Count, 2);
            Assert.AreEqual(xs[0], i1);
            Assert.AreEqual(xs[1], i2);
        }

        [TestMethod]
        public void HitWhenAllPositive()
        {
            var s = new Sphere();
            var i1 = new Intersection(1, s);
            var i2 = new Intersection(2, s);
            var xs = new List<Intersection> { i1, i2 };
            var i = Intersection.Hit(ref xs);
            Assert.AreEqual(i, i1);
        }

        [TestMethod]
        public void HitWhenNegative()
        {
            var s = new Sphere();
            var i1 = new Intersection(-1, s);
            var i2 = new Intersection(1, s);
            var xs = new List<Intersection> { i1, i2 };
            var i = Intersection.Hit(ref xs);
            Assert.AreEqual(i, i2);
        }

        [TestMethod]
        public void HitWhenAllNegative()
        {
            var s = new Sphere();
            var i1 = new Intersection(-1, s);
            var i2 = new Intersection(-2, s);
            var xs = new List<Intersection> { i1, i2 };
            var i = Intersection.Hit(ref xs);
            Assert.AreEqual(i, null);
        }

        [TestMethod]
        public void HitLowestNonNegative()
        {
            var s = new Sphere();
            var i1 = new Intersection(5, s);
            var i2 = new Intersection(7, s);
            var i3 = new Intersection(-3, s);
            var i4 = new Intersection(2, s);
            var xs = new List<Intersection> { i1, i2, i3, i4 };
            var i = Intersection.Hit(ref xs);
            Assert.AreEqual(i, i4);
        }

        [TestMethod]
        public void PrecomputingStateOfIntersection()
        {
            var r = new Ray(0f, 0f, -5f, 0f, 0f, 1f);
            var s = new Sphere();
            var i = new Intersection(4f, s);
            var comps = IntersectionState.Prepare(ref i, ref r);
            Assert.AreEqual(comps.Distance, i.Distance);
            Assert.AreEqual(comps.Object, i.Object);
            Assert.That.VectorsAreEqual(comps.Point, Point(0f, 0f, -1f));
            Assert.That.VectorsAreEqual(comps.Eye, Direction(0f, 0f, -1f));
            Assert.That.VectorsAreEqual(comps.Normal, Direction(0f, 0f, -1f));
        }

        [TestMethod]
        public void HitOccursOutside()
        {
            var r = new Ray(0f, 0f, -5f, 0f, 0f, 1f);
            var s = new Sphere();
            var i = new Intersection(4f, s);
            var comps = IntersectionState.Prepare(ref i, ref r);
            Assert.IsFalse(comps.IsInside);
        }

        [TestMethod]
        public void HitOccursInside()
        {
            var r = new Ray(0f, 0f, 0f, 0f, 0f, 1f);
            var s = new Sphere();
            var i = new Intersection(1f, s);
            var comps = IntersectionState.Prepare(ref i, ref r);
            Assert.IsTrue(comps.IsInside);
            Assert.That.VectorsAreEqual(comps.Point, Point(0f, 0f, 1f));
            Assert.That.VectorsAreEqual(comps.Eye, Direction(0f, 0f, -1f));
            Assert.That.VectorsAreEqual(comps.Normal, Direction(0f, 0f, -1f));
        }

        [TestMethod]
        public void HitOffsetsPoint()
        {
            var r = new Ray(0f, 0f, -5f, 0f, 0f, 1f);
            var s = new Sphere(Translation(0f, 0f, 1f));
            var i = new Intersection(5f, s);
            var c = IntersectionState.Prepare(ref i, ref r);
            Assert.IsTrue(c.OverPoint.Z < -Constants.Epsilon / 2f);
            Assert.IsTrue(c.Point.Z > c.OverPoint.Z);
        }

        [TestMethod]
        public void PrecomputingReflectionVector()
        {
            var s = new Plane();
            var r = new Ray(0f, 1f, -1f, 0f, -MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f);
            var i = new Intersection(MathF.Sqrt(2f), s);
            var comps = IntersectionState.Prepare(ref i, ref r);
            Assert.AreEqual(comps.Reflect, Direction(0f, MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f));
        }

        [TestMethod]
        public void FindingN1AndN2()
        {
            var a = Sphere.GlassSphere;
            a.Transform = Scale(2f);

            var b = Sphere.GlassSphere;
            b.Transform = Translation(0, 0, -.25f);
            b.Material.IOR = 2f;

            var c = Sphere.GlassSphere;
            c.Transform = Translation(0, 0, .25f);
            c.Material.IOR = 2.5f;
            var r = new Ray(0, 0, -4, 0, 0, 1);
            var xs = new List<Intersection>
            {
                new(2f, a),
                new(2.75f, b),
                new(3.25f, c),
                new(4.75f, b),
                new(5.25f, c),
                new(6, a)
            };
            var n1 = new [] {1, 1.5f, 2, 2.5f, 2.5f, 1.5f};
            var n2 = new [] {1.5f, 2, 2.5f, 2.5f, 1.5f, 1};
            for (var i = 0; i <= 5; i++)
            {
                var hit = xs[i];
                var comps = IntersectionState.Prepare(ref hit, ref r, xs);
                Assert.AreEqual(comps.N1, n1[i]);
                Assert.AreEqual(comps.N2, n2[i]);
            }
        }

        [TestMethod]
        public void UnderPointIstBelowSurf()
        {
            var r = new Ray(0, 0, -5, 0, 0, 1);
            var s = Sphere.GlassSphere;
            s.Transform = Translation(0, 0, 1);
            var i = new Intersection(5, s);
            var comps = IntersectionState.Prepare(ref i, ref r); //third argument is list of i
            Assert.IsTrue(comps.UnderPoint.Z > Constants.Epsilon / 2f);
            Assert.IsTrue(comps.Point.Z < comps.UnderPoint.Z);
        }

        [TestMethod]
        public void SchlickTotalReflection()
        {
            var s = Sphere.GlassSphere;
            var r = new Ray(0, 0, MathF.Sqrt(2f) / 2f, 0, 1, 0);
            var xs = new List<Intersection>
            {
                new(-MathF.Sqrt(2f) / 2f, s),
                new(MathF.Sqrt(2f) / 2f, s)
            };
            var hit = xs[1];
            var comps = IntersectionState.Prepare(ref hit, ref r, xs);
            Assert.AreEqual(comps.Schlick(), 1f);
        }

        [TestMethod]
        public void SchlickPerpendicular()
        {
            var s = Sphere.GlassSphere;
            var r = new Ray(0, 0, 0, 0, 1, 0);
            var xs = new List<Intersection>
            {
                new(-1, s),
                new(1, s)
            };
            var hit = xs[1];
            var comps = IntersectionState.Prepare(ref hit, ref r, xs);
            Assert.That.FloatsAreEqual(comps.Schlick(), .04f);
        }

        [TestMethod]
        public void SchlickN2GreaterN1()
        {
            var s = Sphere.GlassSphere;
            var r = new Ray(0, .99f, -2, 0, 0, 1);
            var xs = new Intersection(1.8589f, s);
            var comps = IntersectionState.Prepare(ref xs, ref r);
            Assert.That.FloatsAreEqual(comps.Schlick(), .48873f);
        }
    }
}
