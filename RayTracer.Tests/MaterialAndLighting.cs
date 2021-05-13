using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using RayTracer.Engine.Lighting;
using RayTracer.Engine.Material;
using RayTracer.Primitives;
using System;
using System.Numerics;

namespace RayTracer.Tests
{
    [TestClass]
    public class MaterialAndLighting
    {
        [TestMethod]
        public void DefaultMaterial()
        {
            var m = PhongMaterial.DefaultMaterial;
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
            var s = new Sphere { Material = { Ambient = 1f } };
            Assert.AreEqual(s.Material.Ambient, 1f);
        }

        private static (PhongMaterial m, Vector3 p) Setup => (PhongMaterial.DefaultMaterial, Vector3.Zero);

        [TestMethod]
        public void EyeBetween()
        {
            var (m, position) = Setup;
            var eye = -Vector3.UnitZ;
            var light = new PointLight(Vector3.UnitZ * -10, Color.White);
            var result = m.Lighten(light, position, eye, eye);
            Assert.AreEqual(result, new Color(1.9f, 1.9f, 1.9f));
        }

        [TestMethod]
        public void EyeBehind()
        {
            var (m, position) = Setup;
            var eye = -Vector3.UnitZ;
            var light = new PointLight(Vector3.UnitZ * 10, Color.White);
            var result = m.Lighten(light, position, eye, eye);
            Assert.AreEqual(result, new Color(.1f, .1f, .1f));
        }

        [TestMethod]
        public void EyeOffset45()
        {
            var (m, position) = Setup;
            var eye = new Vector3(0, MathF.Sqrt(2f) / 2f, MathF.Sqrt(2f) / 2f);
            var normal = -Vector3.UnitZ;
            var light = new PointLight(Vector3.UnitZ * -10, Color.White);
            var result = m.Lighten(light, position, eye, normal);
            Assert.AreEqual(result, Color.White);
        }

        [TestMethod]
        public void LightOffset45()
        {
            var (m, position) = Setup;
            var eye = -Vector3.UnitZ;
            var normal = -Vector3.UnitZ;
            var light = new PointLight(new Vector3(0f, 10f, -10f), Color.White);
            var result = m.Lighten(light, position, eye, normal);
            Assert.That.ColorsAreEqual(result, new Color(.7364f, .7364f, .7364f), 1e-4f);
        }

        [TestMethod]
        public void EyeInPath()
        {
            var (m, position) = Setup;
            var eye = new Vector3(0, -MathF.Sqrt(2f) / 2f, -MathF.Sqrt(2f) / 2f);
            var normal = -Vector3.UnitZ;
            var light = new PointLight(new Vector3(0f, 10f, -10f), Color.White);
            var result = m.Lighten(light, position, eye, normal);
            Assert.That.ColorsAreEqual(result, new Color(1.6364f, 1.6364f, 1.6364f), 1e-4f);
        }
    }
}
