using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Environment;
using RayTracer.Materials;
using RayTracer.Shapes;
using RayTracer.Tests.TestObjects;
using System;
using System.Collections.Generic;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests.Environment
{
    [TestClass]
    public class WorldTest
    {
        [TestMethod]
        public void CreateWorld()
        {
            var w = new World();
            Assert.AreEqual(w.Objects.Count, 0);
            Assert.AreEqual(w.Lights.Count, 0);
        }

        [TestMethod]
        public void DefaultWorld()
        {
            var w = World.Default;
            var s1 = new Sphere
            {
                Material = new PhongMaterial(new Color(.8f, 1f, .6f)) { Diffuse = .7f, Specular = .2f }
            };
            var s2 = new Sphere(Scale(.5f));
            Assert.AreEqual(w.Objects.Count, 2);
            Assert.AreEqual(w.Lights.Count, 1);
            Assert.AreEqual(w.Objects[0], s1);
            Assert.AreEqual(w.Objects[1], s2);
            Assert.AreEqual(w.Lights[0], PointLight.Default);
        }

        [TestMethod]
        public void IntersectWorldWithRay()
        {
            var w = World.Default;
            var ray = new Ray(0f, 0f, -5f, 0f, 0f, 1f);
            var xs = new List<Intersection>();
            w.Intersect(ref ray, ref xs);
            Assert.AreEqual(xs.Count, 4);
            Assert.AreEqual(xs[0].Distance, 4f);
            Assert.AreEqual(xs[1].Distance, 4.5f);
            Assert.AreEqual(xs[2].Distance, 5.5f);
            Assert.AreEqual(xs[3].Distance, 6f);
        }

        [TestMethod]
        public void ShadingIntersection()
        {
            var w = World.Default;
            var r = new Ray(0f, 0f, -5f, 0f, 0f, 1f);
            var s = w.Objects[0];
            var i = new Intersection(4f, s);
            var comps = IntersectionState.Prepare(ref i, ref r);
            var c = w.ShadeHit(ref comps);
            Assert.That.VectorsAreEqual(c, new Color(.38066f, .47583f, .2855f));
        }

        [TestMethod]
        public void ShadingIntersectionInside()
        {
            var w = World.Default;
            var r = new Ray(0f, 0f, 0f, 0f, 0f, 1f);
            var s = w.Objects[1];
            w.Lights[0] = new PointLight(Point(0f, .25f, 0f), Color.White);
            var i = new Intersection(.5f, s);
            var comps = IntersectionState.Prepare(ref i, ref r);
            var c = w.ShadeHit(ref comps);
            Assert.That.VectorsAreEqual(c, new Color(.90498f, .90498f, .90498f));
        }

        [TestMethod]
        public void ColorRayMisses()
        {
            var w = World.Default;
            var r = new Ray(0f, 0f, -5f, 0f, 1f, 0f);
            var c = w.ColorAt(ref r);
            Assert.That.VectorsAreEqual(c, Color.Black);
        }

        [TestMethod]
        public void ColorRayHits()
        {
            var w = World.Default;
            var r = new Ray(0f, 0f, -5f, 0f, 0f, 1f);
            var c = w.ColorAt(ref r);
            Assert.That.VectorsAreEqual(c, new Color(.38066f, .47583f, .2855f));
        }

        [TestMethod]
        public void ColorIntersectionBehindRay()
        {
            var w = World.Default;
            var outer = w.Objects[0];
            var mat = outer.Material;
            mat.Ambient = 1f;
            outer.Material = mat;
            var inner = w.Objects[1];
            var mat2 = inner.Material;
            mat2.Ambient = 1f;
            inner.Material = mat2;
            var r = new Ray(0f, 0f, .75f, 0f, 0f, -1f);
            var c = w.ColorAt(ref r);
            Assert.That.VectorsAreEqual(c, inner.Material.BaseColor);
        }

        [TestMethod]
        public void NoShadowWhenNothingCollinear()
        {
            var w = World.Default;
            var p = Point(0f, 10f, 0f);
            Assert.IsFalse(w.InShadow(w.Lights[0], p));
        }

        [TestMethod]
        public void ShadowWhenObjectBetweenPointLight()
        {
            var w = World.Default;
            var p = Point(10f, -10f, 10f);
            Assert.IsTrue(w.InShadow(w.Lights[0], p));
        }

        [TestMethod]
        public void NoShadowWhenBehindPoint()
        {
            var w = World.Default;
            var p = Point(-2f, 2f, -2f);
            Assert.IsFalse(w.InShadow(w.Lights[0], p));
        }

        [TestMethod]
        public void ReflectedColorForNonReflectiveMaterial()
        {
            var w = World.Default;
            var r = new Ray(0f, 0f, 0f, 0f, 0f, 1f);
            var s = w.Objects[1];
            s.Material.Ambient = 1;
            var i = new Intersection(1, s);
            var comps = IntersectionState.Prepare(ref i, ref r);
            var col = w.ReflectedColor(ref comps);
            Assert.That.VectorsAreEqual(col, Color.Black);
        }

        [TestMethod]
        public void ReflectedColorReflectiveMaterial()
        {
            var w = World.Default;
            var s = new Plane(Translation(0, -1, 0)) { Material = { Reflectivity = .5f } };
            w.Objects.Add(s);
            var r = new Ray(0, 0, -3, 0, -MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f);
            var i = new Intersection(MathF.Sqrt(2f), s);
            var comps = IntersectionState.Prepare(ref i, ref r);
            var col = w.ReflectedColor(ref comps, 1);
            Assert.That.VectorsAreEqual(col, new Color(.19032f, .2379f, .14274f), 1e-4f);
        }

        [TestMethod]
        public void ShadeHitWithReflective()
        {
            var w = World.Default;
            var s = new Plane(Translation(0f, -1f, 0f)) { Material = { Reflectivity = .5f } };
            w.Objects.Add(s);
            var r = new Ray(0, 0, -3, 0, -MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f);
            var i = new Intersection(MathF.Sqrt(2f), s);
            var comps = IntersectionState.Prepare(ref i, ref r);
            var col = w.ShadeHit(ref comps, 5);
            Assert.That.VectorsAreEqual(col, new Color(.87677f, .92436f, .82918f), 1e-4f);
        }

        [TestMethod]
        public void AvoidInfiniteLoop()
        {
            var w = new World
            {
                Lights = new List<PointLight> { new(Point(0, 0, 0), Color.White) }
            };
            var lower = new Plane(Translation(0, -1, 0))
            {
                Material = { Reflectivity = 1 }
            };
            var upper = new Plane(Translation(0, 1, 0))
            {
                Material = { Reflectivity = 1 }
            };
            w.Objects.AddRange(new[] { lower, upper });
            var r = new Ray(0, 0, 0, 0, 1, 0);
            Assert.AreNotEqual(w.ColorAt(ref r, 100), Color.Black);
        }

        [TestMethod]
        public void LimitRecursion()
        {
            var w = World.Default;
            var s = new Plane(Translation(0, -1, 0))
            { Material = { Reflectivity = .5f } };
            w.Objects.Add(s);
            var r = new Ray(0, 0, -3, 0, -MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f);
            var i = new Intersection(MathF.Sqrt(2f), s);
            var comps = IntersectionState.Prepare(ref i, ref r);
            var col = w.ReflectedColor(ref comps, 0);
            Assert.That.VectorsAreEqual(col, Color.Black);
        }

        [TestMethod]
        public void RefractedColorOpaque()
        {
            var w = World.Default;
            var s = w.Objects[0];
            var r = new Ray(0, 0, -5, 0, 0, 1);
            var xs = new List<Intersection>
            {
                new(4, s),
                new(6, s)
            };
            var hit = xs[0];
            var comps = IntersectionState.Prepare(ref hit, ref r, xs);
            Assert.AreEqual(w.RefractedColor(ref comps, 5), Color.Black);
        }

        [TestMethod]
        public void RefractedColorLimit()
        {
            var w = World.Default;
            var s = w.Objects[0];
            s.Material.Transparency = 1;
            s.Material.IOR = 1.5f;
            var r = new Ray(0, 0, -5, 0, 0, 1);
            var xs = new List<Intersection>
            {
                new(4, s),
                new(6, s)
            };
            var hit = xs[0];
            var comps = IntersectionState.Prepare(ref hit, ref r, xs);
            Assert.AreEqual(w.RefractedColor(ref comps, 0), Color.Black);
        }

        [TestMethod]
        public void RefractedColorUnterTotalReflection()
        {
            var w = World.Default;
            var s = w.Objects[0];
            s.Material.Transparency = 1;
            s.Material.IOR = 1.5f;
            var r = new Ray(0, 0, MathF.Sqrt(2f) / 2f, 0, 1, 0);
            var xs = new List<Intersection>
            {
                new(-MathF.Sqrt(2f) / 2f, s),
                new(MathF.Sqrt(2f) / 2f, s)
            };
            var hit = xs[1];
            var comps = IntersectionState.Prepare(ref hit, ref r, xs);
            Assert.AreEqual(w.RefractedColor(ref comps, 5), Color.Black);
        }

        [TestMethod]
        public void RefractedColorWithRay()
        {
            var w = World.Default;
            var s = w.Objects[0];
            s.Material.Ambient = 1f;
            s.Material.Pattern = new TestPattern();
            var b = w.Objects[1];
            b.Material.Transparency = 1;
            b.Material.IOR = 1.5f;
            var r = new Ray(0, 0, .1f, 0, 1, 0);
            var xs = new List<Intersection>
            {
                new(-.9899f, s),
                new(-.4899f, b),
                new(.4899f, b),
                new(.9899f, s)
            };
            var hit = xs[2];
            var comps = IntersectionState.Prepare(ref hit, ref r, xs);
            var c = w.RefractedColor(ref comps, 5);
            Assert.That.VectorsAreEqual(c, new Color(0, .99888f, .04725f), 1e-4f);
        }

        [TestMethod]
        public void ShadeHitRefraction()
        {
            var w = World.Default;
            var floor = new Plane(Translation(0, -1, 0))
            {
                Material = { Transparency = .5f, IOR = 1.5f }
            };
            w.Objects.Add(floor);
            var s = new Sphere(Translation(0, -3.5f, -.5f))
            {
                Material =
                {
                    BaseColor = new Color(1, 0, 0),
                    Ambient = .5f
                }
            };
            w.Objects.Add(s);
            var r = new Ray(0, 0, -3, 0, -MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f);
            var xs = new Intersection(MathF.Sqrt(2f), floor);
            var comps = IntersectionState.Prepare(ref xs, ref r);
            var color = w.ShadeHit(ref comps, 5);
            Assert.That.VectorsAreEqual(color, new Color(.93642f, .68642f, .68642f), 1e-4f);
        }

        [TestMethod]
        public void ShadeHitSchlick()
        {
            var w = World.Default;
            var floor = new Plane(Translation(0, -1, 0))
            {
                Material = { Transparency = .5f, IOR = 1.5f, Reflectivity = .5f }
            };
            w.Objects.Add(floor);
            var s = new Sphere(Translation(0, -3.5f, -.5f))
            {
                Material =
                {
                    BaseColor = new Color(1, 0, 0),
                    Ambient = .5f
                }
            };
            w.Objects.Add(s);
            var r = new Ray(0, 0, -3, 0, -MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f);
            var xs = new Intersection(MathF.Sqrt(2f), floor);
            var comps = IntersectionState.Prepare(ref xs, ref r);
            var color = w.ShadeHit(ref comps, 5);
            Assert.That.VectorsAreEqual(color, new Color(.93391f, .69643f, .69243f), 1e-4f);
        }
    }
}
