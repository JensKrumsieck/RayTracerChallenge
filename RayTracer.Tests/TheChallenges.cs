using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Drawing;
using RayTracer.Environment;
using RayTracer.Materials;
using RayTracer.Materials.Patterns;
using RayTracer.Shapes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;
using Plane = RayTracer.Shapes.Plane;

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
                    if (shape.Hit(ref ray) != null) canvas[x, y] = col;
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
            var shape = new Sphere { Material = new PhongMaterial(new Color(1f, .2f, 1f)) };
            var l = new PointLight(Point(-10f, 10f, -10f), Color.White);
            Parallel.For(0, px, y =>
            {
                var wY = half - pxSize * y;
                for (var x = 0; x < px; x++)
                {
                    var wx = -half + pxSize * x;
                    var pos = Point(wx, wY, wallZ);
                    var ray = new Ray(rayOrigin, Vector4.Normalize(pos - rayOrigin));
                    var hit = shape.Hit(ref ray);
                    if (hit == null) continue;
                    var comps = IntersectionState.Prepare(ref hit, ref ray);
                    canvas[x, y] = shape.Material.Shade(in l, ref comps);
                }
            });
        }

        [TestMethod]
        public void ChapterVII_III()
        {
            var w = new World();
            var wallMaterial = new PhongMaterial(new Color(1f, .9f, .9f)) { Specular = 0f };
            var wallScale = Scale(10f, .01f, 10f);
            var floor = new Sphere(wallScale) { Material = wallMaterial };
            var leftWall = new Sphere(
                Translation(0f, 0f, 5f) *
                RotationY(MathF.PI / -4f) *
                RotationX(MathF.PI / 2f) *
                wallScale)
            { Material = wallMaterial };
            var rightWall = new Sphere(
                Translation(0f, 0f, 5f) *
                RotationY(MathF.PI / 4f) *
                RotationX(MathF.PI / 2f) *
                wallScale)
            { Material = wallMaterial };
            var middle = new Sphere(Translation(-.5f, 1f, .5f)) { Material = new PhongMaterial(new Color(.1f, 1f, .5f)) { Diffuse = .7f, Specular = .2f } };
            var right = new Sphere(Translation(1.5f, .5f, -.5f) * Scale(.5f)) { Material = new PhongMaterial(new Color(.5f, 1f, .1f)) { Diffuse = .7f, Specular = .2f } };
            var left = new Sphere(Translation(-1.5f, .33f, -.75f) * Scale(.33f)) { Material = new PhongMaterial(new Color(1f, .8f, .1f)) { Diffuse = .7f, Specular = .2f } };
            w.Objects.Add(floor);
            w.Objects.Add(leftWall);
            w.Objects.Add(rightWall);
            w.Objects.Add(middle);
            w.Objects.Add(right);
            w.Objects.Add(left);
            w.Lights.Add(new PointLight(Point(-10f, 10f, -10f), Color.White));
            var cam = new Camera(100, 50, MathF.PI / 3f)
            {
                Transform = Camera.ViewTransform(Point(0f, 1.5f, -5f), Point(0f, 1f, 0f), Direction(0f, 1f, 0f))
            };
            cam.Render(w);
        }

        [TestMethod]
        public void Chapter_IX()
        {
            var wallMaterial = new PhongMaterial(new Color(.9f, .9f, .9f)) { Specular = 0f };
            var floorMaterial = new PhongMaterial(new Color(.7f, .7f, .8f)) { Specular = 0f };
            var left = new Plane(Translation(0f, 0f, 10f) * RotationY(MathF.PI / -4f) * RotationX(MathF.PI / 2f)) { Material = wallMaterial };
            var right = new Plane(Translation(0f, 0f, 10f) * RotationY(MathF.PI / 4f) * RotationX(MathF.PI / 2f)) { Material = wallMaterial };
            var floor = new Plane(Translation(0f, -7f, 0f)) { Material = floorMaterial };
            var mescho = Util.LoadMesitaldehydeAtoms();
            var w = new World
            {
                Objects = new List<Entity> { floor, left, right },
            };
            w.Lights.Add(new PointLight(Point(-7f, 15f, -7f), new Color(1f, 1f, 1.05f)));
            w.Objects.AddRange(mescho);
            var cam = new Camera(100, 100, MathF.PI / 3f)
            {
                Transform = Camera.ViewTransform(Point(1f, .5f, -12f), Point(0f, .5f, 0f), Direction(0f, 1f, 0f))
            };
            cam.Render(w);
        }

        [TestMethod]
        public void Chapter_X()
        {
            var floorMaterial = PhongMaterial.Default;
            floorMaterial.Pattern = new CheckerPattern(Util.FromHex("#bd2c00"), Util.FromHex("#222222"), RotationY(MathF.PI / 4f));
            floorMaterial.Specular = 0f;

            var back = PhongMaterial.Default;
            back.Pattern = new RingPattern(Util.FromHex("#0c3866"), Util.FromHex("#007cc0"));
            back.Specular = 0.5f;
            var sphereMaterial = PhongMaterial.Default;
            sphereMaterial.Pattern = new StripePattern(Util.FromHex("#ffc20e"), Util.FromHex("#ffd54f"))
            { Transform = Scale(.1f) * RotationY(MathF.PI / 2f) };
            sphereMaterial.Diffuse = 1f;
            var floor = new Plane { Material = floorMaterial };
            var s = new Sphere(Translation(0f, 0f, 2f)) { Material = sphereMaterial };
            var backDrop = new Plane(Translation(0f, 0f, 10f) * RotationX(MathF.PI / 2f)) { Material = back };
            var w = new World
            {
                Objects = new List<Entity> { floor, s, backDrop },
            };
            w.Lights.Add(PointLight.Default);
            var cam = new Camera(100, 100, MathF.PI / 3f)
            { Transform = Camera.ViewTransform(Point(0f, 1f, -2f), Point(0f, 1f, 0f), Direction(0f, 1f, 0f)) };
            cam.Render(w);
        }

        [TestMethod]
        public void Chapter_XI_a()
        {
            var floorMaterial = PhongMaterial.Default;
            floorMaterial.Pattern = new CheckerPattern(Util.FromHex("#393e46"), Util.FromHex("#222831"), RotationY(MathF.PI / 4f));
            floorMaterial.Specular = 0f;
            floorMaterial.Reflectivity = 1f;

            var sphereMaterial = PhongMaterial.Default;
            sphereMaterial.Pattern = new StripePattern(Util.FromHex("#7bc74d"), Util.FromHex("#eeeeee"))
            { Transform = Scale(.1f) * RotationY(MathF.PI / 2f) };
            sphereMaterial.Diffuse = 1f;
            var floor = new Plane { Material = floorMaterial };
            var s = new Sphere(Translation(0f, 1f, 2f) * RotationY(MathF.PI / 4f)) { Material = sphereMaterial };

            var w = new World
            {
                Objects = new List<Entity> { floor, s },
            };
            w.Lights.Add(PointLight.Default);
            var cam = new Camera(1000, 1000, MathF.PI / 3f)
            { Transform = Camera.ViewTransform(Point(0f, 1f, -2f), Point(0f, 1f, 0f), Direction(0f, 1f, 0f)) };
            cam.Render(w).Save();
        }
    }
}
