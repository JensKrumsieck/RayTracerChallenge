using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Materials.Patterns;
using RayTracer.Shapes;
using RayTracer.Tests.TestObjects;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests
{
    [TestClass]
    public class Patterns
    {
        [TestMethod]
        public void StripePattern()
        {
            var p = new StripePattern(Color.White, Color.Black);
            Assert.That.VectorsAreEqual(p.A, Color.White);
            Assert.That.VectorsAreEqual(p.B, Color.Black);
        }

        [TestMethod]
        public void StripePatternConstantInY()
        {
            var p = new StripePattern(Color.White, Color.Black);
            Assert.That.VectorsAreEqual(p.At(Point(0f, 0f, 0f)), Color.White);
            Assert.That.VectorsAreEqual(p.At(Point(0f, 1f, 0f)), Color.White);
            Assert.That.VectorsAreEqual(p.At(Point(0f, 2f, 0f)), Color.White);
        }

        [TestMethod]
        public void StripePatternConstantInZ()
        {
            var p = new StripePattern(Color.White, Color.Black);
            Assert.That.VectorsAreEqual(p.At(Point(0f, 0f, 0f)), Color.White);
            Assert.That.VectorsAreEqual(p.At(Point(0f, 0f, 1f)), Color.White);
            Assert.That.VectorsAreEqual(p.At(Point(0f, 0f, 2f)), Color.White);
        }

        [TestMethod]
        public void StripePatternAlternatesInZ()
        {
            var p = new StripePattern(Color.White, Color.Black);
            Assert.That.VectorsAreEqual(p.At(Point(1f, 0f, 0f)), Color.Black);
            Assert.That.VectorsAreEqual(p.At(Point(.9f, 0f, 0f)), Color.White);
            Assert.That.VectorsAreEqual(p.At(Point(0f, 0f, 0f)), Color.White);
            Assert.That.VectorsAreEqual(p.At(Point(-.1f, 0f, 0f)), Color.Black);
            Assert.That.VectorsAreEqual(p.At(Point(-1.1f, 0f, 0f)), Color.White);
        }

        [TestMethod]
        public void StripesWithTransformation()
        {
            var s = new Sphere(Scale(2f));
            var p = new StripePattern(Color.White, Color.Black);
            var c = p.ColorAtEntity(s, Point(1.5f, 0f, 0f));
            Assert.That.VectorsAreEqual(c, Color.White);
        }

        [TestMethod]
        public void StripesWithTransformedPattern()
        {
            var s = new Sphere();
            var p = new StripePattern(Color.White, Color.Black, Scale(2f));
            var c = p.ColorAtEntity(s, Point(1.5f, 0f, 0f));
            Assert.That.VectorsAreEqual(c, Color.White);
        }
        [TestMethod]
        public void StripesEverythingTransformed()
        {
            var s = new Sphere(Scale(2f));
            var p = new StripePattern(Color.White, Color.Black, Scale(2f));
            var c = p.ColorAtEntity(s, Point(1.5f, 0f, 0f));
            Assert.That.VectorsAreEqual(c, Color.White);
        }

        [TestMethod]
        public void PatternTransform()
        {
            var p = new TestPattern();
            Assert.That.MatricesAreEqual(p.Transform, Transform.Identity);
        }

        [TestMethod]
        public void AssignTransform()
        {
            var p = new TestPattern { Transform = Translation(1f, 2f, 3f) };
            Assert.That.MatricesAreEqual(p.Transform.Matrix, Translation(1f, 2f, 3f));
        }

        [TestMethod]
        public void PatternWithObjectTransform()
        {
            var p = new TestPattern();
            var s = new Sphere(Scale(2f));
            var c = p.ColorAtEntity(s, Point(2f, 3f, 4f));
            Assert.That.VectorsAreEqual(c, new Color(1f, 1.5f, 2f));
        }
        [TestMethod]
        public void PatternWithTransform()
        {
            var p = new TestPattern { Transform = Scale(2f) };
            var s = new Sphere();
            var c = p.ColorAtEntity(s, Point(2f, 3f, 4f));
            Assert.That.VectorsAreEqual(c, new Color(1f, 1.5f, 2f));
        }

        [TestMethod]
        public void PatternWithBothTransformed()
        {
            var p = new TestPattern { Transform = Translation(.5f, 1f, 1.5f) };
            var s = new Sphere(Scale(2f));
            var c = p.ColorAtEntity(s, Point(2.5f, 3f, 3.5f));
            Assert.That.VectorsAreEqual(c, new Color(.75f, .5f, .25f));
        }

        [TestMethod]
        public void GradientPattern()
        {
            var p = new GradientPattern(Color.White, Color.Black);
            Assert.That.VectorsAreEqual(p.At(Point(0f, 0f, 0f)), Color.White);
            Assert.That.VectorsAreEqual(p.At(Point(.25f, 0f, 0f)), new Color(.75f, .75f, .75f));
            Assert.That.VectorsAreEqual(p.At(Point(.5f, 0f, 0f)), new Color(.5f, .5f, .5f));
            Assert.That.VectorsAreEqual(p.At(Point(.75f, 0f, 0f)), new Color(.25f, .25f, .25f));
        }
    }
}
