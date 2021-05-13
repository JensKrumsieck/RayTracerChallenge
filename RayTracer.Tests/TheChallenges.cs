using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using RayTracer.Engine.Lighting;
using RayTracer.Primitives;
using System.Numerics;
using System.Threading.Tasks;
using Color = RayTracer.Engine.Color;

namespace RayTracer.Tests
{
    [TestClass]
    public class TheChallenges
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
            var s = new Sphere { Material = { BaseColor = Color.Red } };
            var col = s.Material.BaseColor;
            var origin = new Vector3(0f, 0f, -5f);
            Parallel.For(0, canvasSize, y =>
            {
                var worldY = half - pxSize * y;
                for (var x = 0; x < canvasSize; x++)
                {
                    var worldX = -half + pxSize * x;
                    if (Ray.Hit(origin, Vector3.Normalize(new Vector3(worldX, worldY, wallZ) - origin), s) != null) canvas.SetPixel(x, y, col);
                }
            });
        }

        [TestMethod]
        public void ChapterVI()
        {
            const float wallZ = 10f;
            const float wallSize = 7f;
            const int canvasSize = 100;
            const float pxSize = wallSize / canvasSize;
            const float half = wallSize / 2f;

            var canvas = new Viewport(canvasSize, canvasSize);
            var s = new Sphere { Material = { BaseColor = new Color(1f, .2f, 1f) } };
            var origin = new Vector3(0f, 0f, -5f);
            var light = new PointLight(new Vector3(-10f, 10f, -10f), Color.White);
            Parallel.For(0, canvasSize, y =>
            {
                var worldY = half - pxSize * y;
                for (var x = 0; x < canvasSize; x++)
                {
                    var worldX = -half + pxSize * x;
                    var ray = new Ray(origin, Vector3.Normalize(new Vector3(worldX, worldY, wallZ) - origin));
                    var hit = Ray.Hit(ray, s);
                    if (hit == null) continue;
                    var p = ray.PointByDistance(hit.Distance);
                    var normal = hit.HitObject.Normal(p);
                    var eye = -ray.Direction;
                    canvas.SetPixel(x, y, s.Material.Lighten(light, p, eye, normal));
                }
            });
        }
    }
}
