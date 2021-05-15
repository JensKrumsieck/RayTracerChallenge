using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using RayTracer.Extension;
using RayTracer.Primitives;
using System.Numerics;

namespace RayTracer.Tests
{
    [TestClass]
    public class SphereTests
    {
        [TestMethod]
        public void DefaultTransformation()
        {
            var s = new Sphere();
            Assert.AreEqual(s.TransformationMatrix, Matrix4x4.Identity);
        }

        [TestMethod]
        public void ChangeTransformation()
        {
            var s = new Sphere();
            Assert.AreEqual(s.TransformationMatrix, Matrix4x4.Identity);
            s.TransformationMatrix = Matrix.TranslationMatrix(2f, 3f, 4f);
            Assert.AreEqual(s.Position, new Vector3(2f, 3f, 4f));
        }

        [TestMethod]
        public void IntersectScaledSphere()
        {
            var s = new Sphere { TransformationMatrix = Matrix.ScaleMatrix(2f, 2f, 2f) };
            Ray.Intersect(new Vector3(0f, 0f, -5f), Vector3.UnitZ, s, out var hits);
            Assert.AreEqual(hits.Length, 2);
            Assert.AreEqual(hits[0].Distance, 3f);
            Assert.AreEqual(hits[1].Distance, 7f);
        }

        [TestMethod]
        public void IntersectTranslatedSphere()
        {
            var s = new Sphere { TransformationMatrix = Matrix.TranslationMatrix(5f, 0f, 0f) };
            Ray.Intersect(new Vector3(0f, 0f, -5f), Vector3.UnitZ, s, out var hits);
            Assert.AreEqual(hits.Length, 0);
        }
    }
}
