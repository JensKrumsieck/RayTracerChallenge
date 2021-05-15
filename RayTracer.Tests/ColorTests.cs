using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;

namespace RayTracer.Tests
{
    [TestClass]
    public class ColorTests
    {
        [TestMethod]
        public void AddColors()
        {
            var c1 = new Color(.9f, .6f, .75f);
            var c2 = new Color(.7f, .1f, .25f);
            var sum = new Color(1.6f, .7f, 1f);
            Assert.That.ColorsAreEqual(c1 + c2, sum, 1e-6f);
        }

        [TestMethod]
        public void SubtractColors()
        {
            var c1 = new Color(.9f, .6f, .75f);
            var c2 = new Color(.7f, .1f, .25f);
            var res = new Color(.2f, .5f, .5f);
            Assert.That.ColorsAreEqual(c1 - c2, res, 1e-6f);
        }

        [TestMethod]
        public void ScalarMultiplyColors()
        {
            var c1 = new Color(.2f, .3f, .4f);
            var res = new Color(.4f, .6f, .8f);
            Assert.That.ColorsAreEqual(c1 * 2, res, 1e-6f);
        }

        [TestMethod]
        public void MultiplyColors()
        {
            var c1 = new Color(1f, .2f, .4f);
            var c2 = new Color(.9f, 1f, .1f);
            var res = new Color(.9f, .2f, .04f);
            Assert.That.ColorsAreEqual(c1 * c2, res, 1e-6f);
        }
    }
}
