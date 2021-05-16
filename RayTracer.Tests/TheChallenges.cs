using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Drawing;
using RayTracer.Shapes;
using System.Numerics;
using System.Threading.Tasks;
using RayTracer.Lights;
using RayTracer.Materials;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.Tests
{
    [TestClass]
    public class PuttingItTogether
    {
        [TestMethod]
        public void ChapterV()
        {
            const float wallZ = 10f;
            const float wallSize = 7f;
            const int px = 100;
            const float pxSize = wallSize / px;
            const float half = wallSize / 2f;
            var canvas = new Canvas(px, px);
            var rayOrigin = Point(0f, 0f, -5f);
            var col = Color.Red;
            var shape = new Sphere();

            Parallel.For(0, px, y =>
            {
                var wY = half - pxSize * y;
                for (var x = 0; x < px; x++)
                {
                    var wx = -half + pxSize * x;
                    var pos = Point(wx, wY, wallZ);
                    var ray = new Ray(rayOrigin, Vector4.Normalize(pos - rayOrigin));
                    if (shape.Hit(ray) != null) canvas[x, y] = col;
                }
            });
        }

        [TestMethod]
        public void ChapterVI()
        {
            const float wallZ = 10f;
            const float wallSize = 7f;
            const int px = 100;
            const float pxSize = wallSize / px;
            const float half = wallSize / 2f;
            var canvas = new Canvas(px, px);
            var rayOrigin = Point(0f, 0f, -5f);
            var shape = new Sphere {Material = new PhongMaterial(new Color(1f, .2f, 1f))};
            var l = new PointLight(Point(-10f, 10f, -10f), Color.White);
            Parallel.For(0, px, y =>
            {
                var wY = half - pxSize * y;
                for (var x = 0; x < px; x++)
                {
                    var wx = -half + pxSize * x;
                    var pos = Point(wx, wY, wallZ);
                    var ray = new Ray(rayOrigin, Vector4.Normalize(pos - rayOrigin));
                    var hit = shape.Hit(ray);
                    if (hit is not { }) continue;
                    var point = ray.PointByDistance(hit.Value.Distance);
                    canvas[x, y] = shape.Material.Shade(l, point, -ray.Direction, hit.Value.Object.Normal(point));
                }
            });
        }
    }
}
