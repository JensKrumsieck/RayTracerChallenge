using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;

namespace RayTracer.Tests
{
    [TestClass]
    public class TransformationTest
    {
        [TestMethod]
        public void MultiplyByTranslation()
        {
            var p = Vector3.Point(-3, 4, 5);
            var res = Vector3.Point(2, 1, 7);
            Assert.AreEqual(p.Translate(5, -3, 2), res);
        }

        [TestMethod]
        public void TranslationNotAffectingVectors()
        {
            var v = Vector3.Vector(-3, 4, 5);
            Assert.AreEqual(v.Translate(-3,4,5), v);
        }

        [TestMethod]
        public void InverseTranslation()
        {
            var mat = Matrix.Translation(5, -3, 2);
            var inv = mat.Inverse();
            var p = Vector3.Point(-3, 4, 5);
            var res = Vector3.Point(-8, 7, 3);
            Assert.AreEqual(inv * p, res);
        }

        [TestMethod]
        public void ScaleTest()
        {
            var p = Vector3.Point(-4, 6, 8);
            var res = Vector3.Point(-8, 18, 32);
            Assert.AreEqual(p.Scale(2,3,4), res);
        }
    }
}
