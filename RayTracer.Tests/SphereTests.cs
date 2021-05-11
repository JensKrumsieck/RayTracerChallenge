using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using RayTracer.Primitives;

namespace RayTracer.Tests
{
    [TestClass]
    public class SphereTests
    {
        [TestMethod]
        public void DefaultTransformation()
        {
            var s = new Sphere();
            Assert.AreEqual(s.TransformationMatrix, Matrix.Identity(4,4));
        }

        [TestMethod]
        public void ChangeTransformation()
        {
            var s = new Sphere();
            Assert.AreEqual(s.TransformationMatrix, Matrix.Identity(4, 4));
            s.TransformationMatrix = Matrix.Translation(2f,3f,4f);
            Assert.AreEqual(s.Position, Vector3.Point(2f, 3f, 4f));
        }

        [TestMethod]
        public void IntersectScaledSphere()
        {
            var s = new Sphere {TransformationMatrix = Matrix.Scale(2f, 2f, 2f)};
            Ray.Intersect(Vector3.Point(0f, 0f, -5f), Vector3.UnitZ, s, out var hits);
            Assert.AreEqual(hits.Length, 2);
            Assert.AreEqual(hits[0].Distance, 3f);
            Assert.AreEqual(hits[1].Distance, 7f);
        }

        [TestMethod]
        public void IntersectTranslatedSphere()
        {
            var s = new Sphere { TransformationMatrix = Matrix.Translation(5f, 0f, 0f) };
            Ray.Intersect(Vector3.Point(0f, 0f, -5f), Vector3.UnitZ, s, out var hits);
            Assert.AreEqual(hits.Length, 0);
        }

        [TestMethod]
        public void NativeDefaultTransformation()
        {
            var s = new NativeSphere();
            Assert.AreEqual(s.TransformationMatrix, System.Numerics.Matrix4x4.Identity);
        }

        [TestMethod]
        public void NativeChangeTransformation()
        {
            var s = new NativeSphere();
            Assert.AreEqual(s.TransformationMatrix, System.Numerics.Matrix4x4.Identity);
            s.TransformationMatrix = System.Numerics.Matrix4x4.CreateTranslation(2f, 3f, 4f);
            Assert.AreEqual(s.Position, new System.Numerics.Vector3(2f, 3f, 4f));
        }

        [TestMethod]
        public void NativeIntersectScaledSphere()
        {
            var s = new NativeSphere { TransformationMatrix = System.Numerics.Matrix4x4.CreateScale(2f, 2f, 2f) };
            NativeRay.Intersect(new System.Numerics.Vector3(0f, 0f, -5f), System.Numerics.Vector3.UnitZ, s, out var hits);
            Assert.AreEqual(hits.Length, 2);
            Assert.AreEqual(hits[0].Distance, 3f);
            Assert.AreEqual(hits[1].Distance, 7f);
        }

        [TestMethod]
        public void NativeIntersectTranslatedSphere()
        {
            var s = new NativeSphere { TransformationMatrix = System.Numerics.Matrix4x4.CreateTranslation(5f, 0f, 0f) };
            NativeRay.Intersect(new System.Numerics.Vector3(0f, 0f, -5f), System.Numerics.Vector3.UnitZ, s, out var hits);
            Assert.AreEqual(hits.Length, 0);
        }
    }
}
