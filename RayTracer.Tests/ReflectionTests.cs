using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace RayTracer.Tests
{
    [TestClass]
    public class ReflectionTests
    {
        [TestMethod]
        public void ReflectingAVector()
        {
            var v = new Vector3(1f, -1f, 0f);
            var n = Vector3.UnitY;
            var reflect = Vector3.Reflect(v, n);
            Assert.AreEqual(reflect, new Vector3(1f, 1f, 0f));
        }

        [TestMethod]
        public void ReflectingSlanted()
        {
            var v = new Vector3(0f, -1f, 0f);
            var s = MathF.Sqrt(2f);
            var n = new Vector3(s / 2f, s / 2f, 0f);
            var reflect = Vector3.Reflect(v, n);
            Assert.AreEqual(reflect.X, 1f, 1e-5f);
            Assert.AreEqual(reflect.Y, 0f, 1e-5f);
            Assert.AreEqual(reflect.Y, 0f, 1e-5f);
        }
    }
}
