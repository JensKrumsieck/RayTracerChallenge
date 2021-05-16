using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Drawing;
using System.Linq;
using static System.Environment;

namespace RayTracer.Tests.Drawing
{
    [TestClass]
    public class CanvasTest
    {
        [TestMethod]
        public void CreateCanvas()
        {
            var c = new Canvas(10, 20);
            Assert.AreEqual(c.Size.X, 10);
            Assert.AreEqual(c.Size.Y, 20);
            for (var y = 0; y < c.Size.Y; y++)
            {
                for (var x = 0; x < c.Size.X; x++)
                {
                    Assert.That.VectorsAreEqual(c[x, y], Color.Black);
                }
            }
        }

        [TestMethod]
        public void WritePixel()
        {
            var c = new Canvas(10, 20) { [2, 3] = Color.Red };
            Assert.AreEqual(c[2, 3], Color.Red);
        }

        [TestMethod]
        public void PixmapHeader()
        {
            var c = new Canvas(5, 3);
            var ppm = c.ToPixmap().Split(NewLine);
            Assert.AreEqual(ppm[0], "P3");
            Assert.AreEqual(ppm[1], "5 3");
            Assert.AreEqual(ppm[2], "255");
        }

        [TestMethod]
        public void Pixmap()
        {
            var c = new Canvas(5, 3)
            {
                [0, 0] = new(1.5f, 0f, 0f),
                [2, 1] = new(0f, .5f, 0f),
                [4, 2] = new(-.5f, 0f, 1f)
            };
            var ppm = c.ToPixmap();
            var expected = string.Join(NewLine,
                "P3",
                "5 3",
                "255",
                "255 0 0 0 0 0 0 0 0 0 0 0 0 0 0",
                "0 0 0 0 0 0 0 128 0 0 0 0 0 0 0",
                "0 0 0 0 0 0 0 0 0 0 0 0 0 0 255",
                "");
            Assert.AreEqual(ppm, expected);
        }

        [TestMethod]
        public void PixmapLineNotLongerAs70()
        {
            var c = new Canvas(10, 2);
            c.Fill(new Color(1f, .8f, .6f));

            var expected = string.Join(NewLine,
                "P3",
                "10 2",
                "255",
                "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204",
                "153 255 204 153 255 204 153 255 204 153 255 204 153",
                "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204",
                "153 255 204 153 255 204 153 255 204 153 255 204 153",
                "");
            Assert.AreEqual(expected, c.ToPixmap());
        }

        [TestMethod]
        public void PixmapEndsNewLine()
        {
            var c = new Canvas(5, 3);
            Assert.AreEqual(c.ToPixmap().Last(), NewLine.Last());
        }
    }
}
