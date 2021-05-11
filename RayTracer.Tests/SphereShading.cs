using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Primitives;
using System;
using System.Numerics;
using RayTracer.Engine;
using Vector = RayTracer.Engine.Vector;

namespace RayTracer.Tests
{
    [TestClass]
    public class SphereShading
    {
        [TestMethod]
        public void NormalX()
        {
            var s = new Sphere();
            var n = s.Normal(Vector3.UnitX);
            Assert.AreEqual(n, Vector3.UnitX);
        }

        [TestMethod]
        public void NormalY()
        {
            var s = new Sphere();
            var n = s.Normal(Vector3.UnitY);
            Assert.AreEqual(n, Vector3.UnitY);
        }

        [TestMethod]
        public void NormalZ()
        {
            var s = new Sphere();
            var n = s.Normal(Vector3.UnitZ);
            Assert.AreEqual(n, Vector3.UnitZ);
        }

        [TestMethod]
        public void NormalNonAxial()
        {
            var s = new Sphere();
            var sqrt = MathF.Sqrt(3f) / 3f;
            var n = s.Normal(new Vector3(sqrt, sqrt, sqrt));
            Assert.IsTrue(n.LengthSquared()-1f < Constants.Epsilon);
        }

        [TestMethod]
        public void NormalTranslated()
        {
            var s = new Sphere(new Vector3(0f, 1f, 0f));
            const float val = .70711f;
            var n = s.Normal(new Vector3(0, 1f + val, val));
            Assert.AreEqual(n, new Vector3(0, val, -val));
        }
    }
}
