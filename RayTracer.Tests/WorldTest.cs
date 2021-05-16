using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Environment;
using RayTracer.Materials;
using RayTracer.Shapes;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests
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
                Material = new PhongMaterial(new Color(.8f, 1f, .6f), .7f, .2f)
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
            var xs = w.Intersect(ray);
            Assert.AreEqual(xs.Length, 4);
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
            var comps = IntersectionState.Prepare(i, r);
            var c = w.ShadeHit(comps);
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
            var comps = IntersectionState.Prepare(i, r);
            var c = w.ShadeHit(comps);
            Assert.That.VectorsAreEqual(c, new Color(.90498f, .90498f, .90498f));
        }

        [TestMethod]
        public void ColorRayMisses()
        {
            var w = World.Default;
            var r = new Ray(0f, 0f, -5f, 0f, 1f, 0f);
            var c = w.ColorAt(r);
            Assert.That.VectorsAreEqual(c, Color.Black);
        }

        [TestMethod]
        public void ColorRayHits()
        {
            var w = World.Default;
            var r = new Ray(0f, 0f, -5f, 0f, 0f, 1f);
            var c = w.ColorAt(r);
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
            var c = w.ColorAt(r);
            Assert.That.VectorsAreEqual(c, inner.Material.BaseColor);
        }

        [TestMethod]
        public void NoShadowWhenNothingCollinear()
        {
            var w = World.Default;
            var p = Point(0f, 10f, 0f);
            Assert.IsFalse(w.InShadow(p));
        }

        [TestMethod]
        public void ShadowWhenObjectBetweenPointLight()
        {
            var w = World.Default;
            var p = Point(10f, -10f, 10f);
            Assert.IsTrue(w.InShadow(p));
        }

        [TestMethod]
        public void NoShadowWhenBehindPoint()
        {
            var w = World.Default;
            var p = Point(-2f, 2f, -2f);
            Assert.IsFalse(w.InShadow(p));
        }
    }
}
