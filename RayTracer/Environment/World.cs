﻿using RayTracer.Materials;
using RayTracer.Shapes;
using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Extension.MatrixExtension;

namespace RayTracer.Environment
{
    public class World
    {
        public static World Default => new()
        {
            Lights = new List<PointLight> { PointLight.Default },
            Objects = new List<Entity>
            {
                new Sphere
                {
                    Material = new PhongMaterial(new Color(.8f, 1f, .6f)){Diffuse = .7f, Specular = .2f}
                },
                new Sphere(Scale(.5f))
            }
        };


        public List<PointLight> Lights = new();
        public List<Entity> Objects = new();

        public void Intersect(ref Ray ray, ref List<Intersection> hits)
        {
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var entity in Objects)
            {
                entity.Intersect(ref ray, ref hits);
            }
            hits.Sort();
        }

        public Color ShadeHit(ref IntersectionState comps) => comps.Object.Material.Shade(Lights[0], ref comps, InShadow(Lights[0], comps.OverPoint));

        public Color ColorAt(ref Ray ray)
        {
            var xs = new List<Intersection>();
            Intersect(ref ray, ref xs);
            var hit = Intersection.Hit(ref xs);
            if (hit == null)
                return Color.Black;
            var comp = IntersectionState.Prepare(ref hit, ref ray);
            return ShadeHit(ref comp);
        }

        public bool InShadow(PointLight l, Vector4 point)
        {
            var xs = new List<Intersection>();
            var v = l.Position - point;
            var dir = Vector4.Normalize(v);
            var dis = v.LengthSquared();
            var r = new Ray(point, dir);
            foreach (var obj in Objects)
            {
                xs.Clear();
                obj.Intersect(ref r, ref xs);
                var hit = Intersection.Hit(ref xs);
                if (hit != null && hit.Distance * hit.Distance < dis) return true;
            }
            return false;
        }
    }
}