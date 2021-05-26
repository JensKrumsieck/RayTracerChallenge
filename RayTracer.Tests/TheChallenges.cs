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
                    canvas[x, y] = shape.Material.Shade(l, ref comps, 0);
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
            var cam = new Camera(10, 5, MathF.PI / 3f)
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
            var cam = new Camera(10, 10, MathF.PI / 3f)
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
            var cam = new Camera(10, 10, MathF.PI / 3f)
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
            var cam = new Camera(20, 10, MathF.PI / 2f)
            { Transform = Camera.ViewTransform(Point(0f, 1f, -2f), Point(0f, 1f, 0f), Direction(0f, 1f, 0f)) };
            cam.Render(w);
        }

        [TestMethod]
        public void Chapter_XI_b()
        {
            var floorMaterial = new PhongMaterial(Color.White)
            {
                IOR = Constants.GlassIOR,
                Reflectivity = 1f
            };
            var skyMaterial = new PhongMaterial(Color.White) { Pattern = new RingPattern(Util.FromHex("#0092ca"), Util.FromHex("#00adb5")) };
            var sky = new Plane(Translation(0, 5, 0)) { Material = skyMaterial };
            var floor = new Plane { Material = floorMaterial };

            var w = new World
            {
                Objects = new List<Entity> { floor, sky }
            };

            for (var i = 0; i < 20; i++)
            {
                var rnd = new Random();
                var x = (float)rnd.NextDouble();
                var y = (float)rnd.NextDouble();
                var z = (float)rnd.NextDouble();
                var col = new Color(x, y, z);
                var mat = new PhongMaterial(col) { IOR = Constants.GlassIOR, Transparency = y, Reflectivity = x, Specular = z };
                var s = new Sphere(Translation((x * 10f) - 5f, y, z * 10f) * Scale(y)) { Material = mat };
                w.Objects.Add(s);
            }
            var l = PointLight.Default;
            l.Intensity *= 2f;
            w.Lights.Add(l);
            var cam = new Camera(20, 10, MathF.PI / 1.5f)
            { Transform = Camera.ViewTransform(Point(0f, 1f, -2f), Point(0f, 1f, 0f), Direction(0f, 1f, 0f)) };
            cam.Render(w);
        }

        [TestMethod]
        public void ChapterXII()
        {
            var cube1 = new Cube(Translation(-8f, 4, 8) * Scale(30, 5, 30))
            {
                Material = new PhongMaterial(Color.White) { Pattern = new CheckerPattern(Util.FromHex("#222831"), Util.FromHex("#d72323"), Scale(.2f)), Specular = .1f }
            };
            var cube2 = new Cube(Translation(-8, 4, 8) * Scale(28, 6, 28))
            {
                Material = new PhongMaterial(Color.White * .4f)
            };

            var sphere1 = new Sphere(Translation(-8f, 2.7f, 8.1f) * Scale(4))
            {
                Material = new PhongMaterial(Util.FromHex("#d72323")) { Reflectivity = 1 }
            };
            var sphere2 = new Sphere(Translation(.9f, 1f, 8f) * Scale(2))
            {
                Material = new PhongMaterial(Color.White) { Transparency = 1f, IOR = Constants.GlassIOR, Reflectivity = .5f }
            };

            var w = new World();
            w.Objects.Add(cube1);
            w.Objects.Add(cube2);
            w.Objects.Add(sphere1);
            w.Objects.Add(sphere2);
            var l = new PointLight(Point(-12, 8, -10), Color.White);
            w.Lights.Add(l);
            var cam = new Camera(192, 108, MathF.PI / 180 * 50)
            {
                Transform = Camera.ViewTransform(Point(19f, 6f, -14f), Point(-8f, 2.7f, 8f), Direction(0f, 1f, 0f))
            };
            cam.Render(w);
        }

        [TestMethod]
        public void ChapterXIII_a()
        {
            var w = new World();
            var p = new Plane()
            {
                Material = new PhongMaterial(Util.FromHex("#393e46")) { Reflectivity = .1f }
            };
            w.Objects.Add(p);
            var boostMat = new PhongMaterial(Util.FromHex("#222831")) { Specular = 0 };
            var c1 = new Cone(Translation(0.363811f, 0.47248f, 0) * RotationZ(MathF.PI) * Scale(.202f, .367f, .202f)) { Minimum = 0, Maximum = 1, IsClosed = true, Material = boostMat };
            var c2 = new Cone(Translation(0f, 0.47248f, -0.386016f) * RotationZ(MathF.PI) * Scale(.202f, .367f, .202f)) { Minimum = 0, Maximum = 1, IsClosed = true, Material = boostMat };
            var c3 = new Cone(Translation(-0.363811f, 0.47248f, 0) * RotationZ(MathF.PI) * Scale(.202f, .367f, .202f)) { Minimum = 0, Maximum = 1, IsClosed = true, Material = boostMat };
            var c4 = new Cone(Translation(0f, 0.47248f, 0.386016f) * RotationZ(MathF.PI) * Scale(.202f, .367f, .202f)) { Minimum = 0, Maximum = 1, IsClosed = true, Material = boostMat };
            w.Objects.Add(c1);
            w.Objects.Add(c2);
            w.Objects.Add(c3);
            w.Objects.Add(c4);

            var baseMat = new PhongMaterial(Color.White)
            {
                Pattern = new StripePattern(Color.White, Color.Black, Scale(.05f) * RotationZ(MathF.PI / 2f)),
                Reflectivity = .7f
            };
            var cyl = new Cylinder(Translation(0, 4.23516f, 0) * Scale(.596f, 3.782f, .596f)) { Minimum = -1f, Maximum = 0f, IsClosed = true, Material = baseMat };
            var con = new Cone(Translation(0f, 4.8f, 0f) * RotationZ(MathF.PI) * Scale(.596f)) { Minimum = 0, Maximum = 1, IsClosed = true, Material = baseMat };
            w.Objects.Add(cyl);
            w.Objects.Add(con);

            var cyl1 = new Cylinder(Translation(0, 5.15554f, 0) * Scale(.411f, 1.104f, .411f)) { Minimum = -1f, Maximum = 0f, IsClosed = true, Material = baseMat };
            var con1 = new Cone(Translation(0f, 5.55f, 0f) * RotationZ(MathF.PI) * Scale(.411f)) { Minimum = 0, Maximum = 1, IsClosed = true, Material = baseMat };
            w.Objects.Add(cyl1);
            w.Objects.Add(con1);

            var cyl2 = new Cylinder(Translation(0, 5.7f, 0) * Scale(.211f, .345f, .211f)) { Minimum = -1f, Maximum = 0f, IsClosed = true, Material = baseMat };
            var con2 = new Cone(Translation(0f, 5.9f, 0f) * RotationZ(MathF.PI) * Scale(.211f)) { Minimum = 0, Maximum = 1, IsClosed = true, Material = baseMat };
            w.Objects.Add(cyl2);
            w.Objects.Add(con2);

            var cyl3 = new Cylinder(Translation(0, 6f, 0) * Scale(.092f, .151f, .092f)) { Minimum = -1f, Maximum = 0f, IsClosed = true, Material = baseMat };
            var con3 = new Cone(Translation(0f, 6.1f, 0f) * RotationZ(MathF.PI) * Scale(.092f)) { Minimum = 0, Maximum = 1, IsClosed = true, Material = baseMat };
            w.Objects.Add(cyl3);
            w.Objects.Add(con3);
            var topMat = new PhongMaterial(Color.White);
            var cyl4 = new Cylinder(Translation(0, 7f, 0) * Scale(.04f, 3f, .04f)) { Minimum = -1f, Maximum = 0f, IsClosed = true, Material = topMat };
            w.Objects.Add(cyl4);

            var cube = new Cube(Translation(-5, 1, -2))
            {
                Material = new PhongMaterial(Color.White)
                { Transparency = 1f, IOR = Constants.GlassIOR, Reflectivity = 0f }
            };
            var s = new Sphere(Translation(-5, .5f, -2) * Scale(.5f)) { Material = new PhongMaterial(Color.Red) { Reflectivity = 1 } };
            var s2 = new Sphere(Translation(5, 1.5f, 2) * Scale(1.5f)) { Material = new PhongMaterial(Color.Red) { Reflectivity = 1 } };
            var cube2 = new Cube(Translation(7, .7f, -2) * Scale(.7f))
            {
                Material = new PhongMaterial(Color.White)
                { Transparency = 1f, IOR = Constants.GlassIOR, Reflectivity = 0f }
            };
            var s3 = new Sphere(Translation(5, 1, -2)) { Material = new PhongMaterial(Color.Blue) { Reflectivity = 1 } };
            var s4 = new Sphere(Translation(-8, 1, 3)) { Material = new PhongMaterial(Color.Green) { Reflectivity = 1 } };
            w.Objects.Add(cube);
            w.Objects.Add(cube2);
            w.Objects.Add(s);
            w.Objects.Add(s2);
            w.Objects.Add(s3);
            w.Objects.Add(s4);
            var l = new PointLight(Point(1.4122f, 10.242f, -10f), Color.White);
            w.Lights.Add(l);
            var cam = new Camera(10, 10, 50 * MathF.PI / 180)
            {
                Transform = Camera.ViewTransform(Point(15.548f, 5f, 0f), Point(0, 1, 0), Vector4.UnitY)
            };
            cam.Render(w);
        }

        [TestMethod]
        public void Chapter_XIII_b()
        {
            var room = new Cube(RotationY(MathF.PI / 3.5f) * Scale(15f, 7f, 15f)) { Material = new PhongMaterial(Util.FromHex("#393e46")) { Reflectivity = 1 } };
            var mescho = Util.LoadMesitaldehydeAtoms();
            var w = new World();
            w.Lights.Add(new PointLight(Point(-5f, 5f, -5f), new Color(1f, 1f, 1.05f)));
            w.Objects.AddRange(mescho);
            w.Objects.Add(room);
            var cam = new Camera(20, 10, MathF.PI / 3f)
            {
                Transform = Camera.ViewTransform(Point(-7f, 1f, -11f), Point(0f, .5f, 0f), Direction(0f, 1f, 0f))
            };
            cam.Render(w);
        }

        [TestMethod]
        public void Chapter_XIV_a()
        {
            var g = new Group();
            var withBoundingBoxes = new World();
            withBoundingBoxes.Objects.Add(g);
            for (var z = 0; z < 10; z++)
            {
                var gy = new Group();
                for (var y = 0; y < 10; y++)
                {
                    var gx = new Group();
                    for (var x = 0; x < 10; x++)
                    {
                        var s = new Sphere(Translation(x, y, z) * Scale(.5f)) { Material = new PhongMaterial(new Color(x, y, z)) { Reflectivity = x / 10f, Transparency = y / 10f, Specular = z / 10f } };
                        gx.AddChild(s);
                    }
                    gy.AddChild(gx);
                }
                g.AddChild(gy);
            }
            g.Divide();
            var l = PointLight.Default;
            withBoundingBoxes.Lights.Add(l);
            var cam = new Camera(10, 10, MathF.PI / 180 * 60)
            {
                Transform = Camera.ViewTransform(Point(-8, .1f, -5), Point(0, 5, 0), Vector4.UnitY)
            };
            cam.Render(withBoundingBoxes);
        }

        [TestMethod]
        public void Chapter_XIV_b()
        {
            var floor = new PhongMaterial(Util.FromHex("#222831"))
            { Reflectivity = .9f, Specular = .5f, Shininess = 300 };
            var p = new Plane(Translation(0, -.25f, 0)) { Material = floor };
            static Group HexagonSide()
            {
                var mat = new PhongMaterial(Util.FromHex("#7bc74d")) { Specular = .5f, Shininess = 800 };
                var edge = new Cylinder(Translation(0, 0, -1) * RotationY(-MathF.PI / 6f) * RotationZ(-MathF.PI / 2f) *
                                        Scale(.25f, 1, .25f))
                { Minimum = 0, Maximum = 1, Material = mat };
                var s = new Sphere(Translation(0, 0, -1) * Scale(.25f)) { Material = mat };
                var side = new Group();
                side.AddChildren(edge, s);
                return side;
            }
            var hex = new Group();
            for (var n = 0; n < 6; n++)
            {
                var side = HexagonSide();
                side.Transform = RotationY(n * MathF.PI / 3f);
                hex.AddChild(side);
            }
            var w = new World();
            w.Objects.Add(hex);
            w.Objects.Add(p);
            var l = PointLight.Default;
            w.Lights.Add(l);
            var cam = new Camera(10, 10, MathF.PI / 180 * 40)
            {
                Transform = Camera.ViewTransform(Point(-2, 3, -2), Point(0, 0, 0), Vector4.UnitY)
            };
            cam.Render(w);
        }

        [TestMethod]
        public void Chapter_XV_a()
        {
            //THE TEAPOT!!!
            const string tea = "files/teapot.obj";
            var parser = new ObjParser(tea);
            var mat = new PhongMaterial(Util.FromHex("#ff304f")) { Reflectivity = .1f };
            parser.Parse();
            parser.Group.Fill(mat);
            parser.Group.Divide();
            var w = World.Default;
            w.Objects.Clear();
            w.Objects.Add(parser.Group);
            var floor = new Plane { Material = new PhongMaterial(Color.White) { Pattern = new CheckerPattern(Util.FromHex("#002651"), Util.FromHex("#775ada")), Reflectivity = .4f } };
            w.Objects.Add(floor);
            var cam = new Camera(10, 10, MathF.PI / 180 * 55)
            {
                Transform = Camera.ViewTransform(Point(0, 3, -7), Point(0, 2, 0), Vector4.UnitY)
            };
            cam.Render(w);
        }

        [TestMethod]
        public void Chapter_XV_b()
        {
            //SUZANNE!!!
            const string monkey = "files/suzanne.obj";
            var parser = new ObjParser(monkey);
            parser.Parse();
            parser.Group.Divide();
            parser.Group.Transform = Translation(-2, 0, 0) * RotationY(MathF.PI / 180 * 170);
            parser.Group.Fill(new PhongMaterial(Util.FromHex("#fc5185")) { Reflectivity = .2f });

            //Smooth Suzanne :)
            const string monkeySmooth = "files/suzanne_smooth.obj";
            var parser2 = new ObjParser(monkeySmooth);
            parser2.Parse();
            parser2.Group.Divide();
            parser2.Group.Transform = Translation(2, 1, 0) * RotationY(MathF.PI / 180 * 10);
            parser2.Group.Fill(new PhongMaterial(Util.FromHex("#364f6b")) { Reflectivity = .4f });

            //Subdivided SMOOTH Suzanne :)
            const string monkeySubdiv = "files/suzanne_subdiv.obj";
            var parser3 = new ObjParser(monkeySubdiv);
            parser3.Parse();
            parser3.Group.Divide();
            parser3.Group.Transform = Translation(0, 0, 5) * Scale(3);
            parser3.Group.Fill(new PhongMaterial(Util.FromHex("#43dde6")) { Reflectivity = .8f });

            var w = World.Default;
            w.Objects.Clear();
            w.Objects.Add(parser.Group);
            w.Objects.Add(parser2.Group);
            w.Objects.Add(parser3.Group);
            var floor = new Plane { Material = new PhongMaterial(Util.FromHex("#f0f0f0")) { Reflectivity = .4f } };
            w.Objects.Add(floor);
            var cam = new Camera(10, 10, MathF.PI / 180 * 55)
            {
                Transform = Camera.ViewTransform(Point(0, 1, -7), Point(0, 3, 5), Vector4.UnitY)
            };
            cam.Render(w);
        }

        [TestMethod]
        public void SuzanneAreaLight()
        {
            //Smooth Suzanne :)
            const string monkeySmooth = "files/suzanne_smooth.obj";
            var parser = new ObjParser(monkeySmooth);
            parser.Parse();
            parser.Group.Divide();
            parser.Group.Transform = Translation(0, 1, 0) * RotationY(MathF.PI / 180 * 120);
            parser.Group.Fill(new PhongMaterial(Util.FromHex("#30e3ca")));
            var w = new World();
            w.Objects.Clear();
            w.Objects.Add(parser.Group);
            var floor = new Plane { Material = new PhongMaterial(Util.FromHex("#f0f0f0")) { Reflectivity = .4f } };
            w.Objects.Add(floor);
            var l = new AreaLight(Point(-1, 2, 4), Direction(4, 0, 4), 2, Direction(0, 4, 0), 2, Color.White);
            w.Lights.Add(l);
            var cam = new Camera(10, 10, MathF.PI / 180 * 75)
            {
                Transform = Camera.ViewTransform(Point(-3, 1, 2.5f), Point(0, .5f, 0), Vector4.UnitY)
            };
            cam.Render(w);
        }
    }
}
