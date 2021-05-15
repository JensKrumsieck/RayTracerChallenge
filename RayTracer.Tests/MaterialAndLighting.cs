using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using RayTracer.Engine.Lighting;
using RayTracer.Engine.Material;
using RayTracer.Extension;
using RayTracer.Primitives;
using System;
using System.Numerics;

namespace RayTracer.Tests
{
    [TestClass]
    public class MaterialShadowingAndLighting
    {
        [TestMethod]
        public void DefaultMaterial()
        {
            var m = PhongMaterial.Default;
            Assert.AreEqual(m.BaseColor, Color.White);
            Assert.AreEqual(m.Ambient, .1f);
            Assert.AreEqual(m.Diffuse, .9f);
            Assert.AreEqual(m.Specular, .9f);
            Assert.AreEqual(m.Shininess, 200f);
        }

        [TestMethod]
        public void PointLight()
        {
            var light = new PointLight(Vector3.Zero, Color.White);
            Assert.AreEqual(light.Position, Vector3.Zero);
            Assert.AreEqual(light.Intensity, Color.White);
        }

        [TestMethod]
        public void SphereHasMaterial()
        {
            var mat = PhongMaterial.Default.WithAmbient(1.0f);
            var s = new Sphere { Material = mat };
            Assert.AreEqual(((PhongMaterial)s.Material).Ambient, 1f);
        }

        private static (PhongMaterial m, Vector3 p) Setup => (PhongMaterial.Default, Vector3.Zero);

        [TestMethod]
        public void EyeBetween()
        {
            var (m, position) = Setup;
            var eye = -Vector3.UnitZ;
            var light = new PointLight(Vector3.UnitZ * -10, Color.White);
            var result = m.Shade(light, new IntersectionPoint { HitPoint = position, Eye = eye, Normal = eye });
            Assert.AreEqual(result, new Color(1.9f, 1.9f, 1.9f));
        }

        [TestMethod]
        public void EyeBehind()
        {
            var (m, position) = Setup;
            var eye = -Vector3.UnitZ;
            var light = new PointLight(Vector3.UnitZ * 10, Color.White);
            var result = m.Shade(light, new IntersectionPoint { HitPoint = position, Eye = eye, Normal = eye });
            Assert.AreEqual(result, new Color(.1f, .1f, .1f));
        }

        [TestMethod]
        public void EyeOffset45()
        {
            var (m, position) = Setup;
            var eye = new Vector3(0, MathF.Sqrt(2f) / 2f, -MathF.Sqrt(2f) / 2f);
            var normal = -Vector3.UnitZ;
            var light = new PointLight(Vector3.UnitZ * -10, Color.White);
            var result = m.Shade(light, new IntersectionPoint { HitPoint = position, Eye = eye, Normal = normal });
            Assert.AreEqual(result, Color.White);
        }

        [TestMethod]
        public void LightOffset45()
        {
            var (m, position) = Setup;
            var eye = -Vector3.UnitZ;
            var normal = -Vector3.UnitZ;
            var light = new PointLight(new Vector3(0f, 10f, -10f), Color.White);
            var result = m.Shade(light, new IntersectionPoint { HitPoint = position, Eye = eye, Normal = normal });
            Assert.That.ColorsAreEqual(result, new Color(.7364f, .7364f, .7364f), 1e-4f);
        }

        [TestMethod]
        public void EyeInPath()
        {
            var (m, position) = Setup;
            var eye = new Vector3(0, -MathF.Sqrt(2f) / 2f, -MathF.Sqrt(2f) / 2f);
            var normal = -Vector3.UnitZ;
            var light = new PointLight(new Vector3(0f, 10f, -10f), Color.White);
            var result = m.Shade(light, new IntersectionPoint { HitPoint = position, Eye = eye, Normal = normal });
            Assert.That.ColorsAreEqual(result, new Color(1.6364f, 1.6364f, 1.6364f), 1e-4f);
        }

        [TestMethod]
        public void LightWithSurfInShadow()
        {
            var (m, position) = Setup;
            var eyeV = new Vector3(0f, 0f, -1f);
            var normalV = new Vector3(0f, 0f, -1f);
            var light = new PointLight(Vector3.UnitZ * -10, Color.White);
            var res = m.Shade(light,
                new IntersectionPoint {Eye = eyeV, HitPoint = position, Normal = normalV, Object = null}, true);
            Assert.AreEqual(res, new Color(.1f, .1f, .1f));
        }

        [TestMethod]
        public void NoShadowWhenCollinearWithPointAndLight()
        {
            var w = World.Default;
            var p = new Vector3(0f, 10f, 0f);
            Assert.IsFalse(w.ShadowCheck(p));
        }

        [TestMethod]
        public void ShadowObjectBetweenPointAndLight()
        {
            var w = World.Default;
            var p = new Vector3(10f, -10f, 10f);
            Assert.IsTrue(w.ShadowCheck(p));
        }

        [TestMethod]
        public void NoShadowObjectBehindLight()
        {
            var w = World.Default;
            var p = new Vector3(-20f, 20f, -20f);
            Assert.IsFalse(w.ShadowCheck(p));
        }

        [TestMethod]
        public void NoShadowObjectBehindPoint()
        {
            var w = World.Default;
            var p = new Vector3(-2f, 2f, -2f);
            Assert.IsFalse(w.ShadowCheck(p));
        }

        [TestMethod]
        public void ShadeHitIntersection()
        {
            var w = new World
            {
                Light = new PointLight(new Vector3(0f, 0f, -10f), new Color(1f, 1f, 1f))
            };
            var s1 = new Sphere();
            var s2 = new Sphere {TransformationMatrix = Matrix.TranslationMatrix(0f, 0f, 10f)};
            w.Objects = new Transform[] {s1, s2};
            var r = new Ray(new Vector3(0f, 0f, 5f), Vector3.UnitZ);
            var i = new HitInfo(4, s2);
            var comps = IntersectionPoint.Prepare(i, r);
            var col = w.Shade(comps);
            Assert.AreEqual(col, new Color(.1f,.1f,.1f));
        }
    }
}
