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
                    canvas.SetPixel(x, y, s.Material.Shade(light, p, eye, normal));
                }
            });
        }

        [TestMethod]
        public void ChapterVII()
        {
            var floor = new Sphere
            {
                TransformationMatrix = Matrix4x4.CreateScale(10f, .01f, 10f),
                Material = PhongMaterial.Default.WithColor(new Color(1f, .9f, .9f)).WithSpecular(0f)
            };

            var leftWall = new Sphere
            {
                TransformationMatrix =
                    Matrix4x4.Multiply(
                        Matrix4x4.CreateTranslation(0f, 0f, 5f), Matrix4x4.Multiply(
                        Matrix4x4.Multiply(
                            Matrix4x4.CreateRotationY(-MathF.PI / 2f),
                            Matrix4x4.CreateRotationX(MathF.PI / 2f)),
                    Matrix4x4.CreateScale(10f, .01f, 10f))),
                Material = floor.Material
            };
            var rightWall = new Sphere(new Vector3(-.5f, 1f, .5f))
            {
                TransformationMatrix = 
                    Matrix4x4.Multiply(
                        Matrix4x4.CreateTranslation(0f, 0f, 5f), Matrix4x4.Multiply(
                        Matrix4x4.Multiply(
                            Matrix4x4.CreateRotationY(MathF.PI / 2f),
                            Matrix4x4.CreateRotationX(MathF.PI / 2f)),
                    Matrix4x4.CreateScale(10f, .01f, 10f))),
                Material = PhongMaterial.Default.WithColor(new Color(.1f, 1f, .5f)).WithSpecular(.3f).WithDiffuse(.7f)
            };
            var right = new Sphere(new Vector3(1.5f, .5f, -.5f), .5f)
            {
                Material = PhongMaterial.Default.WithColor(new Color(.5f, 1f, .1f)).WithDiffuse(.7f).WithSpecular(.3f)
            };
            var left = new Sphere(new Vector3(-1.5f, .33f, -.75f), .33f)
            {
                Material = PhongMaterial.Default.WithColor(new Color(1f, .8f, .1f)).WithDiffuse(.7f).WithSpecular(.3f)
            };
            var world = new World
            {
                Objects = new Transform[] { floor, leftWall, rightWall, left, right },
                Lights = new ILight[] { new PointLight(new Vector3(-10f, 10f, -10f), Color.White) }
            };
            var camera = new Camera(1000, 500, MathF.PI / 3)
            {
                TransformationMatrix = Camera.ViewTransform(
                    new Vector3(0, 1.5f, -5f),
                    Vector3.UnitY, Vector3.UnitY)
            };
            var image = camera.Render(world);
            image.Render();
        }
    }
}
