using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using System;

namespace RayTracer.Tests
{
    [TestClass]
    public class ViewportTests
    {
        [TestMethod]
        public void EveryColorIsBlack()
        {
            var v = new Viewport(10, 20);
            Assert.AreEqual(10, v.Width);
            Assert.AreEqual(20, v.Height);
            CollectionAssert.That.AllItemsAre(v.GetPixels(), Color.Black);
        }

        [TestMethod]
        public void SetPixels()
        {
            var v = new Viewport(10, 20);
            v.SetPixel(2, 3, Color.Red);
            Assert.That.ColorsAreEqual(v.PixelAt(2, 3), Color.Red);
        }

        [TestMethod]
        public void TestPPM()
        {
            var v = new Viewport(5, 3);
            var c1 = new Color(1.5f, 0f, 0f);
            var c2 = new Color(0f, .5f, 0f);
            var c3 = new Color(-.5f, 0f, 1f);
            v.SetPixel(0, 0, c1);
            v.SetPixel(2, 1, c2);
            v.SetPixel(4, 2, c3);
            var expected = string.Join(Environment.NewLine,
                "P3",
                "5 3",
                "255",
                "255 0 0 0 0 0 0 0 0 0 0 0 0 0 0",
                "0 0 0 0 0 0 0 128 0 0 0 0 0 0 0",
                "0 0 0 0 0 0 0 0 0 0 0 0 0 0 255",
                "");
            Assert.AreEqual(expected, v.ToPixmap());
        }

        [TestMethod]
        public void LinesCanNotBeLongerThan70()
        {
            var v = new Viewport(10, 2);
            v.SetAll(new Color(1f, .8f, .6f));
            var expected = string.Join(Environment.NewLine,
                "P3",
                "10 2",
                "255",
                "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204",
                "153 255 204 153 255 204 153 255 204 153 255 204 153",
                "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204",
                "153 255 204 153 255 204 153 255 204 153 255 204 153",
                "");
            Assert.AreEqual(expected, v.ToPixmap());
        }
    }
}
