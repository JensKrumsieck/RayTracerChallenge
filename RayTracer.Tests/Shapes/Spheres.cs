using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Materials;
using RayTracer.Shapes;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests.Shapes
{
    [TestClass]
    public class Spheres
    {
        [TestMethod]
        public void RayIntersectsSphereAtTwoPoints()
        {
            var r = new Ray(0f, 0f, -5f, 0f, 0f, 1f);
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, 4.0f);
            Assert.AreEqual(xs[1].Distance, 6.0f);
        }

        [TestMethod]
        public void RayIntersectsSphereAtTangent()
        {
            var r = new Ray(0f, 1f, -5f, 0f, 0f, 1f);
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, 5.0f);
            Assert.AreEqual(xs[1].Distance, 5.0f);
        }

        [TestMethod]
        public void RayMissesSphere()
        {
            var r = new Ray(0f, 2f, -5f, 0f, 0f, 1f);
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 0);
        }

        [TestMethod]
        public void RayInsideSphere()
        {
            var r = new Ray(0f, 0f, 0f, 0f, 0f, 1f);
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, -1.0f);
            Assert.AreEqual(xs[1].Distance, 1.0f);
        }

        [TestMethod]
        public void RayWithSphereBehind()
        {
            var r = new Ray(0f, 0f, 5f, 0f, 0f, 1f);
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, -6.0f);
            Assert.AreEqual(xs[1].Distance, -4.0f);
        }

        [TestMethod]
        public void IntersectionCheckObject()
        {
            var r = new Ray(0f, 0f, -5f, 0f, 0f, 1f);
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Object, s);
            Assert.AreEqual(xs[1].Object, s);
        }

        [TestMethod]
        public void DefaultTransform()
        {
            var s = new Sphere();
            Assert.AreEqual(s.Transform, Transform.Identity);
        }

        [TestMethod]
        public void ChangingTransform()
        {
            var s = new Sphere(Translation(2f, 3f, 4f));
            Assert.AreEqual(s.Transform.Matrix, Translation(2f, 3f, 4f));
        }

        [TestMethod]
        public void IntersectScaledSphere()
        {
            var r = new Ray(Point(0f, 0f, -5f), Direction(0f, 0f, 1f));
            var s = new Sphere(Scale(2f, 2f, 2f));
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Distance, 3f);
            Assert.AreEqual(xs[1].Distance, 7f);
        }

        [TestMethod]
        public void IntersectTranslatedSphere()
        {
            var r = new Ray(Point(0f, 0f, -5f), Direction(0f, 0f, 1f));
            var s = new Sphere(Translation(5f, 0f, 0f));
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 0);
        }

        [TestMethod]
        public void NormalOnSphereX()
        {
            var s = new Sphere();
            var n = s.LocalNormal(Point(1f, 0f, 0f));
            Assert.That.VectorsAreEqual(n, Direction(1f, 0f, 0f));
        }

        [TestMethod]
        public void NormalOnSphereY()
        {
            var s = new Sphere();
            var n = s.LocalNormal(Point(0f, 1f, 0f));
            Assert.That.VectorsAreEqual(n, Direction(0f, 1f, 0f));
        }

        [TestMethod]
        public void NormalOnSphereZ()
        {
            var s = new Sphere();
            var n = s.LocalNormal(Point(0f, 0f, 1f));
            Assert.That.VectorsAreEqual(n, Direction(0f, 0f, 1f));
        }

        [TestMethod]
        public void NormalOnSphereNonAxial()
        {
            var s = new Sphere();
            var val = MathF.Sqrt(3f) / 3f;
            var n = s.LocalNormal(Point(val, val, val));
            Assert.That.VectorsAreEqual(n, Direction(val, val, val));
            Assert.That.VectorsAreEqual(n, Vector4.Normalize(n));
        }

        [TestMethod]
        public void NormalOnTranslatedSphere()
        {
            var s = new Sphere(Translation(0f, 1f, 0f));
            var n = s.Normal(Point(0f, 1.70711f, -.70711f));
            Assert.That.VectorsAreEqual(n, Direction(0f, .70711f, -.70711f));
        }

        [TestMethod]
        public void NormalOnTransformedSphere()
        {
            var s = new Sphere(Scale(1f, .5f, 1f) * RotationZ(MathF.PI / 5f));
            var val = MathF.Sqrt(2f) / 2f;
            var n = s.Normal(Point(0f, val, -val));
            Assert.That.VectorsAreEqual(n, Direction(0f, .97014f, -.24254f));
        }

        [TestMethod]
        public void SphereHasMaterial()
        {
            var s = new Sphere();
            Assert.AreEqual(s.Material, PhongMaterial.Default);
        }

        [TestMethod]
        public void SphereCanGetMaterial()
        {
            var s = new Sphere();
            Assert.AreEqual(s.Material, PhongMaterial.Default);
            var mat = s.Material;
            mat.Ambient = 1f;
            s.Material = mat;
            Assert.AreEqual(s.Material, mat);
            Assert.AreEqual(s.Material.Ambient, 1.0f);
        }
    }
}
