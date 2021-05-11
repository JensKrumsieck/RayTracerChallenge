using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using RayTracer.Primitives;
using Color = RayTracer.Engine.Color;

/*
 * Implemented the System.Numerics Types, too
 * The default implementation takes about 121ms to Render (87 ms without saving an image)
 * System.Numerics: 12 ms (!!), 3 ms without saving an image!
 */

namespace RayTracer.Tests
{
    [TestClass]
    public class Challenges
    {
        [TestMethod]
        public void ChapterV()
        {
            const float wallZ = 10f;
            const float wallSize = 7f;
            const int canvasSize = 100;
            const float pxSize = wallSize / canvasSize;
            const float half = wallSize / 2f;

            var canvas = new Viewport(canvasSize, canvasSize);
            var col = Color.Red;
            var s = new Sphere();
            var origin = Vector3.Point(0f, 0f, -5f);

            Parallel.For(0, canvasSize, y =>
            {
                var worldY = half - pxSize * y;
                for (var x = 0; x < canvasSize; x++)
                {
                    var worldX = -half + pxSize * x;
                    var point = Vector3.Point(worldX, worldY, wallZ);
                    if (Ray.Intersect(origin, (point - origin).Normalized, s, out var hits) &&
                        Transform.Hit(hits) != null)
                    {
                        canvas.SetPixel(x, y, col);
                    }
                }
            });
        }

        [TestMethod]
        public void ChapterV_Native()
        {
            const float wallZ = 10f;
            const float wallSize = 7f;
            const int canvasSize = 100;
            const float pxSize = wallSize / canvasSize;
            const float half = wallSize / 2f;

            var canvas = new Viewport(canvasSize, canvasSize);
            var col = Color.Red;
            var s = new NativeSphere();
            var origin = new System.Numerics.Vector3(0f, 0f, -5f);

            Parallel.For(0, canvasSize, y =>
            {
                var worldY = half - pxSize * y;
                for (var x = 0; x < canvasSize; x++)
                {
                    var worldX = -half + pxSize * x;
                    var point = new System.Numerics.Vector3(worldX, worldY, wallZ);
                    if (NativeRay.Intersect(origin, System.Numerics.Vector3.Normalize(point - origin), s, out var hits) &&
                        NativeTransform.Hit(hits) != null)
                    {
                        canvas.SetPixel(x, y, col);
                    }
                }
            });
        }
    }
}
