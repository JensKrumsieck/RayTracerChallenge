﻿#nullable enable
using RayTracer.Engine.Lighting;
using RayTracer.Engine.Material;
using RayTracer.Primitives;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace RayTracer.Engine
{
    public class World
    {
        public Transform[] Objects;

        public ILight[] Lights;

        public static World Default => new()
        {
            Lights = new ILight[] { new PointLight(new Vector3(-10, 10, -10), Color.White) },
            Objects = new Transform[]
            {
                new Sphere
                {
                    Material = new PhongMaterial(new Color(.8f, 1f, .6f), .1f, .7f, .2f, 200f)
                },
                new Sphere(.5f)
            }
        };

        public bool Intersections(Ray ray, out HitInfo[] hitInfo, bool doHit = false)
        {
            var hits = new ConcurrentBag<HitInfo>();
            Parallel.ForEach(Objects, o =>
            {
                var newHits = o.Intersect(ray, doHit); //if doHit is true only one intersection is returned
                foreach (var hit in newHits) hits.Add(hit);
            });
            hitInfo = hits.OrderBy(s => s.Distance).ToArray();
            return hitInfo.Length != 0;
        }

        public Color Shade(IntersectionPoint p) => Lights.Aggregate(Color.Black, (current, l) => current + p.Object.Material.Shade(l, p.HitPoint, p.Eye, p.Normal));

        public Color ColorAt(Ray ray)
        {
            Console.WriteLine(Lights[0]);
            if (!Intersections(ray, out var xs, true)) return Color.Black;
            var com = IntersectionPoint.Prepare(xs[0], ray);
            var col = Shade(com);
            return col;
        }
    }
}
