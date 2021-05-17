using RayTracer.Environment;
using RayTracer.Materials;
using RayTracer.Shapes;
using System;
using System.Collections.Generic;
using static RayTracer.Extension.MatrixExtension;
using static RayTracer.Extension.VectorExtension;

namespace RayTracer.ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var wallMaterial = new PhongMaterial(new Color(.9f, .9f, .9f)) { Specular = 0f };
            var floorMaterial = new PhongMaterial(new Color(.7f, .7f, .8f)) { Specular = 0f };
            var left = new Plane(Translation(0f, 0f, 10f) * RotationY(MathF.PI / -4f) * RotationX(MathF.PI / 2f)) { Material = wallMaterial };
            var right = new Plane(Translation(0f, 0f, 10f) * RotationY(MathF.PI / 4f) * RotationX(MathF.PI / 2f)) { Material = wallMaterial };
            var floor = new Plane(Translation(0f, -7f, 0f)) { Material = floorMaterial };
            var mescho = RayTracer.Tests.Util.LoadMesitaldehydeAtoms();
            var w = new World
            {
                Objects = new List<Entity> { floor, left, right },
            };
            w.Lights.Add(new PointLight(Point(-7f, 15f, -7f), new Color(1f, 1f, 1.05f)));
            w.Objects.AddRange(mescho);
            var cam = new Camera(1000, 1000, MathF.PI / 3f)
            {
                Transform = Camera.ViewTransform(Point(1f, .5f, -12f), Point(0f, .5f, 0f), Direction(0f, 1f, 0f))
            };
            cam.Render(w).Save("D://profiled.jpg");
        }
    }
}
