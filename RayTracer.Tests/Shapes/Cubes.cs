using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Shapes;
using System.Numerics;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests.Shapes
{
    [TestClass]
    public class Cubes
    {
        [TestMethod]
        public void RayIntersectsCube()
        {
            var c = new Cube();
            var rays = new Ray[]
            {
                new(5, .5f, 0, -1, 0, 0),
                new(-5, .5f, 0, 1, 0, 0),
                new(.5f, 5, 0, 0, -1, 0),
                new(.5f, -5, 0, 0, 1, 0),
                new(.5f, 0, 5, 0, 0, -1),
                new(.5f, 0, -5, 0, 0, 1),
                new(0, .5f, 0, 0, 0, 1)
            };
            for (var i = 0; i < rays.Length; i++)
            {
                var r = rays[i];
                var xs = c.IntersectLocal(ref r);
                Assert.AreEqual(xs.Count, 2);
                if (i != rays.Length - 1)
                {
                    Assert.AreEqual(xs[0].Distance, 4);
                    Assert.AreEqual(xs[1].Distance, 6);
                }
                else
                {
                    Assert.AreEqual(xs[0].Distance, -1);
                    Assert.AreEqual(xs[1].Distance, 1);
                }
            }
        }

        [TestMethod]
        public void RayMissesCube()
        {
            var c = new Cube();
            var rays = new Ray[]
            {
                new(-2, 0, 0, .2673f, .5345f, .8018f),
                new(0, -2, 0, .8018f, .2673f, .5345f),
                new(0, 0, -2, .5345f, .8018f, .2673f),
                new(2, 0, 2, 0, 0, -1),
                new(0, 2, 2, 0, -1, 0),
                new(2, 2, 0, -1, 0, 0),
                new(0, 0, 2, 0, 0, 1)
            };
            foreach (var t in rays)
            {
                var r = t;
                var xs = c.IntersectLocal(ref r);
                Assert.AreEqual(xs.Count, 0);
            }
        }

        [TestMethod]
        public void CubeNormals()
        {
            var c = new Cube();
            var points = new[]
            {
                (Point(1, .5f, -.8f), Vector4.UnitX),
                (Point(-1, -.2f, .9f), -Vector4.UnitX),
                (Point(-.4f, 1, -.1f), Vector4.UnitY),
                (Point(.3f, -1, -.7f), -Vector4.UnitY),
                (Point(-.6f, .3f, 1), Vector4.UnitZ),
                (Point(.4f, .4f, -1), -Vector4.UnitZ),
                (Point(1, 1, 1), Vector4.UnitX),
                (Point(-1, -1, -1), -Vector4.UnitX),
            };
            foreach (var (point, normal) in points)
            {
                Assert.That.VectorsAreEqual(c.LocalNormal(point), normal);
            }
        }
    }
}
