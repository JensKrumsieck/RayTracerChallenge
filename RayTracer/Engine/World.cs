using RayTracer.Engine.Lighting;
using RayTracer.Engine.Material;
using RayTracer.Extension;
using RayTracer.Primitives;
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

        public static readonly World Default = new()
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

        public HitInfo[] Intersections(Ray ray)
        {
            var hits = new ConcurrentBag<HitInfo>();
            Parallel.ForEach(Objects, o =>
            {
                var newHits = o.Intersect(ray);
                foreach (var hit in newHits) hits.Add(hit);
            });
            return hits.OrderBy(s => s.Distance).ToArray();
        }

        public Color Shade(IntersectionPoint p) => Lights.Aggregate(Color.Black, (current, l) => current + p.Object.Material.Shade(l, p.HitPoint, p.Eye, p.Normal));
    }
}
