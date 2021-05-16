using Microsoft.VisualStudio.TestTools.UnitTesting;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;
using static System.MathF;

namespace RayTracer.Tests.Numerics
{
    [TestClass]
    public class Transformations
    {
        [TestMethod]
        public void MultiplyByTranslationMatrix()
        {
            var t = Translation(5f, -3f, 2f);
            var p = Point(-3f, 4f, 5f);
            Assert.That.VectorsAreEqual(t.Multiply(p), Point(2f, 1f, 7f));
        }

        [TestMethod]
        public void MultiplyByTranslationMatrixInverse()
        {
            var t = Translation(5f, -3f, 2f);
            var p = Point(-3f, 4f, 5f);
            Assert.That.VectorsAreEqual(t.Inverse().Multiply(p), Point(-8f, 7f, 3f));
        }

        [TestMethod]
        public void VectorsNotAffectedByTranslation()
        {
            var t = Translation(5f, -3f, 2f);
            var v = Direction(-3f, 4f, 5f);
            Assert.That.VectorsAreEqual(v, t.Multiply(v));
        }

        [TestMethod]
        public void MultiplyByScaleMatrix()
        {
            var t = Scale(2f, 3f, 4f);
            var p = Point(-4f, 6f, 8f);
            Assert.That.VectorsAreEqual(t.Multiply(p), Point(-8, 18, 32));
        }

        [TestMethod]
        public void VectorsAffectedByScaling()
        {
            var t = Scale(2f, 3f, 4f);
            var v = Direction(-4f, 6f, 8f);
            Assert.That.VectorsAreEqual(t.Multiply(v), Direction(-8, 18, 32));
        }

        [TestMethod]
        public void MultiplyByScaleMatrixInverse()
        {
            var t = Scale(2f, 3f, 4f);
            var v = Direction(-4f, 6f, 8f);
            Assert.That.VectorsAreEqual(t.Inverse().Multiply(v), Direction(-2f, 2f, 2f));
        }

        [TestMethod]
        public void ReflectionIsScalingByNegative()
        {
            var t = Scale(-1f, 1f, 1f);
            var p = Point(2f, 3f, 4f);
            Assert.AreEqual(t.Multiply(p), Point(-2f, 3f, 4f));
        }

        [TestMethod]
        public void MultiplyByRotationMatrixX()
        {
            var p = Point(0f, 1f, 0f);
            var halfQuarter = RotationX(PI / 4f);
            var fullQuarter = RotationX(PI / 2f);
            Assert.That.VectorsAreEqual(halfQuarter.Multiply(p), Point(0f, Sqrt(2f) / 2f, Sqrt(2f) / 2f));
            Assert.That.VectorsAreEqual(fullQuarter.Multiply(p), Point(0f, 0f, 1f));
        }

        [TestMethod]
        public void MultiplyByRotationMatrixXInverse()
        {
            var p = Point(0f, 1f, 0f);
            var t = RotationX(PI / 4f);
            Assert.That.VectorsAreEqual(t.Inverse().Multiply(p), Point(0f, Sqrt(2f) / 2f, -Sqrt(2f) / 2f));
        }

        [TestMethod]
        public void MultiplyByRotationMatrixY()
        {
            var p = Point(0f, 0f, 1f);
            var halfQuarter = RotationY(PI / 4f);
            var fullfQuarter = RotationY(PI / 2f);
            Assert.That.VectorsAreEqual(halfQuarter.Multiply(p), Point(Sqrt(2f) / 2f, 0f, Sqrt(2f) / 2f));
            Assert.That.VectorsAreEqual(fullfQuarter.Multiply(p), Point(1f, 0f, 0f));
        }

        [TestMethod]
        public void MultiplyByRotationMatrixZ()
        {
            var p = Point(0f, 1f, 0f);
            var halfQuarter = RotationZ(PI / 4f);
            var fullfQuarter = RotationZ(PI / 2f);
            Assert.That.VectorsAreEqual(halfQuarter.Multiply(p), Point(-Sqrt(2f) / 2f, Sqrt(2f) / 2f, 0f));
            Assert.That.VectorsAreEqual(fullfQuarter.Multiply(p), Point(-1f, 0f, 0f));
        }

        [TestMethod]
        public void MultiplyByShearMatrix()
        {
            var p = Point(2f, 3f, 4f);
            var xy = Shear(1f, 0f, 0f, 0f, 0f, 0f);
            var xz = Shear(0f, 1f, 0f, 0f, 0f, 0f);
            var yx = Shear(0f, 0f, 1f, 0f, 0f, 0f);
            var yz = Shear(0f, 0f, 0f, 1f, 0f, 0f);
            var zx = Shear(0f, 0f, 0f, 0f, 1f, 0f);
            var zy = Shear(0f, 0f, 0f, 0f, 0f, 1f);
            Assert.That.VectorsAreEqual(xy.Multiply(p), Point(5f, 3f, 4f));
            Assert.That.VectorsAreEqual(xz.Multiply(p), Point(6f, 3f, 4f));
            Assert.That.VectorsAreEqual(yx.Multiply(p), Point(2f, 5f, 4f));
            Assert.That.VectorsAreEqual(yz.Multiply(p), Point(2f, 7f, 4f));
            Assert.That.VectorsAreEqual(zx.Multiply(p), Point(2f, 3f, 6f));
            Assert.That.VectorsAreEqual(zy.Multiply(p), Point(2f, 3f, 7f));
        }

        [TestMethod]
        public void ChainingTransformations()
        {
            var p = Point(1f, 0f, 1f);
            var a = RotationX(PI / 2f);
            var b = Scale(5f, 5f, 5f);
            var c = Translation(10f, 5f, 7f);
            var p2 = a.Multiply(p);
            Assert.That.VectorsAreEqual(p2, Point(1f, -1f, 0f));
            var p3 = b.Multiply(p2);
            Assert.That.VectorsAreEqual(p3, Point(5f, -5f, 0f));
            var p4 = c.Multiply(p3);
            Assert.That.VectorsAreEqual(p4, Point(15f, 0f, 7f));

            var t = c * b * a;
            Assert.That.VectorsAreEqual(t.Multiply(p), Point(15f, 0f, 7f));
        }
    }
}
