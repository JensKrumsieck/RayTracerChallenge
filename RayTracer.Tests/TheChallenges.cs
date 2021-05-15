using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Engine;
using RayTracer.Engine.Camera;
using RayTracer.Engine.Lighting;
using RayTracer.Engine.Material;
using RayTracer.Extension;
using RayTracer.Primitives;
using System;
using System.Numerics;
using System.Threading.Tasks;

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
                    canvas.SetPixel(x, y, s.Material.Shade(light, new IntersectionPoint { HitPoint = p, Eye = eye, Normal = normal }));
                }
            });
        }

        [TestMethod]
        public void ChapterVI_extended()
        {
            //same as chap 6 with new classes
            const int canvasSize = 100;
            var s = new Sphere { Material = { BaseColor = new Color(1f, .2f, 1f) } };
            var light = new PointLight(new Vector3(-10f, 10f, -10f), Color.White);
            var cam = new Camera(canvasSize, canvasSize, MathF.PI / 3f)
            {
                Transform = Camera.ViewTransform(
                    new Vector3(0f, 1.5f, -1.5f),
                    s.Position, Vector3.UnitY)
            };
            var w = new World
            {
                Light = light,
                Objects = new Transform[] { s }
            };
            cam.Render(w);
        }

        [TestMethod]
        public void ChapterVII()
        {
            var wallMaterial = new PhongMaterial(new Color(1f, .9f, .9f), .1f, .9f, 0f);
            var wallScale = Matrix.ScaleMatrix(10f, .01f, 10f);
            var wallPos = new Vector3(0f, 0f, 5f);
            var floor = new Sphere
            {
                TransformationMatrix = wallScale,
                Material = wallMaterial,
            };
            var leftWall = new Sphere
            {
                Material = wallMaterial,
                TransformationMatrix = Matrix.TranslationMatrix(wallPos) * Matrix.RotationYMatrix(MathF.PI / -4f) * Matrix.RotationXMatrix(MathF.PI / 2f) * wallScale
            };
            var rightWall = new Sphere
            {
                TransformationMatrix = Matrix.TranslationMatrix(wallPos) * Matrix.RotationYMatrix(MathF.PI / 4f) * Matrix.RotationXMatrix(MathF.PI / 2f) * wallScale,
                Material = wallMaterial
            };
            var right = new Sphere
            {
                TransformationMatrix = Matrix.TranslationMatrix(1.5f, .5f, -.5f) * Matrix.ScaleMatrix(.5f),
                Material = new PhongMaterial(new Color(.5f, 1f, .1f), .1f, .7f, .3f)
            };
            var middle = new Sphere
            {
                TransformationMatrix = Matrix.TranslationMatrix(-.5f, 1f, .5f),
                Material = new PhongMaterial(new Color(.1f, 1f, .5f), .1f, .7f, .3f)
            };
            var left = new Sphere
            {
                TransformationMatrix = Matrix.TranslationMatrix(-1.5f, .33f, -.75f) * Matrix.ScaleMatrix(.33f),
                Material = new PhongMaterial(new Color(1f, .8f, .1f), .1f, .7f, .3f)
            };
            var world = World.Default;
            world.Objects = new Transform[] { left, middle, right, floor, leftWall, rightWall };
            var camera = new Camera(600, 300, MathF.PI /3f)
            {
                Transform = Camera.ViewTransform(
                    new Vector3(0, 1.5f, -5f),
                    Vector3.UnitY, Vector3.UnitY)
            };
            camera.Render(world).Save();
        }
    }
}
