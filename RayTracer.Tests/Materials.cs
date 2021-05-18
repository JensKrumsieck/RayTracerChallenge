using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Environment;
using RayTracer.Materials;
using RayTracer.Materials.Patterns;
using System;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests
{
    [TestClass]
    public class Materials
    {
        PhongMaterial _m = PhongMaterial.Default;
        private Vector4 _position = Vector4.UnitW;

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
        public void EyeBeweenLightAndSurf()
        {
            var eye = Direction(0f, 0f, -1f);
            var normal = Direction(0f, 0f, -1f);
            var l = new PointLight(Point(0f, 0f, -10f), Color.White);
            var comps = new IntersectionState(_position, eye, normal);
            var res = _m.Shade(l, ref comps);
            Assert.That.VectorsAreEqual(res, new Color(1.9f, 1.9f, 1.9f));
        }

        [TestMethod]
        public void EyeOffset45()
        {
            var val = MathF.Sqrt(2f) / 2f;
            var eye = Direction(0f, val, -val);
            var normal = Direction(0f, 0f, -1f);
            var l = new PointLight(Point(0f, 0f, -10f), Color.White);
            var comps = new IntersectionState(_position, eye, normal);
            var res = _m.Shade(l, ref comps);
            Assert.That.VectorsAreEqual(res, Color.White);
        }

        [TestMethod]
        public void LightOffset45()
        {
            var eye = Direction(0f, 0, -1f);
            var normal = Direction(0f, 0f, -1f);
            var l = new PointLight(Point(0f, 10f, -10f), Color.White);
            var comps = new IntersectionState(_position, eye, normal);
            var res = _m.Shade(l, ref comps);
            Assert.That.VectorsAreEqual(res, new Color(.7364f, .7364f, .7364f));
        }

        [TestMethod]
        public void EyeInPath()
        {
            var val = -MathF.Sqrt(2f) / 2f;
            var eye = Direction(0f, val, val);
            var normal = Direction(0f, 0f, -1f);
            var l = new PointLight(Point(0f, 10f, -10f), Color.White);
            var comps = new IntersectionState(_position, eye, normal);
            var res = _m.Shade(l, ref comps);
            Assert.That.VectorsAreEqual(res, new Color(1.6364f, 1.6364f, 1.6364f), 1e-4f);
        }

        [TestMethod]
        public void LightBehindSurf()
        {
            var eye = Direction(0f, 0f, -1f);
            var normal = Direction(0f, 0f, -1f);
            var l = new PointLight(Point(0f, 0, 10f), Color.White);
            var comps = new IntersectionState(_position, eye, normal);
            var res = _m.Shade(l, ref comps);
            Assert.That.VectorsAreEqual(res, new Color(.1f, .1f, .1f));
        }

        [TestMethod]
        public void LightingWithSurfinShadow()
        {
            var eye = Direction(0f, 0f, -1f);
            var normal = Direction(0f, 0f, -1f);
            var l = new PointLight(Point(0f, 0f, -10f), Color.White);
            var comps = new IntersectionState(_position, eye, normal);
            var res = _m.Shade(l, ref comps, true);
            Assert.That.VectorsAreEqual(res, new Color(.1f, .1f, .1f));
        }

        [TestMethod]
        public void LightingWithStripePattern()
        {
            var m = PhongMaterial.Default;
            m.Pattern = new StripePattern(Color.White, Color.Black);
            m.Ambient = 1f;
            m.Diffuse = 0f;
            m.Specular = 0f;
            var eye = Direction(0f, 0f, -1f);
            var light = new PointLight(Point(0f, 0f, -10f), Color.White);
            var comps1 = new IntersectionState(Point(.9f, 0f, 0f), eye, eye);
            var comps2 = new IntersectionState(Point(1.1f, 0f, 0f), eye, eye);
            var c1 = m.Shade(light, ref comps1, false);
            var c2 = m.Shade(light, ref comps2, false);
            Assert.That.VectorsAreEqual(c1, Color.White);
            Assert.That.VectorsAreEqual(c2, Color.Black);
        }
    }
}
