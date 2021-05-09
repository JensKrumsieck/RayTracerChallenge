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
            Assert.That.ColorsAreEqual(c1 + c2, sum);
        }

        [TestMethod]
        public void SubtractColors()
        {
            var c1 = new Color(.9f, .6f, .75f);
            var c2 = new Color(.7f, .1f, .25f);
            var res = new Color(.2f, .5f, .5f);
            Assert.That.ColorsAreEqual(c1 - c2, res);
        }

        [TestMethod]
        public void ScalarMultiplyColors()
        {
            var c1 = new Color(.2f, .3f, .4f);
            var res = new Color(.4f, .6f, .8f);
            Assert.That.ColorsAreEqual(c1 * 2, res);
        }
    }
}
